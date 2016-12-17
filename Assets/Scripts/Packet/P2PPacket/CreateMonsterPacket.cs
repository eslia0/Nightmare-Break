public class CreateMonsterPacket : Packet<CreateMonsterData>
{
    public class CreateMonsterSerializer : Serializer
    {
        public bool Serialize(CreateMonsterData data)
        {
            bool ret = true;

            ret &= Serialize(data.MonsterKind);

            for(int i =0; i< data.MonsterKind; i++)
            {
                ret &= Serialize(data.MonsterNum[i]);
            }            

            return ret;
        }

        public bool Deserialize(ref CreateMonsterData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte monsterKind = 0;
            byte[] monsterNum;

            ret &= Deserialize(ref monsterKind);
            monsterNum = new byte[monsterKind];

            for (int i =0; i < monsterKind; i++)
            {
                ret &= Deserialize(ref monsterNum[i]);
            }

            element = new CreateMonsterData(monsterKind, monsterNum);

            return ret;
        }
    }

    public CreateMonsterPacket(CreateMonsterData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public CreateMonsterPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new CreateMonsterData();
        CreateMonsterSerializer serializer = new CreateMonsterSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        CreateMonsterSerializer serializer = new CreateMonsterSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class CreateMonsterData
{
    private byte monsterKind;
    private byte[] monsterNum;

    public byte MonsterKind { get { return monsterKind; } }    
    public byte[] MonsterNum { get { return monsterNum; } }

    public CreateMonsterData()
    {
        monsterKind = 0;
        monsterNum = new byte[monsterKind];
    }

    public CreateMonsterData(byte newMosnterKind, byte[] newMonsterNum)
    {
        monsterKind = newMosnterKind;
        monsterNum = newMonsterNum;
    }
}