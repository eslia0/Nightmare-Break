public class UDPConnectionPacket : Packet<UDPConnectionData>
{
    public class UDPConnectionSerializer : Serializer
    {
        public bool Serialize(UDPConnectionData data)
        {
            bool ret = true;
            ret &= Serialize(data.playerNum);

            for (int i = 0; i < data.playerNum; i++)
            {
                ret &= Serialize(data.endPoint[i]);
                ret &= Serialize(',');
            }

            return ret;
        }

        public bool Deserialize(ref UDPConnectionData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte playerNum = 0;
            string total;

            ret &= Deserialize(ref playerNum);
            ret &= Deserialize(out total, (int)GetDataSize());

            string[] str = total.Split(',');

            if (str.Length < playerNum)
            {
                return false;
            }

            element.playerNum = playerNum;
            element.endPoint = new string[element.playerNum];

            for (int i = 0; i < playerNum; i++)
            {
                element.endPoint[i] = str[i];
            }

            return ret;
        }
    }

    public UDPConnectionPacket(UDPConnectionData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public UDPConnectionPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new UDPConnectionData();
        UDPConnectionSerializer serializer = new UDPConnectionSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        UDPConnectionSerializer serializer = new UDPConnectionSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class UDPConnectionData
{
    public byte playerNum;
    public string[] endPoint;

    public UDPConnectionData()
    {
        playerNum = 0;
        endPoint = new string[playerNum];
    }

    public UDPConnectionData(string[] newIp)
    {
        playerNum = (byte)newIp.Length;
        endPoint = newIp;
    }
}