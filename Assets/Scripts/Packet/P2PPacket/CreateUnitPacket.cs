public class CreateUnitPacket : Packet<CreateUnitData>
{
    public class CreateUnitSerializer : Serializer
    {
        public bool Serialize(CreateUnitData data)
        {
            bool ret = true;

            ret &= Serialize(data.ID);
            ret &= Serialize(data.UnitIndex);
            ret &= Serialize(data.PosX);
            ret &= Serialize(data.PosY);
            ret &= Serialize(data.PosZ);
            return ret;
        }

        public bool Deserialize(ref CreateUnitData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            short id = 0;
            byte unitIndex = 0;
            float posX = 0;
            float posY = 0;
            float posZ = 0;

            ret &= Deserialize(ref id);
            ret &= Deserialize(ref unitIndex);
            ret &= Deserialize(ref posX);
            ret &= Deserialize(ref posY);
            ret &= Deserialize(ref posZ);
            element = new CreateUnitData(id, unitIndex, posX, posY, posZ);

            return ret;
        }
    }

    public CreateUnitPacket(CreateUnitData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public CreateUnitPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new CreateUnitData();
        CreateUnitSerializer serializer = new CreateUnitSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        CreateUnitSerializer serializer = new CreateUnitSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class CreateUnitData
{
    private short id;
    private byte unitIndex;
    private float posX;
    private float posY;
    private float posZ;

    public short ID { get { return id; } }
    public byte UnitIndex { get { return unitIndex; } }
    public float PosX { get { return posX; } }
    public float PosY { get { return posY; } }
    public float PosZ { get { return posZ; } }

    public CreateUnitData()
    {
        id = 0;
        unitIndex = 0;
        posX = 0;
        posY = 0;
        posZ = 0;
    }

    public CreateUnitData(short newId, byte newUnitIndex, float newX, float newY, float newZ)
    {
        id = newId;
        unitIndex = newUnitIndex;
        posX = newX;
        posY = newY;
        posZ = newZ;
    }
}