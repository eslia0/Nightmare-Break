
public class CreateCharacterPacket : Packet<CreateCharacterData>
{
    public class CreateCharacterSerializer : Serializer
    {
        public bool Serialize(CreateCharacterData data)
        {
            bool ret = true;
            ret &= Serialize(data.Gender);
            ret &= Serialize(data.HClass);
            ret &= Serialize(data.HName);
            return ret;
        }

        public bool Deserialize(ref CreateCharacterData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte gender = 0;
            byte hClass = 0;
            string hName = "";

            ret &= Deserialize(ref gender);
            ret &= Deserialize(ref hClass);
            ret &= Deserialize(out hName, (int)GetDataSize());

            element = new CreateCharacterData(gender, hClass, hName);

            return ret;
        }
    }

    public CreateCharacterPacket(CreateCharacterData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public CreateCharacterPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new CreateCharacterData();
        CreateCharacterSerializer serializer = new CreateCharacterSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        CreateCharacterSerializer serializer = new CreateCharacterSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class CreateCharacterData
{
    byte gender;
    byte hClass;
    string hName;

    public byte Gender { get { return gender; } }
    public byte HClass { get { return hClass; } }
    public string HName { get { return hName; } }

    public CreateCharacterData()
    {
        gender = 0;
        hClass = 0;
        hName = "";
    }

    public CreateCharacterData(byte newGender, byte newClass, string newName)
    {
        gender = newGender;
        hClass = newClass;
        hName = newName;
    }
}