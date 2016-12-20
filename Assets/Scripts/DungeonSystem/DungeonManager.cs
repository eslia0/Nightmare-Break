using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Collections.Generic;
using System.Collections;

public enum UnitId
{
    None = 0,
    ManWarrior,
    WomanWarrior,
    ManMage,
    WomanMage,
    Frog,
    Duck,
    Rabbit,
    BlackBear,
    Bear,
}

//this class manage monsterStageLevel, sumon, player sumon, player death;
public class DungeonManager : MonoBehaviour
{
    private static DungeonManager instance = null;
    public static DungeonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindWithTag("DungeonManager").GetComponent<DungeonManager>();
            }

            return instance;
        }
    }

    GameObject[] playerSpawnPoints;
    GameObject[] monsterSpawnPoints;
    GameObject[] players;
    GameObject[] monsters;
    CharacterManager[] characterData;
    Monster[] monsterData;

    DungeonLevelData dungeonLevelData;
    MonsterStatusData monsterStatusData;

    StagePortal stagePortal;
    SceneChanger.SceneName[] sceneList;

    GameObject m_camera;

    [SerializeField]
    int stageNum;
    int dungeonId;
    int dungeonLevel;

    bool isNormal;
    bool dungeonEnd;

    public GameObject[] Players { get { return players; } }
    public CharacterManager[] CharacterData { get { return characterData; } }
    public SceneChanger.SceneName[] SceneList { get { return sceneList; } }

    public int DungeonId { get { return dungeonId; } }
    public int DungeonLevel { get { return dungeonLevel; } }
    public int StageNum { get { return stageNum; } }
    public bool IsNormal { get { return isNormal; } set { isNormal = value; } }
    public GameObject[] Monsters { get { return monsters; } }
    
    public void SetCurrentStageNum(int newStageNum)
    {
        stageNum = newStageNum;
    }

    //각종 매니저 초기화
    public void ManagerInitialize(int newDungeonId, int newDungeonLevel)
    {
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
    }

    public void InitializePlayer(int playerNum)
    {
        Debug.Log("유저 수 : " + playerNum);
        players = new GameObject[playerNum];
        characterData = new CharacterManager[playerNum];
    }

    public void StartDungeon()
    {
        InitializePlayerSpawnPoint();
        InitializeSceneName(dungeonId);

        CreatePlayer((int)GameManager.Instance.CharacterStatus.HGender, (int)GameManager.Instance.CharacterStatus.HClass);
                
        if (NetworkManager.Instance.IsHost)
        {
            InitializeMonsterSpawnPoint(stageNum);

            if (!isNormal)
            {
                StartCoroutine(DefenseWave());
            }
            else
            {
                if (stageNum < sceneList.Length - 1)
                {
                    InitializeStagePortal();
                }
                SpawnMonster(stageNum);
                SetMonsterStatus(stageNum);
            }

            if(stageNum + 1 >= sceneList.Length)
            {
                StartCoroutine(CheckDungeonEnd(monsters[0]));
            }
        }
    }

    public IEnumerator CheckDungeonEnd(GameObject bossMonster)
    {
        while (!dungeonEnd)
        {
            yield return null;

            if(bossMonster == null)
            {
                dungeonEnd = true;
                SceneChanger.Instance.SceneChange(SceneChanger.SceneName.WaitingScene, true);
            }
        }
    }

    public void InitializeSceneName(int dungeonId)
    {
        if (dungeonId == 0)
        {
            sceneList = new SceneChanger.SceneName[3];
            sceneList[0] = SceneChanger.SceneName.TeddyBearStage1;
            sceneList[1] = SceneChanger.SceneName.TeddyBearStage2;
            sceneList[2] = SceneChanger.SceneName.TeddyBearBoss;
        }
    }

    public void InitializePlayerSpawnPoint()
    {
        playerSpawnPoints = new GameObject[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            playerSpawnPoints[i] = GameObject.Find("PlayerSpawnPoint" + (i + 1));
        }
    }

    public void InitializeMonsterSpawnPoint(int stageIndex)
    {
        Stage stageData = dungeonLevelData.GetStage(stageIndex);

        monsterSpawnPoints = new GameObject[stageData.GetMonsterNum()];

        for (int i = 0; i < stageData.GetMonsterNum(); i++)
        {
            monsterSpawnPoints[i] = GameObject.Find("MonsterSpawnPoint" + (i + 1));
        }
    }

    public void InitializeStagePortal()
    {
        stagePortal = GameObject.Find("StagePortal").GetComponent<StagePortal>();
        stagePortal.InitializePortal();
        StartCoroutine(CheckMapClear());
    }

    public void SetMonsterSpawnList(DungeonLevelData newDungeonData)
    {
        dungeonLevelData = newDungeonData;
    }

    public void SpawnMonster(int stageIndex)
    {
        Stage stageData = dungeonLevelData.GetStage(stageIndex);

        monsters = new GameObject[stageData.GetMonsterNum()];
        monsterData = new Monster[stageData.GetMonsterNum()];

        int spawnCount = stageData.MonsterSpawnData.Count;
        int monsterIndex = 0;

        //몬스터 종류별로
        for (int spawnIndex = 0; spawnIndex < spawnCount; spawnIndex++)
        {
            int maxSpawnNum = stageData.MonsterSpawnData[spawnIndex].MonsterNum;

            //생성 횟수 만큼 생성
            for (int spawnNum = 0; spawnNum < maxSpawnNum; spawnNum++)
            {
                monsters[monsterIndex] = CreateMonster(stageData.MonsterSpawnData[spawnIndex].MonsterId, monsterIndex, monsterSpawnPoints[monsterIndex].transform);
                monsterIndex++;
            }
        }
    }

    public GameObject CreateMonster(int unitId, int unitIndex, Transform createPoint)
    {
        if (monsters[unitIndex] == null)
        {
            GameObject monster = null;

            if (unitId == (int)MonsterId.Frog)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/Frog"), createPoint.position, gameObject.transform.rotation);
            }
            else if (unitId == (int)MonsterId.Duck)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/Duck"), createPoint.position, gameObject.transform.rotation);
            }
            else if (unitId == (int)MonsterId.Rabbit)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/Rabbit"), createPoint.position, gameObject.transform.rotation);
            }
            else if (unitId == (int)MonsterId.BlackBear)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/BlackBear"), createPoint.position, gameObject.transform.rotation);
            }
            else if (unitId == (int)MonsterId.Bear)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/Bear"), createPoint.position, gameObject.transform.rotation);
            }

            if (monster == null)
            {
                return null;
            }

            monster.transform.SetParent(createPoint);
            monsterData[unitIndex] = monster.GetComponent<Monster>();
            monsterData[unitIndex].MonsterId = (MonsterId)unitId;
            monsterData[unitIndex].MonsterIndex = unitIndex;

            return monster;
        }
        else { return null; }
    }

    public IEnumerator DefenseWave()
    {
        for (int spawnCount = 0; spawnCount < dungeonLevelData.WaveCount; spawnCount++)
        {
            Debug.Log("소환 : "+stageNum);
            SpawnMonster(stageNum);
            SetMonsterStatus(stageNum);

            if (spawnCount == dungeonLevelData.WaveCount - 1)
                break;

            yield return new WaitForSeconds(20f);
        }

        InitializeStagePortal();
    }

    public void SetMonsterData(MonsterStatusData newMonsterStatusData)
    {
        monsterStatusData = newMonsterStatusData;
    }

    public void SetMonsterStatus(int stageIndex)
    {
        Stage stageData = dungeonLevelData.GetStage(stageIndex);

        int spawnCount = stageData.MonsterSpawnData.Count;
        int monsterIndex = 0;

        //몬스터 종류별로
        for (int spawnIndex = 0; spawnIndex < spawnCount; spawnIndex++)
        {
            int maxSpawnNum = stageData.MonsterSpawnData[spawnIndex].MonsterNum;

            //생성 횟수 만큼 설정
            for (int spawnNum = 0; spawnNum < maxSpawnNum; spawnNum++)
            {
                monsterData[monsterIndex].MonsterSet(monsterStatusData.MonsterData[spawnIndex]);
                monsterIndex++;
            }
        }
    }

    public IEnumerator CheckMapClear()
    {
        while (true)
        {
            yield return null;

            int count = 0;

            for (int i = 0; i < monsters.Length; i++)
            {
                if (monsters[i] == null)
                {
                    count++;
                }
            }

            if (count >= monsters.Length)
            {
                break;
            }
        }

        stagePortal.StagePortalActivate();
    }

    public GameObject CreatePlayer(int gender, int hClass)
    {
        //여기서는 플레이어 캐릭터 딕셔너리 -> 각 직업에 따른 플레이어 스탯과 능력치, 스킬, 이름을 가지고 있음
        //딕셔너리를 사용하여 그에 맞는 캐릭터를 소환해야 하지만 Prototype 진행 시에는 고정된 플레이어를 소환하도록 함.

        int characterId = (hClass * CharacterStatus.maxGender) + gender + 1;
        int unitIndex = NetworkManager.Instance.MyIndex;
        Debug.Log("내 캐릭터 생성 번호" + unitIndex);

        GameObject player = Instantiate(Resources.Load("Class" + characterId)) as GameObject;
        player.tag = "Player";
        player.transform.position = playerSpawnPoints[unitIndex].transform.position;
        players[unitIndex] = player;

        characterData[unitIndex] = player.GetComponent<CharacterManager>();
        characterData[unitIndex].enabled = true;
        characterData[unitIndex].SetUnitIndex(unitIndex);
        characterData[unitIndex].SetCharacterStatus(GameManager.Instance.CharacterStatus);
        characterData[unitIndex].InitializeCharacter();

        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        StartCoroutine(m_camera.GetComponent<CameraController>().CameraCtrl(player.transform));

        InputManager.Instance.InitializeManager(player);

        for (int index = 0; index < NetworkManager.Instance.UserIndex.Count; index++)
        {
            if (index != unitIndex)
            {
                DataSender.Instance.CreateUnitSend(NetworkManager.Instance.UserIndex[index].EndPoint, (byte)UnitType.Hero, (byte)characterId, (byte)unitIndex, player.transform.position.x, player.transform.position.y, player.transform.position.z);
            }
        }

        return player;
    }

    public void CreateUnit(CreateUnitData createUnitData)
    {
        if (createUnitData.UnitType == (byte)UnitType.Hero)
        {
            if (players[createUnitData.UnitIndex] == null)
            {
                GameObject unit = Instantiate(Resources.Load("Class" + createUnitData.ID)) as GameObject;
                unit.transform.position = new Vector3(createUnitData.PosX, createUnitData.PosY, createUnitData.PosZ);
                unit.tag = "Player";
                players[createUnitData.UnitIndex] = unit;

                int gender = createUnitData.ID % 2;
                int hClass = createUnitData.ID / 2;

                CharacterStatusData characterStatusData = new CharacterStatusData((byte)gender, (byte)hClass);

                characterData[createUnitData.UnitIndex] = unit.GetComponent<CharacterManager>();
                characterData[createUnitData.UnitIndex].SetCharacterStatus(unit.AddComponent<CharacterStatus>());
                characterData[createUnitData.UnitIndex].CharacterStatus.SetCharacterStatus(characterStatusData);
                characterData[createUnitData.UnitIndex].SetUnitIndex(createUnitData.UnitIndex);
                characterData[createUnitData.UnitIndex].InitializeCharacter();
            }
            else
            {
                Debug.Log("이미 있는 캐릭터 인덱스 : " + createUnitData.UnitIndex);
            }
        }
    }

    public void SetCharacterPosition(UnitPositionData unitPositionData)
    {
        characterData[unitPositionData.UnitIndex].SetPosition(unitPositionData);
    }

    public void SetMonsterPosition(UnitPositionData unitPositionData)
    {
        monsterData[unitPositionData.UnitIndex].LookAtPattern(unitPositionData.Dir);
        monsters[unitPositionData.UnitIndex].transform.position = new Vector3(unitPositionData.PosX, unitPositionData.PosY, unitPositionData.PosZ);
    }

    public void CharacterState(UnitStateData unitStateData)
    {
        characterData[unitStateData.UnitIndex].CharState(unitStateData.State);
    }

    public void MonsterState(UnitStateData unitStateData)
    {
        monsterData[unitStateData.UnitIndex].Pattern((StatePosition)unitStateData.State);
    }
}