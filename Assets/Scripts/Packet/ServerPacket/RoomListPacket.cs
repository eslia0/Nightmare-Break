using System.Text;

public class RoomListPacket : Packet<RoomListData>
{
    public class RoomListSerializer : Serializer
    {
        public bool Serialize(RoomListData data)
        {
            bool ret = true;

            for (int i = 0; i < WaitingUIManager.maxRoomNum; i++)
            {
                ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.Rooms[i].RoomName).Length);
                ret &= Serialize(data.Rooms[i].RoomName);
                ret &= Serialize((byte)data.Rooms[i].DungeonId);
                ret &= Serialize((byte)data.Rooms[i].DungeonLevel);
                ret &= Serialize((byte)data.Rooms[i].PlayerNum);

                for (int j = 0; j < WaitingUIManager.maxPlayerNum; j++)
                {
                    ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.Rooms[i].RoomUserData[j].UserName).Length);
                    ret &= Serialize(data.Rooms[i].RoomUserData[j].UserName);
                    ret &= Serialize((byte)data.Rooms[i].RoomUserData[j].UserGender);
                    ret &= Serialize((byte)data.Rooms[i].RoomUserData[j].UserClass);
                    ret &= Serialize((byte)data.Rooms[i].RoomUserData[j].UserLevel);
                }
            }

            return ret;
        }

        public bool Deserialize(ref RoomListData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;

            byte roomNameLength = 0;
            string roomName;
            byte dungeonId = 0;
            byte dungeonLevel = 0;
            byte playerNum = 0;
            byte userNameLength = 0;
            string userName;
            byte userGender = 0;
            byte userClass = 0;
            byte userLevel = 0;

            RoomUserData[] roomUserData;
            Room[] rooms;

            roomUserData = new RoomUserData[WaitingUIManager.maxPlayerNum];
            rooms = new Room[WaitingUIManager.maxRoomNum];

            for (int i = 0; i < WaitingUIManager.maxRoomNum; i++)
            {
                ret &= Deserialize(ref roomNameLength);
                ret &= Deserialize(out roomName, roomNameLength);
                ret &= Deserialize(ref dungeonId);
                ret &= Deserialize(ref dungeonLevel);
                ret &= Deserialize(ref playerNum);

                for (int j = 0; j < WaitingUIManager.maxPlayerNum; j++)
                {
                    ret &= Deserialize(ref userNameLength);
                    ret &= Deserialize(out userName, userNameLength);
                    ret &= Deserialize(ref userGender);
                    ret &= Deserialize(ref userClass);
                    ret &= Deserialize(ref userLevel);

                    roomUserData[j] = new RoomUserData(userName, userGender, userClass, userLevel);
                }

                rooms[i] = new Room(roomName, dungeonId, dungeonLevel, roomUserData, playerNum);
            }

            element = new RoomListData(rooms);

            return ret;
        }
    }

    public RoomListPacket(RoomListData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public RoomListPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new RoomListData();
        RoomListSerializer serializer = new RoomListSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        RoomListSerializer serializer = new RoomListSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class RoomListData
{
    Room[] rooms;

    public Room[] Rooms { get { return rooms; } }

    public RoomListData()
    {
        rooms = new Room[WaitingUIManager.maxRoomNum];

        for (int i = 0; i < WaitingUIManager.maxRoomNum; i++)
        {
            rooms[i] = new Room();
        }
    }

    public RoomListData(Room[] newRooms)
    {
        rooms = newRooms;
    }
}