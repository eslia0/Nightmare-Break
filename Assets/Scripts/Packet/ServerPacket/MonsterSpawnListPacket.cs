using System.Collections.Generic;

public class MonsterSpawnListPacket : Packet<DungeonLevelData>
{
    public class MonsterSpawnListSerializer : Serializer
    {
        public bool Serialize(DungeonLevelData data)
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

        public bool Deserialize(ref DungeonLevelData element)
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
            UnityEngine.Debug.Log("총 스테이지 개수" + stageNum);

            for (int stageIndex = 0; stageIndex < stageNum; stageIndex++)
            {
                ret &= Deserialize(ref monsterKind);
                UnityEngine.Debug.Log(stageIndex + " 스테이지 몬스터 종류수" + monsterKind);

                element.AddStage(new Stage(stageIndex));
                
                for (int monsterIndex = 0; monsterIndex < monsterKind; monsterIndex++)
                {
                    ret &= Deserialize(ref monsterId);
                    ret &= Deserialize(ref monsterLevel);
                    ret &= Deserialize(ref monsterNum);

                    UnityEngine.Debug.Log(monsterIndex + " 번 몬스터 아이디" + monsterId);
                    UnityEngine.Debug.Log(monsterIndex + " 번 몬스터 레벨" + monsterLevel);
                    UnityEngine.Debug.Log(monsterIndex + " 번 몬스터 갯수" + monsterNum);

                    element.Stages[stageIndex].AddMonster(monsterId, monsterLevel, monsterNum);
                }
            }

            return ret;
        }
    }

    public MonsterSpawnListPacket(DungeonLevelData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public MonsterSpawnListPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new DungeonLevelData();
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
public class DungeonBaseData
{
    int id;
    string name;
    List<DungeonLevelData> dungeonLevelData;

    public int Id { get { return id; } }
    public string Name { get { return name; } }
    public List<DungeonLevelData> DungeonLevelData { get { return dungeonLevelData; } }

    public DungeonBaseData()
    {
        id = 0;
        name = "";
        dungeonLevelData = new List<DungeonLevelData>();
    }

    public DungeonBaseData(int _id, string _name)
    {
        id = _id;
        name = _name;
        dungeonLevelData = new List<DungeonLevelData>();
    }

    public DungeonLevelData GetLevelData(int level)
    {
        for (int index = 0; index < dungeonLevelData.Count; index++)
        {
            if (dungeonLevelData[index].Level == level)
            {
                return dungeonLevelData[index];
            }
        }

        return null;
    }

    public bool AddLevelData(DungeonLevelData newDungeonLevelData)
    {
        try
        {
            dungeonLevelData.Add(newDungeonLevelData);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RemoveLevelData(int index)
    {
        try
        {
            dungeonLevelData.Remove(GetLevelData(index));
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class DungeonLevelData
{
    int level;
    List<Stage> stages;

    public int Level { get { return level; } }
    public List<Stage> Stages { get { return stages; } }

    public DungeonLevelData()
    {
        level = 0;
        stages = new List<Stage>();
    }

    public DungeonLevelData(int newLevel)
    {
        level = newLevel;
        stages = new List<Stage>();
    }

    public Stage GetStage(int index)
    {
        for (int i = 0; i < stages.Count; i++)
        {
            if (stages[i].StageNum == index)
            {
                return stages[i];
            }
        }

        return null;
    }

    public bool AddStage(Stage newStage)
    {
        try
        {
            stages.Add(newStage);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RemoveLevelData(int index)
    {
        try
        {
            stages.Remove(GetStage(index));
            return true;
        }
        catch
        {
            return false;
        }
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

    public bool RemoveMonster(int index)
    {
        try
        {
            monsterSpawnData.Remove(monsterSpawnData[index]);
            return true;
        }
        catch
        {
            return false;
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