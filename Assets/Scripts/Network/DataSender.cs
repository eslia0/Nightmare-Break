using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class DataSender : MonoBehaviour
{
    private static DataSender instance;

    public static DataSender Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(DataSender)) as DataSender;
                if (!instance)
                {
                    GameObject container = new GameObject();
                    container.name = "DataSender";
                    instance = container.AddComponent(typeof(DataSender)) as DataSender;
                }
            }

            return instance;
        }
    }

    Socket tcpSock;
    Socket udpSock;

    Queue<DataPacket> sendMsgs;

    public int[] udpId;
    byte[] udpMsg;

    public Queue<DataPacket> SendMsgs { get { return sendMsgs; } }

    public void Initialize(Queue<DataPacket> newSendMsgs, Socket newTcpSock)
    {
        sendMsgs = newSendMsgs;
        InitializeTcpSend(newTcpSock);
        StartCoroutine(DataSend());
    }

    public void InitializeTcpSend(Socket newSocket)
    {
        tcpSock = newSocket;
    }
    
    public void InitializeUdpSend(Socket newSocket)
    {
        udpSock = newSocket;
    }

    //데이타를 전송하는 메소드. byte[] msg 를 newIPEndPoint로 전송한다.
    public IEnumerator DataSend()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.016f);

            int readCount = sendMsgs.Count;

            for (int i = 0; i < readCount; i++)
            {
                DataPacket packet;

                packet = sendMsgs.Dequeue();

                if (packet.endPoint != null)
                {
                    udpSock.SendTo(packet.msg, 0, packet.msg.Length, SocketFlags.None, packet.endPoint);
                }
                else if (packet.endPoint == null)
                {
                    tcpSock.Send(packet.msg, 0, packet.msg.Length, SocketFlags.None);
                }
            }
        }
    }

    //비동기 콜백 메소드
    private void SendData(IAsyncResult ar)
    {
        udpSock.EndSendTo(ar);
    }

    //연결 확인 답장 -> Server
    public void ServerConnectionAnswer()
    {
        ResultData resultData = new ResultData();
        ResultPacket resultPacket = new ResultPacket(resultData);
        resultPacket.SetPacketId((int)ClientPacketId.ServerConnectionAnswer);

        DataPacket packet = new DataPacket(CreatePacket(resultPacket), null);
        sendMsgs.Enqueue(packet);
    }

    //계정 생성 -> Server
    public void CreateAccount(string id, string pw)
    {
        Debug.Log("가입 요청");

        AccountData accountData = new AccountData(id, pw);
        AccountPacket accountDataPacket = new AccountPacket(accountData);
        accountDataPacket.SetPacketId((int)ClientPacketId.CreateAccount);

        DataPacket packet = new DataPacket(CreatePacket(accountDataPacket), null);
        sendMsgs.Enqueue(packet);
    }

    //계정 탈퇴 -> Server
    public void DeleteAccount(string id, string pw)
    {
        Debug.Log("탈퇴 요청");

        AccountData accountData = new AccountData(id, pw);
        AccountPacket accountDataPacket = new AccountPacket(accountData);
        accountDataPacket.SetPacketId((int)ClientPacketId.DeleteAccount);

        DataPacket packet = new DataPacket(CreatePacket(accountDataPacket), null);
        sendMsgs.Enqueue(packet);
    }

    //로그인 -> Server
    public void Login(string id, string pw)
    {
        Debug.Log("로그인");

        AccountData accountData = new AccountData(id, pw);
        AccountPacket accountDataPacket = new AccountPacket(accountData);
        accountDataPacket.SetPacketId((int)ClientPacketId.Login);

        DataPacket packet = new DataPacket(CreatePacket(accountDataPacket), null);
        sendMsgs.Enqueue(packet);
    }

    //로그아웃 -> Server
    public void Logout()
    {
        Debug.Log("로그아웃");

        ResultData resultData = new ResultData();
        ResultPacket resultPacket = new ResultPacket(resultData);
        resultPacket.SetPacketId((int)ClientPacketId.Logout);

        DataPacket packet = new DataPacket(CreatePacket(resultPacket), null);
        sendMsgs.Enqueue(packet);
    }

    //게임 종료 -> Server
    public void GameClose()
    {
        Debug.Log("게임 종료");

        ResultData resultData = new ResultData();
        ResultPacket resultDataPacket = new ResultPacket(resultData);
        resultDataPacket.SetPacketId((int)ClientPacketId.GameClose);

        byte[] msg = CreatePacket(resultDataPacket);

        try
        {
            tcpSock.Send(msg, 0, msg.Length, SocketFlags.None);
        }
        catch
        {
            Debug.Log("GameClose.Send 에러");
        }

        try
        {
            tcpSock.Close();
            udpSock.Close();
        }
        catch
        {
            Debug.Log("이미 소켓이 닫혀있습니다.");
        }
    }

    //캐릭터 리스트 요청 -> Server
    public void RequestCharacterList()
    {
        Debug.Log("캐릭터 리스트 요청");

        ResultData resultData = new ResultData();
        ResultPacket resultPacket = new ResultPacket(resultData);
        resultPacket.SetPacketId((int)ClientPacketId.RequestCharacterList);

        DataPacket packet = new DataPacket(CreatePacket(resultPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //캐릭터 생성 -> Server
    public void CreateCharacter(int gender, int hClass, string name)
    {
        Debug.Log("캐릭터 생성");

        CreateCharacterData createCharacterData = new CreateCharacterData((byte)gender, (byte)hClass, name);
        CreateCharacterPacket createCharacterPacket = new CreateCharacterPacket(createCharacterData);
        createCharacterPacket.SetPacketId((int)ClientPacketId.CreateCharacter);

        DataPacket packet = new DataPacket(CreatePacket(createCharacterPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //캐릭터 삭제 -> Server
    public void DeleteCharacter(int index)
    {
        Debug.Log("캐릭터 삭제");

        CharacterIndexData characterIndexData = new CharacterIndexData((byte)index);
        CharacterIndexPacket characterIndexPacket = new CharacterIndexPacket(characterIndexData);
        characterIndexPacket.SetPacketId((int)ClientPacketId.DeleteCharacter);

        DataPacket packet = new DataPacket(CreatePacket(characterIndexPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //캐릭터 정보 요청 -> Server
    public void RequestCharacterStatus()
    {
        Debug.Log("캐릭터 정보 요청");

        CharacterIndexData characterIndexData = new CharacterIndexData((byte)UIManager.Instance.SelectUIManager.CurrentCharacterIndex);
        CharacterIndexPacket characterIndexPacket = new CharacterIndexPacket(characterIndexData);
        characterIndexPacket.SetPacketId((int)ClientPacketId.RequestCharacterStatus);

        DataPacket packet = new DataPacket(CreatePacket(characterIndexPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //방 목록 요청 -> Server
    public void RequestRoomList()
    {
        Debug.Log("방 목록 요청");

        ResultData resultData = new ResultData();
        ResultPacket resultPacket = new ResultPacket(resultData);
        resultPacket.SetPacketId((int)ClientPacketId.RequestRoomList);

        DataPacket packet = new DataPacket(CreatePacket(resultPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //선택 창으로 돌아가기 -> Server
    public void ReturnToSelect()
    {
        Debug.Log("선택 창으로 돌아가기");

        ResultData resultData = new ResultData();
        ResultPacket resultPacket = new ResultPacket(resultData);
        resultPacket.SetPacketId((int)ClientPacketId.ReturnToSelect);

        DataPacket packet = new DataPacket(CreatePacket(resultPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //스킬 투자 -> Server
    public void SkillUp(int index)
    {
        Debug.Log("스킬 투자");

        SkillUpData skillUpData = new SkillUpData(index);
        SkillUpPacket skillUpPacket = new SkillUpPacket(skillUpData);
        skillUpPacket.SetPacketId((int)ClientPacketId.RequestRoomList);

        DataPacket packet = new DataPacket(CreatePacket(skillUpPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //장비 강화 -> Server
    public void EquipUpgrade(int index)
    {
        Debug.Log("장비 강화");

        EquipUpgradeData equipUpgradeData = new EquipUpgradeData(index);
        EquipUpgradePacket equipUpgradePacket = new EquipUpgradePacket(equipUpgradeData);
        equipUpgradePacket.SetPacketId((int)ClientPacketId.EquipUpgrade);

        DataPacket packet = new DataPacket(CreatePacket(equipUpgradePacket), null);

        sendMsgs.Enqueue(packet);
    }

    //방 생성 -> Server
    public void CreateRoom(string roomName, int dungeonId, int dungeonLevel)
    {
        Debug.Log("방 생성");

        CreateRoomData createRoomData = new CreateRoomData(dungeonId, dungeonLevel, roomName);
        CreateRoomPacket createRoomPacket = new CreateRoomPacket(createRoomData);
        createRoomPacket.SetPacketId((int)ClientPacketId.CreateRoom);

        DataPacket packet = new DataPacket(CreatePacket(createRoomPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //방 입장 -> Server
    public void EnterRoom(int roomNum)
    {
        Debug.Log("방 입장 : " + roomNum);

        EnterRoomData enterRoomData = new EnterRoomData(roomNum);
        EnterRoomPacket enterRoomPacket = new EnterRoomPacket(enterRoomData);
        enterRoomPacket.SetPacketId((int)ClientPacketId.EnterRoom);

        DataPacket packet = new DataPacket(CreatePacket(enterRoomPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //방 퇴장 -> Server
    public void ExitRoom(int roomNum)
    {
        Debug.Log("방 퇴장");

        ExitRoomData exitRoomData = new ExitRoomData(roomNum);
        ExitRoomPacket exitRoomPacket = new ExitRoomPacket(exitRoomData);
        exitRoomPacket.SetPacketId((int)ClientPacketId.ExitRoom);

        DataPacket packet = new DataPacket(CreatePacket(exitRoomPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //방 유저 정보 요청 -> Server
    public void RequestRoomUserData(int roomNum)
    {
        Debug.Log("방 유저 정보 요청");

        RoomNumberData roomNumberData = new RoomNumberData(roomNum);
        RoomNumberPacket roomNumberPacket = new RoomNumberPacket(roomNumberData);
        roomNumberPacket.SetPacketId((int)ClientPacketId.RequestRoomUserData);

        DataPacket packet = new DataPacket(CreatePacket(roomNumberPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //게임 시작 -> Server
    public void StartGame()
    {
        Debug.Log("게임 시작");

        ResultData resultData = new ResultData();
        ResultPacket resultPacket = new ResultPacket(resultData);
        resultPacket.SetPacketId((int)ClientPacketId.StartGame);

        DataPacket packet = new DataPacket(CreatePacket(resultPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //UDP 연결 요청 -> Server
    public void RequestUdpConnection()
    {
        Debug.Log("Udp 연결 요청");

        ResultData resultData = new ResultData();
        ResultPacket resultPacket = new ResultPacket(resultData);
        resultPacket.SetPacketId((int)ClientPacketId.RequestUdpConnection);

        DataPacket packet = new DataPacket(CreatePacket(resultPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //던전 몬스터 소환 데이터 요청 -> Server
    public void RequestMonsterSpawnList(int dungeonId, int dungeonLevel)
    {
        Debug.Log("몬스터 소환 데이터 요청");

        RequestDungeonData requestDungeonData = new RequestDungeonData((byte)dungeonId, (byte)dungeonLevel);
        RequestDungeonDataPacket requestDungeonDataPacket = new RequestDungeonDataPacket(requestDungeonData);
        requestDungeonDataPacket.SetPacketId((int)ClientPacketId.RequestMonsterSpawnList);

        DataPacket packet = new DataPacket(CreatePacket(requestDungeonDataPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //던전 데이터 요청 -> Server
    public void RequestMonsterStatusData(int dungeonId, int dungeonLevel)
    {
        Debug.Log("던전 데이터 요청");

        RequestDungeonData requestDungeonData = new RequestDungeonData((byte)dungeonId, (byte)dungeonLevel);
        RequestDungeonDataPacket requestDungeonDataPacket = new RequestDungeonDataPacket(requestDungeonData);
        requestDungeonDataPacket.SetPacketId((int)ClientPacketId.RequestMonsterStatusData);

        DataPacket packet = new DataPacket(CreatePacket(requestDungeonDataPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //UDP 연결 확인 요청 -> Client
    public void RequestConnectionCheck(EndPoint endPoint)
    {
        Debug.Log(endPoint.ToString() + " 연결 체크 요청");

        int index = NetworkManager.Instance.GetUserIndex(endPoint);

        ResultData resultData = new ResultData(new byte());
        ResultPacket resultDataPacket = new ResultPacket(resultData);
        resultDataPacket.SetPacketId((int)P2PPacketId.RequestConnectionCheck);

        DataPacket packet = new DataPacket(CreateUdpPacket(resultDataPacket, udpId[index]), endPoint);

        sendMsgs.Enqueue(packet);

        SendData sendData = new SendData(udpId[index], endPoint, packet.msg);
        NetworkManager.Instance.ReSendManager.AddReSendData(sendData, index);
        udpId[index]++;
    }

    //Udp 답신 -> Client
    public void UdpAnswer(EndPoint newEndPoint, int udpId)
    {
        Debug.Log(newEndPoint.ToString() + " Udp 답신 보냄 Id : " + udpId);

        ResultData resultData = new ResultData(new byte());
        ResultPacket resultDataPacket = new ResultPacket(resultData);
        resultDataPacket.SetPacketId((int)P2PPacketId.UdpAnswer);

        DataPacket packet = new DataPacket(CreateUdpPacket(resultDataPacket, udpId), newEndPoint);

        sendMsgs.Enqueue(packet);
    }

    //연결 완료 -> Server
    public void LoadingComplete()
    {
        Debug.Log("Udp 연결 완료");

        ResultData resultData = new ResultData();
        ResultPacket resultPacket = new ResultPacket(resultData);
        resultPacket.SetPacketId((int)ClientPacketId.LoadingComplete);

        DataPacket packet = new DataPacket(CreatePacket(resultPacket), null);

        sendMsgs.Enqueue(packet);
    }

    //캐릭터의 생성 -> Client
    public void CreateUnitSend(EndPoint endPoint, byte unitType, short characterId, byte unitIndex, float posX, float posY, float posZ)
    {
        Debug.Log(endPoint.ToString() + "캐릭터 생성 보냄 아이디 " + characterId);

        CreateUnitData createUnitData = new CreateUnitData(unitType, characterId, unitIndex, posX, posY, posZ);
        CreateUnitPacket createUnitDataPacket = new CreateUnitPacket(createUnitData);
        createUnitDataPacket.SetPacketId((int)P2PPacketId.CreateUnit);

        int index = NetworkManager.Instance.GetUserIndex(endPoint);

        DataPacket packet = new DataPacket(CreateUdpPacket(createUnitDataPacket, udpId[index]), endPoint);
        sendMsgs.Enqueue(packet);

        SendData sendData = new SendData(udpId[index], endPoint, packet.msg);
        NetworkManager.Instance.ReSendManager.AddReSendData(sendData, index);
        ReSendManager.Instance.characterCreating = true;
        udpId[index]++;
    }

    //캐릭터 위치 -> Client
    public IEnumerator CharacterPositionSend(CharacterManager unit)
    {
        Debug.Log("캐릭터 위치 보내기 시작");

        while (unit != null)
        {
            bool dir = unit.charDir;
            float xPos = unit.transform.position.x;
            float yPos = unit.transform.position.y;
            float zPos = unit.transform.position.z;

            UnitPositionData unitPositionData = new UnitPositionData((byte)UnitType.Hero, dir, xPos, yPos, zPos, (byte)NetworkManager.Instance.MyIndex);
            UnitPositionPacket unitPositionPacket = new UnitPositionPacket(unitPositionData);
            unitPositionPacket.SetPacketId((int)P2PPacketId.UnitPosition);

            for (int index = 0; index < NetworkManager.Instance.UserIndex.Count; index++)
            {
                if (NetworkManager.Instance.MyIndex != index)
                {
                    byte[] msg = CreateUdpPacket(unitPositionPacket, udpId[index]);
                    
                    DataPacket packet = new DataPacket(msg, NetworkManager.Instance.UserIndex[index].EndPoint);
                    sendMsgs.Enqueue(packet);
                }
            }

            yield return new WaitForSeconds(0.016f);
        }

        Debug.Log("캐릭터 위치 보내기 종료");
    }

    //유닛 위치 -> Client
    public IEnumerator UnitPositionSend(GameObject unit)
    {
        Debug.Log("몬스터 위치 보내기 시작");
        Monster monster = unit.GetComponent<Monster>();

        while (true)
        {
            yield return new WaitForSeconds(0.016f);

            bool dir = monster.Direction;
            float xPos = monster.transform.position.x;
            float yPos = monster.transform.position.y;
            float zPos = monster.transform.position.z;

            UnitPositionData unitPositionData = new UnitPositionData((byte)UnitType.Monster, dir, xPos, yPos, zPos, (byte)monster.MonsterIndex);
            UnitPositionPacket unitPositionPacket = new UnitPositionPacket(unitPositionData);
            unitPositionPacket.SetPacketId((int)P2PPacketId.UnitPosition);

            byte[] msg = CreatePacket(unitPositionPacket);

            for (int index = 0; index < NetworkManager.Instance.UserIndex.Count; index++)
            {
                Debug.Log(index + ", " + NetworkManager.Instance.MyIndex);
                if (NetworkManager.Instance.MyIndex != index)
                {
                    Debug.Log("몬스터 위치 보냄");
                    DataPacket packet = new DataPacket(msg, NetworkManager.Instance.UserIndex[index].EndPoint);
                    sendMsgs.Enqueue(packet);
                }
            }
        }
    }

    //캐릭터 움직임(공격, 점프, 스킬 등등) -> Client
    public void CharacterStateSend(int state, int unitIndex)
    {
        UnitStateData unitStateData = new UnitStateData((byte)UnitType.Hero, state, (byte)unitIndex);
        UnitStatePacket unitStatePacket = new UnitStatePacket(unitStateData);
        unitStatePacket.SetPacketId((int)P2PPacketId.UnitState);

        byte[] msg = CreatePacket(unitStatePacket);
        
        for (int index = 0; index < NetworkManager.Instance.UserIndex.Count; index++)
        {
            if (NetworkManager.Instance.MyIndex != index)
            {
                DataPacket packet = new DataPacket(msg, NetworkManager.Instance.UserIndex[index].EndPoint);
                sendMsgs.Enqueue(packet);
            }
        }
    }

    //몬스터 움직임(공격, 점프, 스킬 등등) -> Client
    public void MonsterStateSend(int state, int unitIndex)
    {
        UnitStateData unitStateData = new UnitStateData((byte)UnitType.Monster, state, (byte)unitIndex);
        UnitStatePacket unitStatePacket = new UnitStatePacket(unitStateData);
        unitStatePacket.SetPacketId((int)P2PPacketId.UnitState);

        byte[] msg = CreatePacket(unitStatePacket);

        for (int index = 0; index < NetworkManager.Instance.UserIndex.Count; index++)
        {
            if (index != NetworkManager.Instance.MyIndex)
            {
                DataPacket packet = new DataPacket(msg, NetworkManager.Instance.UserIndex[index].EndPoint);
                sendMsgs.Enqueue(packet);
            }
        }
    }


    //패킷의 헤더 생성
    byte[] CreateHeader<T>(Packet<T> data)
    {
        byte[] msg = data.GetPacketData();

        HeaderData headerData = new HeaderData();
        HeaderSerializer headerSerializer = new HeaderSerializer();

        headerData.length = (short)msg.Length;
        headerData.source = (byte)NetworkManager.Source.ClientSource;
        headerData.id = (byte)data.GetPacketId();

        headerSerializer.Serialize(headerData);
        byte[] header = headerSerializer.GetSerializedData();

        return header;
    }

    //패킷 생성
    byte[] CreatePacket<T>(Packet<T> data)
    {
        byte[] msg = data.GetPacketData();
        byte[] header = CreateHeader(data);
        byte[] packet = CombineByte(header, msg);

        return packet;
    }

    //패킷의 헤더 생성
    byte[] CreateUdpHeader<T>(Packet<T> data, int udpId)
    {
        byte[] msg = data.GetPacketData();

        HeaderData headerData = new HeaderData();
        HeaderSerializer headerSerializer = new HeaderSerializer();

        headerData.length = (short)msg.Length;
        headerData.source = (byte)NetworkManager.Source.ClientSource;
        headerData.id = (byte)data.GetPacketId();
        headerData.udpId = udpId;

        headerSerializer.UdpSerialize(headerData);
        byte[] header = headerSerializer.GetSerializedData();

        return header;
    }

    //패킷 생성
    byte[] CreateUdpPacket<T>(Packet<T> data, int udpId)
    {
        byte[] msg = data.GetPacketData();
        byte[] header = CreateUdpHeader(data, udpId);
        byte[] packet = CombineByte(header, msg);

        return packet;
    }

    public static byte[] CombineByte(byte[] array1, byte[] array2)
    {
        byte[] array3 = new byte[array1.Length + array2.Length];
        Array.Copy(array1, 0, array3, 0, array1.Length);
        Array.Copy(array2, 0, array3, array1.Length, array2.Length);
        return array3;
    }

    public static byte[] CombineByte(byte[] array1, byte[] array2, byte[] array3)
    {
        byte[] array4 = CombineByte(CombineByte(array1, array2), array3);
        return array4;
    }
}