using System.Text;

public class RoomDataPacket : Packet<RoomData>
{
    public class RoomDataSerializer : Serializer
    {
        public bool Serialize(RoomData data)
        {
            bool ret = true;

            ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.RoomName).Length);
            ret &= Serialize(data.RoomName);
            ret &= Serialize(data.RoomNum);
            ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.DungeonName).Length);
            ret &= Serialize(data.DungeonName);
            ret &= Serialize(data.DungeonId);
            ret &= Serialize(data.DungeonLevel);

            for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
            {
                ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.RoomUserData[i].UserName).Length);
                ret &= Serialize(data.RoomUserData[i].UserName);
                ret &= Serialize((byte)data.RoomUserData[i].UserGender);
                ret &= Serialize((byte)data.RoomUserData[i].UserClass);
                ret &= Serialize((byte)data.RoomUserData[i].UserLevel);
            }

            return ret;
        }

        public bool Deserialize(ref RoomData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte roomNameLength = 0;
            string roomName;
            byte roomNum = 0;
            byte dungeonNameLength = 0;
            string dungeonName;
            byte dungeonId = 0;
            byte dungeonLevel = 0;

            byte[] userNameLength = new byte[WaitingUIManager.maxPlayerNum];
            string[] userName = new string[WaitingUIManager.maxPlayerNum];
            byte[] userGender = new byte[WaitingUIManager.maxPlayerNum];
            byte[] userClass = new byte[WaitingUIManager.maxPlayerNum];
            byte[] userLevel = new byte[WaitingUIManager.maxPlayerNum];
            RoomUserData[] roomUserData = new RoomUserData[WaitingUIManager.maxPlayerNum];

            ret &= Deserialize(ref roomNameLength);
            ret &= Deserialize(out roomName, roomNameLength);
            ret &= Deserialize(ref roomNum);
            ret &= Deserialize(ref dungeonNameLength);
            ret &= Deserialize(out dungeonName, dungeonNameLength);
            ret &= Deserialize(ref dungeonId);
            ret &= Deserialize(ref dungeonLevel);

            for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
            {
                ret &= Deserialize(ref userNameLength[i]);
                ret &= Deserialize(out userName[i], userNameLength[i]);
                ret &= Deserialize(ref userGender[i]);
                ret &= Deserialize(ref userClass[i]);
                ret &= Deserialize(ref userLevel[i]);
                roomUserData[i] = new RoomUserData(userName[i], userGender[i], userClass[i], userLevel[i]);
            }

            element = new RoomData(roomName, roomNum, dungeonName, dungeonId, dungeonLevel, roomUserData);

            return ret;
        }
    }

    public RoomDataPacket(RoomData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public RoomDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new RoomData();
        RoomDataSerializer serializer = new RoomDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        RoomDataSerializer serializer = new RoomDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class RoomData
{
    string roomName;
    byte roomNum;
    string dungeonName;
    byte dungeonId;
    byte dungeonLevel;
    RoomUserData[] roomUserData;

    public string RoomName { get { return roomName; } }
    public byte RoomNum { get { return roomNum; } }
    public string DungeonName { get { return dungeonName; } }
    public byte DungeonId { get { return dungeonId; } }
    public byte DungeonLevel { get { return dungeonLevel; } }
    public RoomUserData[] RoomUserData { get { return roomUserData; } }

    public RoomData()
    {
        roomName = "";
        roomNum = 0;
        dungeonName = "";
        dungeonId = 0;
        dungeonLevel = 0;
        roomUserData = new RoomUserData[WaitingUIManager.maxPlayerNum];

        for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
        {
            roomUserData[i] = new RoomUserData();
        }
    }

    public RoomData(string newRoomName, byte newRoomNum, string newDungeonName, byte newDungeonId, byte newDungeonLevel, RoomUserData[] newRoomUserData)
    {
        roomName = newRoomName;
        roomNum = newRoomNum;
        dungeonName = newDungeonName;
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
        roomUserData = newRoomUserData;
    }

    public RoomData(Room room, int newRoomNum)
    {
        roomName = room.RoomName;
        roomNum = (byte)newRoomNum;
        dungeonName = room.DungeonName;
        dungeonId = (byte)room.DungeonId;
        dungeonLevel = (byte)room.DungeonLevel;

        roomUserData = room.RoomUserData;
    }
}