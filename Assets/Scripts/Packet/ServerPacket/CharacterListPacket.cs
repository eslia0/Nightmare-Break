using System.Text;

public class CharacterListPacket : Packet<CharacterList>
{
    public class CharacterListSerializer : Serializer
    {
        public bool Serialize(CharacterList data)
        {
            bool ret = true;

            for (int i =0; i< data.CharacterData.Length; i++)
            {
                ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.CharacterData[i].Name).Length);
                ret &= Serialize(data.CharacterData[i].Name);
                ret &= Serialize(data.CharacterData[i].Gender);
                ret &= Serialize(data.CharacterData[i].HClass);
                ret &= Serialize(data.CharacterData[i].Level);
            }

            return ret;
        }

        public bool Deserialize(ref CharacterList element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte nameLength = 0;
            string name;
            byte gender = 0;
            byte hClass = 0;
            byte level = 0;

            for (int i = 0; i < element.CharacterData.Length; i++)
            {
                ret &= Deserialize(ref nameLength);
                ret &= Deserialize(out name, nameLength);
                ret &= Deserialize(ref gender);
                ret &= Deserialize(ref hClass);
                ret &= Deserialize(ref level);

                element.CharacterData[i] = new CharacterData(name, gender, hClass, level);
            }

            return ret;
        }
    }

    public CharacterListPacket(CharacterList data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public CharacterListPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new CharacterList();
        CharacterListSerializer serializer = new CharacterListSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        CharacterListSerializer serializer = new CharacterListSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class CharacterList
{
    CharacterData[] characterData;

    public CharacterData[] CharacterData { get { return characterData; } }

    public CharacterList()
    {
        characterData = new CharacterData[WaitingUIManager.maxCharacterNum];

        for (int i = 0; i < characterData.Length; i++)
        {
            characterData[i] = new CharacterData();
        }
    }
}

public class CharacterData
{
    string name;
    byte gender;
    byte hClass;
    byte level;

    public string Name { get { return name; } }
    public byte Gender { get { return gender; } }
    public byte HClass { get { return hClass; } }
    public byte Level { get { return level; } }

    public CharacterData()
    {
        name = "Hero";
        gender = 0;
        hClass = 0;
        level = 0;
    }

    public CharacterData(string newName, byte newGender, byte newClass, byte newLevel)
    {
        name = newName;
        gender = newGender;
        hClass = newClass;
        level = newLevel;
    }
}