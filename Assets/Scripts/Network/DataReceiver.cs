using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;

public class DataReceiver : MonoBehaviour
{
    Socket tcpSock;
    Socket udpSock;

    Queue<DataPacket> msgs;

    object receiveLock;

    //클래스 초기화
    public void Initialize(Queue<DataPacket> receiveMsgs, Socket newSock, object newLock)
    {
        msgs = receiveMsgs;
        tcpSock = newSock;
        receiveLock = newLock;
        StartTcpReceive();
    }

    //Tcp (서버) 수신 시작
    public void StartTcpReceive()
    {
        AsyncData asyncData = new AsyncData(tcpSock);

        //패킷 헤더 중 패킷의 길이 (2) 만큼 데이터를 받는다
        tcpSock.BeginReceive(asyncData.msg, 0, NetworkManager.packetLength, SocketFlags.None, new AsyncCallback(TcpReceiveLengthCallback), asyncData);
    }

    //Tcp 길이 수신
    public void TcpReceiveLengthCallback(IAsyncResult asyncResult)
    {
        AsyncData asyncData = (AsyncData)asyncResult.AsyncState;

        try
        {
            asyncData.msgSize = (short)tcpSock.EndReceive(asyncResult);
        }
        catch
        {
            Debug.Log("NetworkManager::TcpReceiveLengthCallback.EndReceive 에러");
            Debug.Log("서버 연결 끊김");
            tcpSock.Close();
            return;
        }

        if (asyncData.msgSize <= 0)
        {
            Debug.Log("서버와 연결이 끊겼습니다.");
            tcpSock.Close();
            return;
        }
        
        if (asyncData.msgSize >= NetworkManager.packetLength)
        {
            try
            {   //데이터 길이 변환에 성공하면 데이터를 받는다
                //남은 데이터는 데이터 출처 + 데이터 아이디 + 데이터
                short msgSize = BitConverter.ToInt16(asyncData.msg, 0);
                asyncData = new AsyncData(tcpSock);
                tcpSock.BeginReceive(asyncData.msg, 0, msgSize + NetworkManager.packetSource + NetworkManager.packetId, SocketFlags.None, new AsyncCallback(TcpReceiveDataCallback), asyncData);
            }
            catch
            {   //데이터 길이 변환 실패시 다시 데이터 길이를 받는다
                Debug.Log("DataReceiver::TcpReceiveLengthReceive.BitConverter 에러");
                asyncData = new AsyncData(tcpSock);
                tcpSock.BeginReceive(asyncData.msg, 0, NetworkManager.packetLength, SocketFlags.None, new AsyncCallback(TcpReceiveLengthCallback), asyncData);
            }
        }
        else
        {   //데이터 길이를 받지 못했을 시 다시 데이터 길이를 받는다
            asyncData = new AsyncData(tcpSock);
            tcpSock.BeginReceive(asyncData.msg, 0, NetworkManager.packetLength, SocketFlags.None, new AsyncCallback(TcpReceiveLengthCallback), asyncData);
        }
    }

    //Tcp 데이터 수신
    void TcpReceiveDataCallback(IAsyncResult asyncResult)
    {
        AsyncData asyncData = (AsyncData)asyncResult.AsyncState;

        try
        {
            asyncData.msgSize = (short)tcpSock.EndReceive(asyncResult);
        }
        catch
        {
            Debug.Log("NetworkManager::HandleAsyncDataReceive.EndReceive 에러");
            Debug.Log("서버 연결 끊김");
            tcpSock.Close();
            return;
        }

        //Debug.Log("받은 메시지 길이 : " + asyncData.msgSize);

        if (asyncData.msgSize <= 0)
        {
            Debug.Log("서버와 연결이 끊겼습니다.");
            tcpSock.Close();
            return;
        }

        if (asyncData.msgSize >= NetworkManager.packetSource + NetworkManager.packetId)
        {
            Array.Resize(ref asyncData.msg, asyncData.msgSize);

            DataPacket packet = new DataPacket(asyncData.msg, null);

            try
            {
                //lock (receiveLock)
                //{
                    msgs.Enqueue(packet);
                //}
            }
            catch
            {
                Console.WriteLine("NetworkManager::HandleAsyncDataReceive.Enqueue 에러");
            }
        }

        //재 수신
        asyncData = new AsyncData(tcpSock);
        tcpSock.BeginReceive(asyncData.msg, 0, NetworkManager.packetLength, SocketFlags.None, new AsyncCallback(TcpReceiveLengthCallback), asyncData);
    }

    public void SetUdpSocket(Socket newSock)
    {
        udpSock = newSock;
    }

    //Udp (클라이언트) 수신 시작
    public void StartUdpReceive()
    {
        //매개변수로 받은 리스트의 EndPoint에서 비동기 수신을 대기한다
        AsyncData asyncData = new AsyncData(new IPEndPoint(IPAddress.Any, 0));
        udpSock.BeginReceiveFrom(asyncData.msg, 0, AsyncData.msgMaxSize, SocketFlags.None, ref asyncData.EP, new AsyncCallback(UdpReceiveDataCallback), asyncData);

        Debug.Log("UDP 수신시작");
    }

    //Udp 데이터 수신
    public void UdpReceiveDataCallback(IAsyncResult asyncResult)
    {
        AsyncData asyncData = (AsyncData)asyncResult.AsyncState;

        try
        {
			asyncData.msgSize = (short)udpSock.EndReceiveFrom(asyncResult, ref asyncData.EP);
        }
        catch (Exception e)
        {
            Debug.Log("연결 끊김 :" + e.Message);
        }

        if (asyncData.msgSize > 0)
        {
            Array.Resize(ref asyncData.msg, asyncData.msgSize);

            while (asyncData.msg.Length > 0)
            {
                byte[] msgSize = ResizeByteArray(0, NetworkManager.packetLength, ref asyncData.msg);
                asyncData.msgSize = (short)(BitConverter.ToInt16(msgSize, 0) + NetworkManager.packetSource + NetworkManager.packetId + NetworkManager.udpId);

                byte[] msg = ResizeByteArray(0, asyncData.msgSize, ref asyncData.msg);
                DataPacket packet = new DataPacket(msg, asyncData.EP);

                //lock (receiveLock)
                //{
                    msgs.Enqueue(packet);
                //}
            }
        }

        asyncData = new AsyncData(asyncData.EP);
        udpSock.BeginReceiveFrom(asyncData.msg, 0, AsyncData.msgMaxSize, SocketFlags.None, ref asyncData.EP, new AsyncCallback(UdpReceiveDataCallback), asyncData);
    }

    //index 부터 length만큼을 잘라 반환하고 매개변수 배열을 남은 만큼 잘라서 반환한다
    public static byte[] ResizeByteArray(int index, int length, ref byte[] array)
    {
        //desArray = 자르고 싶은 배열
        //sourArray = 자르고 남은 배열 => 원래 배열
        //ref 연산자로 원래 배열을 변경하게 된다.

        byte[] desArray = new byte[length];
        byte[] sourArray = new byte[array.Length - length];

        Array.Copy(array, index, desArray, 0, length);
        Array.Copy(array, length + index, sourArray, 0, array.Length - length - index);
        array = sourArray;

        return desArray;
    }
}

//비동기 콜백을 위한 클래스
public class AsyncData
{
    public EndPoint EP;
    public byte[] msg;
    public short msgSize;
    public const int msgMaxSize = 2048;

    public AsyncData(Socket newSock)
    {
        EP = null;
        msgSize = 0;
        msg = new byte[msgMaxSize];
    }

    public AsyncData(EndPoint newEndPoint)
    {
        EP = newEndPoint;
        msgSize = 0;
        msg = new byte[msgMaxSize];
    }
}

public class DataPacket
{
    public byte[] msg;
    public EndPoint endPoint;

    public DataPacket(byte[] newMsg, EndPoint newEndPoint)
    {
        msg = newMsg;
        endPoint = newEndPoint;
    }
}