public class EquipUpgradePacket : Packet<EquipUpgradeData>
{
    public class EquipUpgradeSerializer : Serializer
    {
        public bool Serialize(EquipUpgradeData data)
        {
            bool ret = true;
            ret &= Serialize(data.EquipIndex);

            return ret;
        }

        public bool Deserialize(ref EquipUpgradeData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte skillIndex = 0;

            ret &= Deserialize(ref skillIndex);

            element = new EquipUpgradeData(skillIndex);

            return ret;
        }
    }

    public EquipUpgradePacket(EquipUpgradeData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public EquipUpgradePacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new EquipUpgradeData();
        EquipUpgradeSerializer serializer = new EquipUpgradeSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        EquipUpgradeSerializer serializer = new EquipUpgradeSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class EquipUpgradeData
{
    byte equipIndex;

    public byte EquipIndex { get { return equipIndex; } }

    public EquipUpgradeData()
    {
        equipIndex = 0;
    }

    public EquipUpgradeData(int newIndex)
    {
        equipIndex = (byte)newIndex;
    }
}