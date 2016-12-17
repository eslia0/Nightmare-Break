public class ResultPacket : Packet<ResultData>
{
    public class ResultSerializer : Serializer
    {
        public bool Serialize(ResultData data)
        {
            bool ret = true;
            ret &= Serialize(data.Result);
            return ret;
        }

        public bool Deserialize(ref ResultData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte result = 0;

            ret &= Deserialize(ref result);
            element = new ResultData(result);

            return ret;
        }
    }
    public ResultPacket(ResultData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public ResultPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new ResultData();
        ResultSerializer serializer = new ResultSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        ResultSerializer serializer = new ResultSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class ResultData
{
    byte result;

    public byte Result { get { return result; } }

    public ResultData()
    {
        result = 0;
    }

    public ResultData(byte newResult)
    {
        result = newResult;
    }
}