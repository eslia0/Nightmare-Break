public class RequestDungeonDataPacket : Packet<RequestDungeonData>
{
    public class RequestDungeonDataSerializer : Serializer
    {
        public bool Serialize(RequestDungeonData data)
        {
            bool ret = true;
            ret &= Serialize(data.DungeonId);
            ret &= Serialize(data.DungeonLevel);
            
            return ret;
        }

        public bool Deserialize(ref RequestDungeonData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte dungeonId = 0;
            byte dungeonLevel = 0;

            ret &= Deserialize(ref dungeonId);
            ret &= Deserialize(ref dungeonLevel);
            element = new RequestDungeonData(dungeonId, dungeonLevel);

            return ret;
        }
    }

    public RequestDungeonDataPacket(RequestDungeonData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public RequestDungeonDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new RequestDungeonData();
        RequestDungeonDataSerializer serializer = new RequestDungeonDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        RequestDungeonDataSerializer serializer = new RequestDungeonDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class RequestDungeonData
{
    byte dungeonId;
    byte dungeonLevel;

    public byte DungeonId { get { return dungeonId; } }
    public byte DungeonLevel { get { return dungeonLevel; } }

    public RequestDungeonData()
    {
        dungeonId = 0;
        dungeonLevel = 0;
    }

    public RequestDungeonData(byte newDungeonId, byte newDungeonLevel)
    {
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
    }
}