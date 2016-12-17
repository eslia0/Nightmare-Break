public class HeaderSerializer : Serializer
{
    public bool Serialize(HeaderData data)
    {
        // 기존 데이터를 클리어한다.
        Clear();

        // 각 요소를 차례로 시리얼라이즈한다.
        bool ret = true;
        ret &= Serialize(data.length);
        ret &= Serialize(data.source);
        ret &= Serialize(data.id);

        if (ret == false)
        {
            return false;
        }

        return true;
    }

    public bool UdpSerialize(HeaderData data)
    {
        // 기존 데이터를 클리어한다.
        Clear();

        // 각 요소를 차례로 시리얼라이즈한다.
        bool ret = true;
        ret &= Serialize(data.length);
        ret &= Serialize(data.source);
        ret &= Serialize(data.id);
        ret &= Serialize(data.udpId);

        if (ret == false)
        {
            return false;
        }

        return true;
    }

    public bool Deserialize(ref HeaderData element)
    {
        // 디시리얼라이즈하는 데이터를 설정한다.
        bool ret = (GetDataSize() > 0) ? true : false;

        if (ret == false)
        {
            return false;
        }

        // 데이터의 요소별로 디시리얼라이즈한다.
        byte source = 0;
        byte id = 0;
        ret &= Deserialize(ref source);
        ret &= Deserialize(ref id);
        element.source = source;
        element.id = id;

        return ret;
    }

    public bool UdpDeserialize(ref HeaderData element)
    {
        // 디시리얼라이즈하는 데이터를 설정한다.
        bool ret = (GetDataSize() > 0) ? true : false;

        if (ret == false)
        {
            return false;
        }

        // 데이터의 요소별로 디시리얼라이즈한다.
        byte source = 0;
        byte id = 0;
        int udpId = 0;
        ret &= Deserialize(ref source);
        ret &= Deserialize(ref id);
        ret &= Deserialize(ref udpId);
        element.source = source;
        element.id = id;
        element.udpId = udpId;

        return ret;
    }
}

public class HeaderData
{
    // 헤더 == [2바이트 - 패킷길이][1바이트 - 출처][1바이트 - ID]
    public short length; // 패킷의 길이
    public byte source; //패킷 출처
    public byte id; // 패킷 ID
    public int udpId; //Udp ID
}