
public class CharacterIndexPacket : Packet<CharacterIndexData>
{
    public class CharacterIndexSerializer : Serializer
    {
        public bool Serialize(CharacterIndexData data)
        {
            bool ret = true;
            ret &= Serialize(data.Index);
            return ret;
        }

        public bool Deserialize(ref CharacterIndexData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte index = 0;

            ret &= Deserialize(ref index);

            element = new CharacterIndexData(index);

            return ret;
        }
    }

    public CharacterIndexPacket(CharacterIndexData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public CharacterIndexPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new CharacterIndexData();
        CharacterIndexSerializer serializer = new CharacterIndexSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        CharacterIndexSerializer serializer = new CharacterIndexSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class CharacterIndexData
{
    byte index;

    public byte Index { get { return index; } }

    public CharacterIndexData()
    {
        index = 0;
    }

    public CharacterIndexData(byte newIndex)
    {
        index = newIndex;
    }
}