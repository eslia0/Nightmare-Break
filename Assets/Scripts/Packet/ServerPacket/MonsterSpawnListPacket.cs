using System.Collections.Generic;

public class MonsterSpawnListPacket : Packet<DungeonData>
{
    public class MonsterSpawnListSerializer : Serializer
    {
        public bool Serialize(DungeonData data)
        {
            bool ret = true;

            //총 스테이지 개수
            ret &= Serialize((byte)data.Stages.Count);

            for (int stageIndex = 0; stageIndex < data.Stages.Count; stageIndex++)
            {
                //이 스테이지의 몬스터 개수
                ret &= Serialize((byte)data.Stages[stageIndex].MonsterSpawnData.Count);

                for (int monsterIndex = 0; monsterIndex < data.Stages[stageIndex].MonsterSpawnData.Count; monsterIndex++)
                {
                    ret &= Serialize(data.Stages[stageIndex].MonsterSpawnData[monsterIndex].MonsterId);
                    ret &= Serialize(data.Stages[stageIndex].MonsterSpawnData[monsterIndex].MonsterLevel);
                    ret &= Serialize(data.Stages[stageIndex].MonsterSpawnData[monsterIndex].MonsterNum);
                }
            }

            return ret;
        }

        public bool Deserialize(ref DungeonData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte stageNum = 0;
            byte monsterKind = 0;
            byte monsterId = 0;
            byte monsterLevel = 0;
            byte monsterNum = 0;

            ret &= Deserialize(ref stageNum);
            element = new DungeonData();

            for (int stageIndex = 0; stageIndex < stageNum; stageIndex++)
            {
                ret &= Deserialize(ref monsterKind);

                for (int monsterIndex = 0; monsterIndex < monsterKind; monsterIndex++)
                {
                    ret &= Deserialize(ref monsterId);
                    ret &= Deserialize(ref monsterLevel);
                    ret &= Deserialize(ref monsterNum);
                }

                element.Stages.Add(new Stage(stageIndex));
                element.Stages[stageIndex].AddMonster(monsterId, monsterLevel, monsterNum);
            }

            return ret;
        }
    }

    public MonsterSpawnListPacket(DungeonData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public MonsterSpawnListPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new DungeonData();
        MonsterSpawnListSerializer serializer = new MonsterSpawnListSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public override byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        MonsterSpawnListSerializer serializer = new MonsterSpawnListSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }
}

public class DungeonData
{
    int id;
    string name;
    int level;
    List<Stage> stages;

    public int Id { get { return id; } }
    public string Name { get { return name; } }
    public int Level { get { return level; } }
    public List<Stage> Stages { get { return stages; } }

    public DungeonData()
    {
        id = 0;
        name = "";
        level = 0;
        stages = new List<Stage>();
    }

    public DungeonData(int _id, int _level, string _name)
    {
        id = _id;
        name = _name;
        level = _level;
        stages = new List<Stage>();
    }

    public Stage GetStageData(int stageNum)
    {
        for (int i = 0; i < stages.Count; i++)
        {
            if(stages[i].StageNum == stageNum)
            {
                return stages[i];
            }
        }

        return null;
    }
    public int GetMonsterNum()
    {
        int count = 0;

        for (int i = 0; i < stages.Count; i++)
        {
            count += stages[i].MonsterSpawnData.Count;
        }

        return count;
    }
}

public class Stage
{
    int stageNum;
    List<MonsterSpawnData> monsterSpawnData;

    public int StageNum { get { return stageNum; } }
    public List<MonsterSpawnData> MonsterSpawnData { get { return monsterSpawnData; } }

    public Stage()
    {
        stageNum = 0;
        monsterSpawnData = new List<MonsterSpawnData>();
    }

    public Stage(int newStageNum)
    {
        stageNum = newStageNum;
        monsterSpawnData = new List<MonsterSpawnData>();
    }

    public void AddMonster(int monsterId, int monsterLevel, int monsterNum)
    {
        try
        {
            monsterSpawnData.Add(new MonsterSpawnData((byte)monsterId, (byte)monsterLevel, (byte)monsterNum));
        }
        catch
        {

        }
    }

    public int GetMonsterNum()
    {
        int count = 0;

        for (int i = 0; i < monsterSpawnData.Count; i++)
        {
            count += monsterSpawnData[i].MonsterNum;
        }

        return count;
    }
}

public class MonsterSpawnData
{
    byte monsterId;
    byte monsterLevel;
    byte monsterNum;

    public byte MonsterId { get { return monsterId; } }
    public byte MonsterLevel { get { return monsterLevel; } }
    public byte MonsterNum { get { return monsterNum; } }

    public MonsterSpawnData()
    {
        monsterId = 0;
        monsterLevel = 0;
        monsterNum = 0;
    }

    public MonsterSpawnData(byte newMonsterId, byte newMonsterLevel, byte newMonsterNum)
    {
        monsterId = newMonsterId;
        monsterLevel = newMonsterLevel;
        monsterNum = newMonsterNum;
    }
}