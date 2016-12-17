public class SkillUpPacket : Packet<SkillUpData>
{
    public class SkillUpSerializer : Serializer
    {
        public bool Serialize(SkillUpData data)
        {
            bool ret = true;
            ret &= Serialize(data.SkillIndex);

            return ret;
        }

        public bool Deserialize(ref SkillUpData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte skillIndex = 0;

            ret &= Deserialize(ref skillIndex);

            element = new SkillUpData(skillIndex);

            return ret;
        }
    }

    public SkillUpPacket(SkillUpData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public SkillUpPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new SkillUpData();
        SkillUpSerializer serializer = new SkillUpSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        SkillUpSerializer serializer = new SkillUpSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class SkillUpData
{
    byte skillIndex;

    public byte SkillIndex { get { return skillIndex; } }

    public SkillUpData()
    {
        skillIndex = 0;
    }

    public SkillUpData(int newIndex)
    {
        skillIndex = (byte)newIndex;
    }
}