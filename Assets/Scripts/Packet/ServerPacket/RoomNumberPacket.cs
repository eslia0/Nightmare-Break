
public class RoomNumberPacket : Packet<RoomNumberData>
{
    public class RoomNumberSerializer : Serializer
    {
        public bool Serialize(RoomNumberData data)
        {
            bool ret = true;
            ret &= Serialize(data.RoomNum);

            return ret;
        }

        public bool Deserialize(ref RoomNumberData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte roomNum = 0;

            ret &= Deserialize(ref roomNum);

            element = new RoomNumberData(roomNum);

            return ret;
        }
    }

    public RoomNumberPacket(RoomNumberData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public RoomNumberPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new RoomNumberData();
        RoomNumberSerializer serializer = new RoomNumberSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        RoomNumberSerializer serializer = new RoomNumberSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class RoomNumberData
{
    byte roomNum;

    public byte RoomNum { get { return roomNum; } }

    public RoomNumberData()
    {
        roomNum = 0;
    }

    public RoomNumberData(int newRoomNum)
    {
        roomNum = (byte)newRoomNum;
    }
}