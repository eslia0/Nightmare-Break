using System;
using System.IO;

public class Serializer
{

    private MemoryStream m_buffer = null;

    private int m_offset = 0;

    public Serializer()
    {
        // 시리얼라이즈용 버퍼를 만든다.
        m_buffer = new MemoryStream();
    }

    public byte[] GetSerializedData()
    {
        return m_buffer.ToArray();
    }

    public void Clear()
    {
        byte[] buffer = m_buffer.GetBuffer();
        Array.Clear(buffer, 0, buffer.Length);

        m_buffer.Position = 0;
        m_buffer.SetLength(0);
        m_offset = 0;
    }

    //
    // 디시리얼라이즈할 데이터를 버퍼에 설정한다.
    //
    public bool SetDeserializedData(byte[] data)
    {
        // 설정할 버퍼를 클리어한다.
        Clear();

        try
        {
            // 디시리얼라이즈할 데이터를 설정한다.
            m_buffer.Write(data, 0, data.Length);
        }
        catch
        {
            return false;
        }

        return true;
    }


    //
    // bool형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(bool element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(bool));
    }

    //
    // char형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(char element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(char));
    }

    protected bool Serialize(byte element)
    {
        byte[] data = new byte[1];
        data[0] = element;
        return WriteBuffer(data, sizeof(byte));
    }

    //
    // float형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(float element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(float));
    }

    //
    // double형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(double element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(double));
    }

    //
    // short형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(short element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(short));
    }

    //
    // ushort형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(ushort element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(ushort));
    }

    //
    // int형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(int element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(int));
    }

    //
    // uint형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(uint element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(uint));
    }

    //
    // long형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(long element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(long));
    }

    //
    // ulong형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(ulong element)
    {
        byte[] data = BitConverter.GetBytes(element);

        return WriteBuffer(data, sizeof(ulong));
    }

    //
    // byte[]형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(byte[] element, int length)
    {
        return WriteBuffer(element, length);
    }

    //
    // string형 데이터를 시리얼라이즈한다.
    //
    protected bool Serialize(string element)
    {
        byte[] buffer = System.Text.Encoding.Unicode.GetBytes(element);
        return WriteBuffer(buffer, buffer.Length);
    }

    //
    // 데이터를 bool형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref bool element)
    {
        int size = sizeof(bool);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToBoolean(data, 0);
            return true;
        }

        return false;
    }

    //
    // 데이터를 char형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref char element)
    {
        int size = sizeof(char);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToChar(data, 0);
            return true;
        }

        return false;
    }

    //
    // 데이터를 byte형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref byte element)
    {
        int size = sizeof(byte);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = data[0];
            return true;
        }

        return false;
    }

    //
    // 데이터를 float형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref float element)
    {
        int size = sizeof(float);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToSingle(data, 0);
            return true;
        }

        return false;
    }

    //
    // 데이터를 double형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref double element)
    {
        int size = sizeof(double);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToDouble(data, 0);
            return true;
        }

        return false;
    }

    //
    // 데이터를 short형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref short element)
    {
        int size = sizeof(short);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToInt16(data, 0);
            return true;
        }

        return false;
    }

    //
    // 데이터를 ushort형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref ushort element)
    {
        int size = sizeof(ushort);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToUInt16(data, 0);
            return true;
        }

        return false;
    }

    //
    // 데이터를 int형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref int element)
    {
        int size = sizeof(int);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToInt32(data, 0);
            return true;
        }

        return false;
    }

    //
    // 데이터를 uint형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref uint element)
    {
        int size = sizeof(uint);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToUInt32(data, 0);
            return true;
        }

        return false;
    }

    //
    // 데이터를 long형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref long element)
    {
        int size = sizeof(long);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToInt64(data, 0);
            return true;
        }

        return false;
    }

    //
    // 데이터를 ulong형으로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref ulong element)
    {
        int size = sizeof(ulong);
        byte[] data = new byte[size];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            // 읽어낸 값을 변환한다.
            element = BitConverter.ToUInt64(data, 0);
            return true;
        }

        return false;
    }

    //
    // byte[]형 데이터로 디시리얼라이즈한다.
    //
    protected bool Deserialize(ref byte[] element, int length)
    {

        // 
        bool ret = ReadBuffer(ref element, length);


        if (ret == true)
        {
            return true;
        }

        return false;
    }

    //
    // string 데이터로 디시리얼라이즈한다.
    //
    protected bool Deserialize(out string element, int length)
    {
        byte[] data = new byte[length];

        // 
        bool ret = ReadBuffer(ref data, data.Length);
        if (ret == true)
        {
            element = System.Text.Encoding.Unicode.GetString(data);

            return true;
        }
        element = null;
        return false;
    }

    protected bool ReadBuffer(ref byte[] data, int size)
    {
        // 현재 오프셋에서 데이터를 읽어낸다.
        try
        {
            m_buffer.Position = m_offset;
            m_buffer.Read(data, 0, size);
            m_offset += size;
        }
        catch
        {
            return false;
        }

        return true;
    }

    protected bool WriteBuffer(byte[] data, int size)
    {
        // 현재 오프셋에서 데이터를 써넣는다.
        try
        {
            m_buffer.Position = m_offset;
            m_buffer.Write(data, 0, size);
            m_offset += size;
        }
        catch
        {
            return false;
        }

        return true;
    }

    public long GetDataSize()
    {
        return m_buffer.Length;
    }
}

