using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WaitingUIManager : MonoBehaviour
{
    public const int maxCharacterNum = 3;
    public const int maxRoomNum = 20;
    public const int maxPlayerNum = 4;
    public const int maxSkillNum = 6;

    private Button refreshBtn;
    private Button roomCreateBtn;
    private Button roomEntereBtn;
    private Button skillAddBtn;
    private Button equipInfoBtn;
    private Button myInfoBtn;
    private Button returnToSelectBtn;
    private Button gameExitBtn;
    private Button nextDungeonLevelBtn;
    private Button previousDungeonLevelBtn;
    private Button roomCreateExitBtn;
    private Button skillAddExitBtn;
    private Button equipInfoExitBtn;
    private Button myInfoExitBtn;
    private Button roomInfoExitBtn;
    private Button roomCreateYesBtn;
	private Button[] roomBtn;
    private Button[] skillAddIcon;

    private InputField createroomName;

	private GameObject roomCreateUI;
	private GameObject skillAddUI;
	private GameObject equipInfoUI;
	private GameObject myInfoUI;
	private GameObject roomInfoUI;

    private Text mySkillInfo;
    private Text myInfoData;
    private Text dungeonLevelText;
    private Text[] roomIndex;
    private Text[] roomName;
    private Text[] roomDungeonLevel;
    private Text[] roomCurrentUser;
	private Text[] roomInfoUserName;

	private Image equipWeaponIcon;
    private Image lockImage;
    private Image[] roomInfoClassIcon;
	private Image[] roomInfoGenderIcon;
    private Image[] skillAddSelectImage;

    private EventTrigger.Entry[] mouseOverIn;
    private EventTrigger.Entry[] mouseOverOut;

    Room[] rooms;

    int currentRoomNum;
    int dungeonId;
    int dungeonLevel;

    public CharacterStatus characterStatus;

    public Room[] Rooms { get { return rooms; } }
    public int CurrentRoomNum { get { return currentRoomNum; } }

    void Start()
    {

    }

    public void ManagerInitialize()
    {
        characterStatus = GameObject.Find("CharacterStatus").GetComponent<CharacterStatus>();
        currentRoomNum = -1;
        dungeonId = 0;
        dungeonLevel = 1;

        SetUIObject();
        InitializeAddListner();

        roomCreateUI.SetActive(false);
        skillAddUI.SetActive(false);
        equipInfoUI.SetActive(false);
        myInfoUI.SetActive(false);
        roomInfoUI.SetActive(false);
        lockImage.gameObject.SetActive(false);
    }

    public void SetUIObject()
    {
		roomBtn = new Button[maxRoomNum];
        roomName = new Text[maxRoomNum];
        roomDungeonLevel = new Text[maxRoomNum];
        roomCurrentUser = new Text[maxRoomNum];
		roomInfoUserName = new Text[maxPlayerNum];
        roomIndex = new Text[maxRoomNum];
        roomInfoClassIcon = new Image[maxPlayerNum];
		roomInfoGenderIcon = new Image[maxPlayerNum];
        skillAddIcon = new Button[maxSkillNum];
        skillAddSelectImage = new Image[maxSkillNum];
        mouseOverIn = new EventTrigger.Entry[maxSkillNum];
        mouseOverOut = new EventTrigger.Entry[maxSkillNum];

        mySkillInfo = GameObject.Find("MouseOverUI").GetComponent<Text>();
        lockImage = GameObject.Find("LockObject").GetComponent<Image>();
        myInfoData = GameObject.Find("MyInfoData").GetComponent<Text>();
        roomCreateUI = GameObject.Find("RoomCreateUI");
        skillAddUI = GameObject.Find("SkillAddUI");
        equipInfoUI = GameObject.Find("EquipInfoUI");
        myInfoUI = GameObject.Find("MyInfoUI");
        roomInfoUI = GameObject.Find("RoomInfoUI");
        refreshBtn = GameObject.Find("RefreshBtn").GetComponent<Button>();
        roomCreateBtn = GameObject.Find("RoomCreateBtn").GetComponent<Button>();
        roomEntereBtn = GameObject.Find("RoomEnterBtn").GetComponent<Button>();
        roomCreateYesBtn = GameObject.Find("RoomCreateYesBtn").GetComponent<Button>();
        skillAddBtn = GameObject.Find("SkillAddBtn").GetComponent<Button>();
        equipInfoBtn = GameObject.Find("EquipBtn").GetComponent<Button>();
        myInfoBtn = GameObject.Find("MyInfoBtn").GetComponent<Button>();
        returnToSelectBtn = GameObject.Find("ReturnToSelectBtn").GetComponent<Button>();
        gameExitBtn = GameObject.Find("GameExitBtn").GetComponent<Button>();
        dungeonLevelText = GameObject.Find("DungeonLevelText").GetComponent<Text>();
        createroomName = GameObject.Find("RoomCreateInputField").GetComponent<InputField>();
        nextDungeonLevelBtn = GameObject.Find("NextDungeonLevelBtn").GetComponent<Button>();
        previousDungeonLevelBtn = GameObject.Find("PreviousDungeonLevelBtn").GetComponent<Button>();

        equipWeaponIcon = equipInfoUI.transform.FindChild("Weapon").GetComponent<Image>();
        roomCreateExitBtn = roomCreateUI.transform.FindChild("ExitBtn").GetComponent<Button>();
        skillAddExitBtn = skillAddUI.transform.FindChild("ExitBtn").GetComponent<Button>();
        equipInfoExitBtn = equipInfoUI.transform.FindChild("ExitBtn").GetComponent<Button>();
        myInfoExitBtn = myInfoUI.transform.FindChild("ExitBtn").GetComponent<Button>();
        roomInfoExitBtn = roomInfoUI.transform.FindChild("ExitBtn").GetComponent<Button>();

        mySkillInfo.transform.parent.gameObject.SetActive(false);
        dungeonLevelText.text = dungeonLevel.ToString();
		for (int i = 0; i < skillAddIcon.Length; i++) {
            skillAddSelectImage[i] = skillAddUI.transform.FindChild("SkillSelect" + (i + 1)).GetComponent<Image>();
            skillAddIcon[i] = skillAddUI.transform.FindChild("Skill" + (i + 1)).GetComponent<Button>();
            skillAddIcon[i].image.sprite = Resources.Load<Sprite> ("UI/SkillIcon/" + characterStatus.HClass.ToString ()+"/Skill"+(i+1));
            skillAddSelectImage[i].gameObject.SetActive(false);
		}
		for (int i = 0; i < maxRoomNum; i++) {
			roomBtn [i] = GameObject.Find ("Room" + (i + 1)).GetComponent<Button> ();
            roomIndex[i] = roomBtn[i].transform.GetChild(0).GetComponent<Text>();
            roomName [i] = roomBtn [i].transform.GetChild (1).GetComponent<Text> ();
            roomDungeonLevel [i] = roomBtn [i].transform.GetChild (2).GetComponent<Text> ();
			roomCurrentUser [i] = roomBtn [i].transform.GetChild (3).GetComponent<Text> ();
		}
        for (int i = 0; i < maxPlayerNum; i++)
        {
            roomInfoClassIcon[i] = roomInfoUI.transform.FindChild("ClassIcon" + (i + 1)).GetComponent<Image>();
            roomInfoUserName[i] = roomInfoClassIcon[i].transform.GetChild(0).GetComponent<Text>();
            roomInfoGenderIcon[i] = roomInfoClassIcon[i].transform.GetChild(1).GetComponent<Image>();
        }
    }

    public void InitializeAddListner()
    {
        refreshBtn.onClick.AddListener(() => OnClickRefreash());
        roomCreateBtn.onClick.AddListener(() => RoomCreate());
        roomEntereBtn.onClick.AddListener(() => OnClickEnterRoomButton());
        roomCreateYesBtn.onClick.AddListener(() => OnClickCreateRoomButton()); 
        skillAddBtn.onClick.AddListener(() => SkillAdd());
        equipInfoBtn.onClick.AddListener(() => EquipInfo());
        myInfoBtn.onClick.AddListener(() => MyInfo());
        returnToSelectBtn.onClick.AddListener(() => OnClickReturnToSelect());
        gameExitBtn.onClick.AddListener(() => OnClickGameExit());
        roomCreateExitBtn.onClick.AddListener(() => UIActiveCheck());
        skillAddExitBtn.onClick.AddListener(() => UIActiveCheck());
        equipInfoExitBtn.onClick.AddListener(() => UIActiveCheck());
        myInfoExitBtn.onClick.AddListener(() => UIActiveCheck());
        roomInfoExitBtn.onClick.AddListener(() => UIActiveCheck());
        nextDungeonLevelBtn.onClick.AddListener(() => DungeonLevelUP());
        previousDungeonLevelBtn.onClick.AddListener(() => DungeonLevelDown());

        roomBtn [0].onClick.AddListener (() => RoomInfo (0));
		roomBtn [1].onClick.AddListener (() => RoomInfo (1));
		roomBtn [2].onClick.AddListener (() => RoomInfo (2));
		roomBtn [3].onClick.AddListener (() => RoomInfo (3));
		roomBtn [4].onClick.AddListener (() => RoomInfo (4));
		roomBtn [5].onClick.AddListener (() => RoomInfo (5));
		roomBtn [6].onClick.AddListener (() => RoomInfo (6));
		roomBtn [7].onClick.AddListener (() => RoomInfo (7));
		roomBtn [8].onClick.AddListener (() => RoomInfo (8));
		roomBtn [9].onClick.AddListener (() => RoomInfo (9));
		roomBtn [10].onClick.AddListener (() => RoomInfo (10));
		roomBtn [11].onClick.AddListener (() => RoomInfo (11));
		roomBtn [12].onClick.AddListener (() => RoomInfo (12));
		roomBtn [13].onClick.AddListener (() => RoomInfo (13));
		roomBtn [14].onClick.AddListener (() => RoomInfo (14));
		roomBtn [15].onClick.AddListener (() => RoomInfo (15));
		roomBtn [16].onClick.AddListener (() => RoomInfo (16));
		roomBtn [17].onClick.AddListener (() => RoomInfo (17));
		roomBtn [18].onClick.AddListener (() => RoomInfo (18));
		roomBtn [19].onClick.AddListener (() => RoomInfo (19));

        skillAddIcon[0].onClick.AddListener(() => SkillInfoEnter(0));
        skillAddIcon[1].onClick.AddListener(() => SkillInfoEnter(1));
        skillAddIcon[2].onClick.AddListener(() => SkillInfoEnter(2));
        skillAddIcon[3].onClick.AddListener(() => SkillInfoEnter(3));
        skillAddIcon[4].onClick.AddListener(() => SkillInfoEnter(4));

    }

    public void SkillInfoEnter(int skillIndex)
    {
        SkillBasicData skillData = SkillManager.instance.SkillData.GetSkill(1, skillIndex + 1);  // 고쳐야함
        for(int i=0; i< skillAddIcon.Length; i++)
        {
            if (skillAddSelectImage[i].IsActive())
            {
                skillAddSelectImage[i].gameObject.SetActive(false);
            }
        }
        skillAddSelectImage[skillIndex].gameObject.SetActive(true);
        mySkillInfo.transform.parent.gameObject.SetActive(true);
        mySkillInfo.transform.parent.SetParent(skillAddIcon[skillIndex].transform);
        mySkillInfo.transform.parent.position = Vector3.zero;
        mySkillInfo.transform.parent.position = new Vector3(119f, 31f, 0f);
     //   mySkillInfo.text = "스킬이름: " + skillData.SkillName + "  " + "쿨타임: " + skillData.SkillCoolTime.ToString() + "초" + "\n" + skillData.SkillBasicExplanation + "\n" + skillData.GetSkillData(skillLevel).SkillExplanation;
    }

    public void SkillInfoExit()
    {
        mySkillInfo.transform.parent.gameObject.SetActive(false);
    }

    public void RoomCreate()
	{
		UIActiveCheck ();
		roomCreateUI.SetActive (true);
        lockImage.gameObject.SetActive(true);
    }

	public void SkillAdd()
	{
		UIActiveCheck ();
		skillAddUI.SetActive (true);
        lockImage.gameObject.SetActive(true);
    }

	public void EquipInfo()
	{
		UIActiveCheck ();
		equipInfoUI.SetActive(true);
        lockImage.gameObject.SetActive(true);
    }

    public void MyInfo()
    {
        UIActiveCheck();
        myInfoUI.SetActive(true);
        lockImage.gameObject.SetActive(true);
    }

    public void DungeonLevelUP()
    {
        dungeonLevel++;
        if (dungeonLevel > 10)
        {
            dungeonLevel = 1;
        }
        dungeonLevelText.text = dungeonLevel.ToString();
    }

    public void DungeonLevelDown()
    {
        dungeonLevel--;
        if (dungeonLevel < 1)
        {
            dungeonLevel = 10;
        }
        dungeonLevelText.text = dungeonLevel.ToString();
    }

    public void UIActiveCheck()
	{
		if (roomCreateUI.activeSelf) {
			createroomName.text = "";
            dungeonLevel = 1;
            roomCreateUI.SetActive (false);
		} else if (skillAddUI.activeSelf) {
			skillAddUI.SetActive (false);
		} else if (equipInfoUI.activeSelf) {
			equipInfoUI.SetActive (false);
		} else if (myInfoUI.activeSelf) {
			myInfoUI.SetActive (false);
		} else if (roomInfoUI.activeSelf) {
			roomInfoUI.SetActive (false);
		}
        lockImage.gameObject.SetActive(false);
    }

	public void RoomInfo(int roomNum)
	{
		UIActiveCheck ();
		roomInfoUI.SetActive (true);
        for (int i = 0; i < maxPlayerNum; i++)
        {
            if (rooms[roomNum].PlayerNum > 0)
            {
                roomInfoClassIcon[i].sprite = Resources.Load<Sprite>("UI/RoomClassIcon/Class" + rooms[roomNum].RoomUserData[i].UserClass);
                roomInfoUserName[i].text = rooms[roomNum].RoomUserData[i].UserName;
                roomInfoGenderIcon[i].sprite = Resources.Load<Sprite>("UI/RoomGenderIcon/Gender" + rooms[roomNum].RoomUserData[i].UserGender);
            }
        }
        currentRoomNum = roomNum;
    }

    public void SetRoomListData(RoomListData roomListData)
    {
        rooms = roomListData.Rooms;
    }

    public void SetRoom()
    {
		for (int i = 0; i < maxRoomNum; i++) {
            if (rooms[i].PlayerNum != 0)
			{
				roomBtn [i].image.sprite = Resources.Load<Sprite> ("UI/WaitingRoomImage/RoomActive");
				roomIndex[i].text = "00"+(i+1);
				roomName [i].text = rooms [i].RoomName;
				roomDungeonLevel [i].text = rooms [i].DungeonLevel.ToString();
				roomCurrentUser [i].text = (rooms [i].PlayerNum.ToString () + "/" + maxPlayerNum.ToString ());
			}
            else
            {
				roomBtn [i].image.sprite = Resources.Load<Sprite> ("UI/WaitingRoomImage/RoomNotActive") as Sprite;
				roomIndex [i].text = "";
                roomName[i].text = "";
                roomDungeonLevel[i].text = "";
                roomCurrentUser[i].text = "";
            }
		}
    }

    public void OnClickCreateRoomButton()
    {
        DataSender.Instance.CreateRoom(createroomName.text, dungeonId, dungeonLevel);
    }

    public void OnClickEnterRoomButton()
    {
        if (currentRoomNum >= 0)
        {
            DataSender.Instance.EnterRoom(currentRoomNum);
        }        
    }

    public void OnClickReturnToSelect()
    {
        DataSender.Instance.ReturnToSelect();
    }

    public void OnClickRefreash()
    {
        DataSender.Instance.RequestRoomList();
    }

    public void OnClickGameExit()
    {
        Application.Quit();
    }

    public void SetMyInfoData()
    {
        /*
        myInfoData.text = "\n" + "공격력:" + characterStatus.Attack.ToString() + "\n" + "방어력: " + characterStatus.Defense.ToString() + "\n" + "체력: " + characterStatus.MaxHealthPoint.ToString()
             + "\n" + "마나: " + characterStatus.MaxMagicPoint.ToString() + "\n" + "체력회복: " + characterStatus.HpRegeneration.ToString() + "\n" + "마나회복: " + characterStatus.MpRegeneration.ToString()
             + "\n" + "공격력 증가량: 10" + "\n" + "방어력 증가량: 10" + "\n" + "체력 증가량: 10" + "\n" + "마나 증가량: 10" + "\n" + "체력회복 증가량: 10" + "\n" + "마나회복 증가량: 10";
        */
    }

}

public class Room
{
    string roomName;
    string dungeonName;
    int dungeonId;
    int dungeonLevel;
	int playerNum;
    RoomUserData[] roomUserData;

    public string RoomName { get { return roomName; } }
    public string DungeonName { get { return dungeonName; } }
    public int DungeonId { get { return dungeonId; } }
    public int DungeonLevel { get { return dungeonLevel; } }
	public int PlayerNum { get { return playerNum; } }
    public RoomUserData[] RoomUserData { get { return roomUserData; } }
    
    public Room()
    {
        roomName = "";
        dungeonName = "";
        playerNum = 0;
        dungeonId = 0;
        dungeonLevel = 0;
        roomUserData = new RoomUserData[WaitingUIManager.maxPlayerNum];

        for (int i = 0; i < WaitingUIManager.maxPlayerNum; i++)
        {
            roomUserData[i] = new RoomUserData();
        }
    }

    public Room(string newRoomName, int newDungeonId, int newDungeonLevel, RoomUserData[] newRoomUserData, int newPlayerNum)
    {
        roomName = newRoomName;
        dungeonName = "";
        dungeonId = newDungeonId;
        dungeonLevel = newDungeonLevel;
        roomUserData = new RoomUserData[newRoomUserData.Length];
        playerNum = newPlayerNum;

        for (int i = 0; i < newRoomUserData.Length; i++)
        {
            roomUserData[i] = new RoomUserData(newRoomUserData[i].UserName, newRoomUserData[i].UserGender, newRoomUserData[i].UserClass, newRoomUserData[i].UserLevel);
        }
    }
}

public class RoomUserData
{
    string userName;
    int userGender;
    int userClass;
    int userLevel;

    public string UserName { get { return userName; } }
    public int UserGender { get { return userGender; } }
    public int UserClass { get { return userClass; } }
    public int UserLevel { get { return userLevel; } }

    public RoomUserData()
    {
        userName = "";
        userGender = 0;
        userClass = 0;
        userLevel = 0;
    }

    public RoomUserData(string newUserName, int newUserGender, int newUserClass, int newUserLevel)
    {
        userName = newUserName;
        userGender = newUserGender;
        userClass = newUserClass;
        userLevel = newUserLevel;
    }
}