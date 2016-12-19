using System.Text;

public class RoomListPacket : Packet<RoomListData>
{
    public class RoomListSerializer : Serializer
    {
        public bool Serialize(RoomListData data)
        {
            bool ret = true;

            for (int roomIndex = 0; roomIndex < WaitingUIManager.maxRoomNum; roomIndex++)
            {
                ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.Rooms[roomIndex].RoomName).Length);
                ret &= Serialize(data.Rooms[roomIndex].RoomName);
                ret &= Serialize((byte)data.Rooms[roomIndex].DungeonId);
                ret &= Serialize((byte)data.Rooms[roomIndex].DungeonLevel);
                ret &= Serialize((byte)data.Rooms[roomIndex].PlayerNum);
                ret &= Serialize((byte)data.Rooms[roomIndex].State);

                for (int userIndex = 0; userIndex < WaitingUIManager.maxPlayerNum; userIndex++)
                {
                    ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.Rooms[roomIndex].RoomUserData[userIndex].UserName).Length);
                    ret &= Serialize(data.Rooms[roomIndex].RoomUserData[userIndex].UserName);
                    ret &= Serialize((byte)data.Rooms[roomIndex].RoomUserData[userIndex].UserGender);
                    ret &= Serialize((byte)data.Rooms[roomIndex].RoomUserData[userIndex].UserClass);
                    ret &= Serialize((byte)data.Rooms[roomIndex].RoomUserData[userIndex].UserLevel);
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
            byte state = 0;
            byte userNameLength = 0;
            string userName;
            byte userGender = 0;
            byte userClass = 0;
            byte userLevel = 0;

            Room[] rooms;
            RoomUserData[] roomUserData;

            rooms = new Room[WaitingUIManager.maxRoomNum];
            roomUserData = new RoomUserData[WaitingUIManager.maxPlayerNum];

            for (int roomIndex = 0; roomIndex < WaitingUIManager.maxRoomNum; roomIndex++)
            {
                ret &= Deserialize(ref roomNameLength);
                ret &= Deserialize(out roomName, roomNameLength);
                ret &= Deserialize(ref dungeonId);
                ret &= Deserialize(ref dungeonLevel);
                ret &= Deserialize(ref playerNum);
                ret &= Deserialize(ref state);

                for (int userIndex = 0; userIndex < WaitingUIManager.maxPlayerNum; userIndex++)
                {
                    ret &= Deserialize(ref userNameLength);
                    ret &= Deserialize(out userName, userNameLength);
                    ret &= Deserialize(ref userGender);
                    ret &= Deserialize(ref userClass);
                    ret &= Deserialize(ref userLevel);

                    roomUserData[userIndex] = new RoomUserData(userName, userGender, userClass, userLevel);
                }

                rooms[roomIndex] = new Room(roomName, dungeonId, dungeonLevel, roomUserData, playerNum, state);
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