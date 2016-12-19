using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSendManager : MonoBehaviour
{
    private static ReSendManager instance;

    public static ReSendManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(ReSendManager)) as ReSendManager;
                if (!instance)
                {
                    GameObject container = new GameObject();
                    container.name = "ReSendManager";
                    instance = container.AddComponent(typeof(ReSendManager)) as ReSendManager;
                }
            }

            return instance;
        }
    }

    private Dictionary<int, SendData>[] reSendDatum;
    SendData reSendData;

    List<int> reSendKey;

    public bool characterCreating;
    public bool monsterCreateing;

    public void Initialize(int userNum)
    {
        reSendDatum = new Dictionary<int, SendData>[userNum];

        for (int i = 0; i < userNum; i++)
        {
            reSendDatum[i] = new Dictionary<int, SendData>();
        }

        StartCoroutine(CheckReSendData());
    }

    public void AddReSendData(SendData sendData, int index)
    {
        try
        {
            Debug.Log(index + "번 유저에 " + sendData.UdpId + " 아이디 데이터 추가");
            reSendDatum[index].Add(sendData.UdpId, sendData);
        }
        catch
        {
            Debug.Log("ReSendManager::AddReSendData.Add 에러");
        }
    }

    public void RemoveReSendData(SendData sendData)
    {
        int index = NetworkManager.Instance.GetUserIndex(sendData.EndPoint);

        if (reSendDatum[index].ContainsKey(sendData.UdpId))
        {
            try
            {
                Debug.Log(index + "번 유저에 " + sendData.UdpId + " 아이디 데이터 삭제");
                reSendDatum[index].Remove(sendData.UdpId);
            }
            catch
            {
                Debug.Log("ReSendManager::AddReSendData.Remove 에러");
            }
        }
    }

    public void DataReSend(SendData sendData)
    {
        DataPacket packet = new DataPacket(sendData.Msg, sendData.EndPoint);
        DataSender.Instance.SendMsgs.Enqueue(packet);
    }

    public IEnumerator CheckReSendData()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            //모든 플레이어들의 ReSendData를 확인한다
            for (int userIndex = 0; userIndex < reSendDatum.Length; userIndex++)
            {
                reSendKey = new List<int>(reSendDatum[userIndex].Keys);

                //플레이어의 ReSendData를 확인한다
                for (int i = 0; i < reSendKey.Count; i++)
                {
                    //플레이어의 foreach문에 걸린 method를 하나 실행한다.
                    if (reSendDatum[userIndex].TryGetValue(reSendKey[i], out reSendData))
                    {
                        DataReSend(reSendData);
                    }
                }
            }

            if (characterCreating)
            {
                for (int userIndex = 0; userIndex < reSendDatum.Length; userIndex++)
                {
                    if (reSendDatum[userIndex].Count != 0)
                    {
                        characterCreating = true;
                        break;
                    }
                    else
                    {
                        characterCreating = false;
                    }
                }

                if (!characterCreating)
                {
                    GameObject character = GameObject.FindWithTag("Player");
                    StartCoroutine(DataSender.Instance.CharacterPositionSend(character));
                }
            }

            if (monsterCreateing)
            {
                for (int userIndex = 0; userIndex < reSendDatum.Length; userIndex++)
                {
                    if (reSendDatum[userIndex].Count != 0)
                    {
                        monsterCreateing = true;
                        break;
                    }
                    else
                    {
                        monsterCreateing = false;
                    }
                }

                if (!monsterCreateing)
                {
                    for (int i = 0; i < DungeonManager.Instance.Monsters.Length; i++)
                    {
                        StartCoroutine(DataSender.Instance.UnitPositionSend(DungeonManager.Instance.Monsters[i]));
                    }                    
                }
            }
        }
    }
}

public class SendData
{
    int udpId;
    EndPoint endPoint;
    byte[] msg;

    public int UdpId { get { return udpId; } }
    public EndPoint EndPoint { get { return endPoint; } }
    public byte[] Msg { get { return msg; } }

    public SendData()
    {
        udpId = 0;
        endPoint = null;
        msg = new byte[0];
    }

    public SendData(int newUdpId, EndPoint newEndPoint)
    {
        udpId = newUdpId;
        endPoint = newEndPoint;
        msg = new byte[0];
    }

    public SendData(int newUdpId, EndPoint newEndPoint, byte[] newMsg)
    {
        udpId = newUdpId;
        endPoint = newEndPoint;
        msg = newMsg;
    }
}