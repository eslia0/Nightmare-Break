using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Collections.Generic;
using System.Collections;

public enum UnitId
{
    ManWarrior = 0,
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

    DungeonData dungeonData;
    MonsterStatusData monsterStatusData;

    public SceneChangeObject[] sceneChangeObject;
	public BossMonsterKYW bossMonster;
    //public Section[] section;
    
    GameObject m_camera;

	[SerializeField]int mapNumber;
    int dungeonId;
    int dungeonLevel;
    int userNum;

    bool normalMode; //false  -> normalBattle, true -> Defence; 

    public GameObject[] Players { get { return players; } }
    public CharacterManager[] CharacterData { get { return characterData; } }

    public int DungeonId { get { return dungeonId; } }
    public int DungeonLevel { get { return dungeonLevel; } }
    public bool NormalMode
    {
		get { return normalMode; }
		set { normalMode = value; }
    }   

	void Start()
	{
        //test

        if (GameObject.FindGameObjectWithTag("GameManager") == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }		

		//Instantiate 스폰포인트 생성조건 - > mapNumber != 2;

		DungeonConstruct();
		CurrentScene();
		Debug.Log (mapNumber);
        if (GameObject.FindGameObjectWithTag("GameManager") == null)
        {
            InitializeMonsterSpawnPoint();

            dungeonData = new DungeonData();

			Stage stage1 = new Stage(1);
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Frog, 1, 3));
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Duck, 1, 3));
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Rabbit, 1, 2));
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Bear, 1, 1));
			Stage stage2 = new Stage(2);
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Frog, 1, 9));
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Duck, 1, 9));
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Rabbit, 1, 9));
			Stage stage3 = new Stage(3);
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Frog, 1, 3));
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Duck, 1, 3));
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Rabbit, 1, 2));
			stage1.MonsterSpawnData.Add(new MonsterSpawnData((int)MonsterId.Bear, 1, 1));

			dungeonData.Stages.Add(stage1);
			dungeonData.Stages.Add(stage2);
			dungeonData.Stages.Add(stage3);

            MonsterBaseData[] monsterBaseData = new MonsterBaseData[4];
            monsterBaseData[0] = new MonsterBaseData((int)MonsterId.Frog, "Frog");
            monsterBaseData[0].AddLevelData(new MonsterLevelData(1, 2, 0, 900, 3));
            monsterBaseData[1] = new MonsterBaseData((int)MonsterId.Duck, "Duck");
            monsterBaseData[1].AddLevelData(new MonsterLevelData(1, 3, 0, 1050, 3));
            monsterBaseData[2] = new MonsterBaseData((int)MonsterId.Rabbit, "Rabbit");
            monsterBaseData[2].AddLevelData(new MonsterLevelData(1, 5, 0, 2250, 4));
			monsterBaseData[3] = new MonsterBaseData((int)MonsterId.Bear, "Bear");
			monsterBaseData[3].AddLevelData(new MonsterLevelData(1, 25, 0, 11250, 3));

            MonsterStatusData monsterStatusData = new MonsterStatusData(4, monsterBaseData);
            SetMonsterData(monsterStatusData);

			if (mapNumber==0) {
				SpawnMonster (1);
				SetMonsterStatus (1);
			}
			if (mapNumber==1){
				SpawnMonster(1);
				SetMonsterStatus (1);
			}
			if (mapNumber==2) {
				SpawnMonster (1);
				SetMonsterStatus (1);
			}

        }
	}


	public void CurrentScene(){
		//SceneManager.GetActiveScene ().name;
		for (int i = 0; i < 2; i++) {
			if (SceneManager.GetActiveScene ().name == "LostTeddyBear_SingleType"+i) {
				mapNumber = i;
				if (i == 1) {
					normalMode = false;
				} else
					normalMode = true;
			}
		}
	}
	//void Update()
	//{
 //       for (int i = 0; i < monsters.Length; i++)
 //       {
	//		monsterData[i].MonsterUpdate();
 //       }
	//}

    //각종 매니저 초기화
    public void ManagerInitialize(int newDungeonId, int newDungeonLevel, int newUserNum)
    {
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
        userNum = newUserNum;
    }

    public void InitializePlayer(int playerNum)
    {
        players = new GameObject[playerNum];
        characterData = new CharacterManager[playerNum];
    }

    public void InitializeMonsterSpawnPoint()
    {
        monsterSpawnPoints = GameObject.FindGameObjectsWithTag("MonsterSpawnPoint");
    }

    public void SceneChange()
    {
        if (mapNumber < 2)
        {
			SceneManager.LoadScene("LostTeddyBear_SingleType"+(mapNumber+1));// loadScene;
			mapNumber++;
        }
     
    }

    void DungeonConstruct()
    {
		
		//if(mapNumber != 2){
			GameObject monsterSpawnPoint= (GameObject)Instantiate (Resources.Load ("Monster/MonsterSpawnPoint" + mapNumber.ToString()), this.transform.parent);
            //요고의 차일드를 받으세요
			
			//MonsterSet ();
		//}



        //if (mapNumber == 0) {
//        	nextSceneObject.SceneChangeObjectSet (mapNumber+1);
        //}
        //else if(mapNumber!= 0 || mapNumber!=4){
        //	nextSceneObject.SceneChangeObjectSet (mapNumber+1);
        //	beforeScneObject.SceneChangeObjectSet (mapNumber-1);
        //}
        //else if (mapNumber == 4) {
        //	beforeScneObject.SceneChangeObjec  tSet (mapNumber - 1);
        //}

    }



    public void SetMonsterSpawnList(DungeonData newDungeonData)
    {
        dungeonData = newDungeonData;
    }

    public void SpawnMonster(int stageIndex)
    {
        Stage stageData = dungeonData.GetStageData(stageIndex);

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
                monsters[monsterIndex] = CreateMonster(stageData.MonsterSpawnData[spawnIndex].MonsterId, monsterIndex, monsterSpawnPoints[monsterIndex].transform.position);
                monsterIndex++;
            }            
        }
    }

    public GameObject CreateMonster(int unitId, int unitIndex, Vector3 createPoint)
    {
        if (monsters[unitIndex] == null)
        {
            GameObject monster = null;

            if (unitId == (int)MonsterId.Frog)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/Frog"), createPoint, gameObject.transform.rotation);
            }
            else if (unitId == (int)MonsterId.Duck)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/Duck"), createPoint, gameObject.transform.rotation);
            }
            else if (unitId == (int)MonsterId.Rabbit)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/Rabbit"), createPoint, gameObject.transform.rotation);
            }
            else if (unitId == (int)MonsterId.BlackBear)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/BlackBear"), createPoint, gameObject.transform.rotation);
            }
            else if (unitId == (int)MonsterId.Bear)
            {
                monster = (GameObject)Instantiate(Resources.Load("Monster/Bear"), createPoint, gameObject.transform.rotation);
            }

            if (monster == null)
            {
                return null;
            }

            monster.transform.SetParent(transform);
            monsterData[unitIndex] = monster.GetComponent<Monster>();
            monsterData[unitIndex].MonsterId = (MonsterId)unitId;
            monsterData[unitIndex].MonsterIndex = unitIndex;

            return monster;
        }
        else { return null; }
    }

    public void SetMonsterData(MonsterStatusData newMonsterStatusData)
    {
        monsterStatusData = newMonsterStatusData;
    }

    public void SetMonsterStatus(int stageIndex)
    {
        Stage stageData = dungeonData.GetStageData(stageIndex);


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

        SceneChange();
    }

	//monsterspawnPoint getting
	//public void GetMonsterTransForm(Vector3[] _monsterTransForm){
	//	monsterTransForm = _monsterTransForm;
	//}

    public GameObject CreatePlayer(int gender, int hClass)
    {
        //여기서는 플레이어 캐릭터 딕셔너리 -> 각 직업에 따른 플레이어 스탯과 능력치, 스킬, 이름을 가지고 있음
        //딕셔너리를 사용하여 그에 맞는 캐릭터를 소환해야 하지만 Prototype 진행 시에는 고정된 플레이어를 소환하도록 함.

        int characterIndex = hClass * CharacterStatus.maxGender + hClass;

        GameObject player = Instantiate(Resources.Load("Character" + characterIndex)) as GameObject;        
        player.tag = "Player";
        player.transform.position = Vector3.zero;
        
        players[userNum] = player;

        characterData[userNum] = player.GetComponent<CharacterManager>();
        characterData[userNum].enabled = true;
        characterData[userNum].SetUserNum(userNum);

        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        StartCoroutine(m_camera.GetComponent<CameraController>().CameraCtrl(player.transform));
        
        InputManager.Instance.InitializeManager();
        StartCoroutine(InputManager.Instance.GetKeyInput());
        
        player.GetComponent<CharacterManager>().SetCharacterStatus();
        player.GetComponent<CharacterManager>().SetCharacterType();

        for (int index = 0; index < NetworkManager.Instance.UserIndex.Count; index++)
        {
            if (NetworkManager.Instance.MyIndex != NetworkManager.Instance.UserIndex[index].UserNum)
            {
                DataSender.Instance.CreateUnitSend(NetworkManager.Instance.UserIndex[index].EndPoint, (byte)characterIndex, player.transform.position.x, player.transform.position.y, player.transform.position.z);
            }
        }

        return player;
    }

    public void CreateUnit(int unitId, int unitIndex, Vector3 newPosition)
    {
        if (unitId <= (int)UnitId.WomanMage)
        {
            if (players[unitIndex] == null)
            {
                GameObject unit = Instantiate(Resources.Load("ManWarrior")) as GameObject;
                unit.transform.position = newPosition;
                unit.name = "Warrior";
                unit.tag = "Untagged";
                players[unitIndex] = unit;

                characterData[unitIndex] = unit.GetComponent<CharacterManager>();
                characterData[unitIndex].SetUserNum(unitIndex);
            }
            else
            {
                Debug.Log("이미 있는 캐릭터 인덱스");
            }
        }
        else
        {
            CreateMonster(unitId, unitIndex, monsterSpawnPoints[unitIndex].transform.position);
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