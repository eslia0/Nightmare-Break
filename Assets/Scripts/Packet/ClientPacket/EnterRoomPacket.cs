public class EnterRoomPacket : Packet<EnterRoomData>
{
    public class EnterRoomSerializer : Serializer
    {
        public bool Serialize(EnterRoomData data)
        {
            bool ret = true;
            ret &= Serialize(data.roomNum);

            return ret;
        }

        public bool Deserialize(ref EnterRoomData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte roomNum = 0;

            ret &= Deserialize(ref roomNum);

            element.roomNum = roomNum;

            return ret;
        }
    }
    
    public EnterRoomPacket(EnterRoomData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public EnterRoomPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new EnterRoomData();
        EnterRoomSerializer serializer = new EnterRoomSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        EnterRoomSerializer serializer = new EnterRoomSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class EnterRoomData
{
    public byte roomNum;

    public EnterRoomData()
    {
        roomNum = 0;
    }

    public EnterRoomData(int newRoomNum)
    {
        roomNum = (byte)newRoomNum;
    }
}