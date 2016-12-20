using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance = null;
    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
            }

            return instance;
        }
    }
    //패킷의 길이
    //패킷이 어디서 오는지
    //패킷의 종류
    //Udp receive 확인 번호
    public const int packetLength = 2;
    public const int packetSource = 1;
    public const int packetId = 1;
    public const int udpId = 4;

    //패킷이 어디서 오는지 - 서버/클라이언트
    public enum Source
    {
        ServerSource = 0,
        ClientSource = 1,
    }

    public const string serverIP = "211.186.39.207";
    public const int serverPortNumber = 8800;
    public const int clientPortNumber = 9000;
    public IPEndPoint serverEndPoint;
    public IPEndPoint clientEndPoint;
    
    Queue<DataPacket> receiveMsgs;
    Queue<DataPacket> sendMsgs;

    Socket clientSock;
    Socket serverSock;

    GameManager gameManager;
    DataReceiver dataReceiver;
    DataHandler dataHandler;
    DataSender dataSender;
    ReSendManager reSendManager;

    List<UserIndex> userIndex;
    [SerializeField] int myIndex;
    bool isHost;

    public bool IsHost { get { return isHost; } }
    public int MyIndex { get { return myIndex; } }
    public Socket ServerSock { get { return serverSock; } }
    public Socket ClientSock { get { return clientSock; } }
    public List<UserIndex> UserIndex { get { return userIndex; } }
    public DataReceiver DataReceiver { get { return dataReceiver; } }
    public DataHandler DataHandler { get { return dataHandler; } }
    public DataSender DataSender { get { return dataSender; } }
    public ReSendManager ReSendManager { get { return reSendManager; } }

    public void InitializeManager()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        receiveMsgs = new Queue<DataPacket>();
        sendMsgs = new Queue<DataPacket>();

        InitializeTcpConnection();

        dataReceiver = GetComponent<DataReceiver>();
        dataHandler = GetComponent<DataHandler>();
        dataSender = GetComponent<DataSender>();

        dataReceiver.Initialize(receiveMsgs, serverSock);
        dataHandler.Initialize(receiveMsgs, sendMsgs);
        dataSender.Initialize(sendMsgs, serverSock);
    }

    public void InitializeTcpConnection()
    {
        serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPortNumber);
        serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		 
        ConnectServer();
    }

    public void InitializeUdpConnection()
    {
        clientEndPoint = new IPEndPoint(IPAddress.Parse(gameManager.MyIP), clientPortNumber + myIndex);
        clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        clientSock.Bind(clientEndPoint);

        userIndex = new List<UserIndex>();
        reSendManager = GetComponent<ReSendManager>();

        DataReceiver.SetUdpSocket(clientSock);
        dataReceiver.StartUdpReceive();
    }

    public void ConnectServer()
    {
        try
        {
            serverSock.Connect(serverEndPoint);
            Debug.Log("서버 연결 성공");
        }
        catch (Exception e)
        {
            Debug.Log("서버 연결 실패" + e.Message);
        }
    }

    public void ConnectP2P(EndPoint endPoint)
    {
        dataSender.RequestConnectionCheck(endPoint);
    }

    public void DisconnectP2P()
    {

    }

    public void SocketClose()
    {
        Debug.Log("소켓 닫기");
        clientSock.Close();
        serverSock.Close();
    }

    public int GetUserIndex(EndPoint endPoint)
    {
        for (int index = 0; index < userIndex.Count; index++)
        {
            if (userIndex[index].EndPoint.ToString() == endPoint.ToString())
            {
                return index;
            }
        }

        return -1;
    }

    public void SetHost()
    {
        isHost = true;
    }

    public void SetMyIndex(int newIndex)
    {
        Debug.Log("내 번호 설정 : " + newIndex);
        myIndex = newIndex;
    }
}

public class UserIndex
{
    EndPoint endPoint;
    int userNum;

    public EndPoint EndPoint { get { return endPoint; } }
    public int UserNum { get { return userNum; } }

    public UserIndex()
    {
        endPoint = null;
        userNum = -1;
    }

    public UserIndex(EndPoint newEndPoint, int newUserNum)
    {
        endPoint = newEndPoint;
        userNum = newUserNum;
    }
}