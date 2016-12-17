using System.Text;

public class CharacterStatusPacket : Packet<CharacterStatusData>
{
    public class CharacterStatusSerializer : Serializer
    {
        public bool Serialize(CharacterStatusData data)
        {
            bool ret = true;

            ret &= Serialize((byte)Encoding.Unicode.GetBytes(data.Name).Length);
            ret &= Serialize(data.Name);
            ret &= Serialize(data.Gender);
            ret &= Serialize(data.Level);
            ret &= Serialize(data.HClass);
            ret &= Serialize(data.Exp);
            ret &= Serialize(data.HealthPoint);
            ret &= Serialize(data.MagicPoint);
            ret &= Serialize(data.HpRegeneration);
            ret &= Serialize(data.MpRegeneration);
            ret &= Serialize(data.Attack);
            ret &= Serialize(data.Defense);
            ret &= Serialize(data.SkillPoint);
            ret &= Serialize(data.DreamStone);

            for (int i = 0; i < CharacterStatus.skillNum; i++)
            {
                ret &= Serialize(data.SkillLevel[i]);
            }

            for (int i = 0; i < CharacterStatus.equipNum; i++)
            {
                ret &= Serialize(data.EquipLevel[i]);
            }

            return ret;
        }

        public bool Deserialize(ref CharacterStatusData element)
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
            byte level = 0;
            byte hClass = 0;
            byte exp = 0;
            byte healthPoint = 0;
            byte magicPoint = 0;
            byte hpRegeneration = 0;
            byte mpRegeneration = 0;
            byte attack = 0;
            byte defense = 0;
            byte skillPoint = 0;
            byte dreamStone = 0;
            byte[] skillLevel = new byte[CharacterStatus.skillNum];
            byte[] equipLevel = new byte[CharacterStatus.equipNum];

            ret &= Deserialize(ref nameLength);
            ret &= Deserialize(out name, nameLength);
            ret &= Deserialize(ref gender);
            ret &= Deserialize(ref level);
            ret &= Deserialize(ref hClass);
            ret &= Deserialize(ref exp);
            ret &= Deserialize(ref healthPoint);
            ret &= Deserialize(ref magicPoint);
            ret &= Deserialize(ref hpRegeneration);
            ret &= Deserialize(ref mpRegeneration);
            ret &= Deserialize(ref attack);
            ret &= Deserialize(ref defense);
            ret &= Deserialize(ref skillPoint);
            ret &= Deserialize(ref dreamStone);

            for (int i = 0; i < CharacterStatus.skillNum; i++)
            {
                ret &= Deserialize(ref skillLevel[i]);
            }

            for (int i = 0; i < CharacterStatus.equipNum; i++)
            {
                ret &= Deserialize(ref equipLevel[i]);
            }

            element = new CharacterStatusData(name, level, gender, hClass, exp, healthPoint, magicPoint, hpRegeneration,
                mpRegeneration, attack, defense, skillPoint, dreamStone, skillLevel, equipLevel);

            return ret;
        }
    }

    public CharacterStatusPacket(CharacterStatusData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public CharacterStatusPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new CharacterStatusData();
        CharacterStatusSerializer serializer = new CharacterStatusSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        CharacterStatusSerializer serializer = new CharacterStatusSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class CharacterStatusData
{
    byte nameLength;
    string name;
    byte gender;
    byte level;
    byte hClass;
    byte exp;
    byte healthPoint;
    byte magicPoint;
    byte hpRegeneration;
    byte mpRegeneration;
    byte attack;
    byte defense;
    byte skillPoint;
    byte dreamStone;
    byte[] skillLevel;
    byte[] equipLevel;

    public byte NameLength { get { return nameLength; } }
    public string Name { get { return name; } }
    public byte Gender { get { return gender; } }
    public byte Level { get { return level; } }
    public byte HClass { get { return hClass; } }
    public byte Exp { get { return exp; } }
    public byte HealthPoint { get { return healthPoint; } }
    public byte MagicPoint { get { return magicPoint; } }
    public byte HpRegeneration { get { return hpRegeneration; } }
    public byte MpRegeneration { get { return mpRegeneration; } }
    public byte Attack { get { return attack; } }
    public byte Defense { get { return defense; } }
    public byte SkillPoint { get { return skillPoint; } }
    public byte DreamStone { get { return dreamStone; } }
    public byte[] SkillLevel { get { return skillLevel; } }
    public byte[] EquipLevel { get { return equipLevel; } }

    public CharacterStatusData()
    {
        name = "Hero";
        level = 0;
        hClass = 0;
        exp = 0;
        healthPoint = 0;
        magicPoint = 0;
        hpRegeneration = 0;
        mpRegeneration = 0;
        attack = 0;
        defense = 0;
        skillPoint = 0;
        dreamStone = 0;
        skillLevel = new byte[CharacterStatus.skillNum];
        equipLevel = new byte[CharacterStatus.equipNum];
    }

    public CharacterStatusData(string newName, byte newLevel, byte newGender, byte newClass, byte newExp, byte newHealthPoint, byte newMagicPoint,
        byte newHpRegeneration, byte newMpRegeneration, byte newAttack, byte newDefense, byte newSkillPoint, byte newDreamStone, byte[] newSkillLevel, byte[] newEquipLevel)
    {
        name = newName;
        level = newLevel;
        gender = newGender;
        hClass = newClass;
        exp = newExp;
        healthPoint = newHealthPoint;
        magicPoint = newMagicPoint;
        hpRegeneration = newHpRegeneration;
        mpRegeneration = newMpRegeneration;
        attack = newAttack;
        defense = newDefense;
        skillPoint = newSkillPoint;
        dreamStone = newDreamStone;
        skillLevel = newSkillLevel;
        equipLevel = newEquipLevel;
    }
}