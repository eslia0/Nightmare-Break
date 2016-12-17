
public class ExitRoomPacket : Packet<ExitRoomData>
{
    public class ExitRoomSerializer : Serializer
    {
        public bool Serialize(ExitRoomData data)
        {
            bool ret = true;
            ret &= Serialize(data.RoomNum);

            return ret;
        }

        public bool Deserialize(ref ExitRoomData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte roomNum = 0;

            ret &= Deserialize(ref roomNum);

            element = new ExitRoomData(roomNum);

            return ret;
        }
    }
    public ExitRoomPacket(ExitRoomData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public ExitRoomPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new ExitRoomData();
        ExitRoomSerializer serializer = new ExitRoomSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        ExitRoomSerializer serializer = new ExitRoomSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class ExitRoomData
{
    byte roomNum;

    public byte RoomNum { get { return roomNum; } }

    public ExitRoomData()
    {
        roomNum = 0;
    }

    public ExitRoomData(int newRoomNum)
    {
        roomNum = (byte)newRoomNum;
    }
}