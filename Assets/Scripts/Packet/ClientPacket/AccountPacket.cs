public class AccountPacket : Packet<AccountData>
{
    public class AccountSerializer : Serializer
    {
        public bool Serialize(AccountData data)
        {
            bool ret = true;
            ret &= Serialize(data.Id);
            ret &= Serialize(".");
            ret &= Serialize(data.Password);
            return ret;
        }

        public bool Deserialize(ref AccountData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            string total;
            ret &= Deserialize(out total, (int)GetDataSize());

            string[] str = total.Split('.');
            if (str.Length < 2)
            {
                return false;
            }

            element = new AccountData(str[0], str[1]);

            return ret;
        }
    }
    
    public AccountPacket(AccountData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public AccountPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new AccountData();
        AccountSerializer serializer = new AccountSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        AccountSerializer serializer = new AccountSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class AccountData
{
    string id;
    string password;

    public string Id { get { return id; } }
    public string Password { get { return password; } }

    public AccountData()
    {
        id = "";
        password = "";
    }

    public AccountData(string newId, string newPassword)
    {
        id = newId;
        password = newPassword;
    }
}