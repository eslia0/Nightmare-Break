public class UnitStatePacket : Packet<UnitStateData>
{
    public class UnitStateSerializer : Serializer
    {
        public bool Serialize(UnitStateData data)
        {
            bool ret = true;
            ret &= Serialize(data.UnitType);
            ret &= Serialize(data.State);
            ret &= Serialize(data.UnitIndex);
            return ret;
        }

        public bool Deserialize(ref UnitStateData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte unitType = 0;
            byte state = 0;
            byte unitIndex = 0;

            ret &= Deserialize(ref unitType);
            ret &= Deserialize(ref state);
            ret &= Deserialize(ref unitIndex);
            element = new UnitStateData(unitType, state, unitIndex);

            return ret;
        }
    }
    
    public UnitStatePacket(UnitStateData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public UnitStatePacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new UnitStateData();
        UnitStateSerializer serializer = new UnitStateSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        UnitStateSerializer serializer = new UnitStateSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class UnitStateData
{
    byte unitType;
    byte state;
    byte unitIndex;

    public byte UnitType { get { return unitType; } }
    public byte State { get { return state; } }
    public byte UnitIndex { get { return unitIndex; } }

    public UnitStateData()
    {
        unitType = 0;
        state = 0;
        unitIndex = 0;
    }

    public UnitStateData(int newUnitType, int newState, int newUnitIndex)
    {
        unitType = (byte)newUnitType;
        state = (byte)newState;
        unitIndex = (byte)newUnitIndex;
    }
}