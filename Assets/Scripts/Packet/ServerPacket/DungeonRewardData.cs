public class DungeonRewardPacket : Packet<DungeonRewardData>
{
    public class DungeonRewardDataSerializer : Serializer
    {
        public bool Serialize(DungeonRewardData data)
        {
            bool ret = true;
            ret &= Serialize(data.DreamStone);
            ret &= Serialize(data.Exp);

            return ret;
        }

        public bool Deserialize(ref DungeonRewardData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte dreamStone = 0;
            int exp = 0;

            ret &= Deserialize(ref dreamStone);
            ret &= Deserialize(ref exp);

            element = new DungeonRewardData(dreamStone, exp);

            return ret;
        }
    }

    public DungeonRewardPacket(DungeonRewardData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public DungeonRewardPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new DungeonRewardData();
        DungeonRewardDataSerializer serializer = new DungeonRewardDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        DungeonRewardDataSerializer serializer = new DungeonRewardDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class DungeonRewardData
{
    byte dreamStone;
    int exp;

    public byte DreamStone { get { return dreamStone; } }
    public int Exp { get { return exp; } }

    public DungeonRewardData()
    {
        dreamStone = 0;
        exp = 0;
    }

    public DungeonRewardData(byte newDreamStone, int newExp)
    {
        dreamStone = newDreamStone;
        exp = newExp;
    }
}