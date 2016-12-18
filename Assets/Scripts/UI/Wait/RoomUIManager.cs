using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomUIManager : MonoBehaviour {

    private const int maxUser = 4;
    private const int maxSkill = 6;

    private Button skillBtn;
    private Button equipBtn;
    private Button myInfoBtn;
    private Button gameStartBtn;
    private Button roomExitBtn;

    private Button equipCloseBtn;
    private Button skillCloseBtn;
    private Button myInfoCloseBtn;
    private Button[] userSkillBtn;
    private Button[] skillAddIcon;
 
    private Image[] characterBackImage;
    private Image[] skillSelectImage;
    private Image[] classIcon;
    private Text[] userName;

    private GameObject equipInfoUI;
    private GameObject skillAddUI;
    private GameObject myInfoUI;
    private GameObject[] rendPos;
    private GameObject[] playerPrefeb;

    private RoomData roomData;
    private Text[] myInfoText;

    Text roomName;
    Text roomDungeon;
    Text roomDungeonLevel;
    int dungeonId;
    int dungeonLevel;

    int roomNum;
    
    public int DungeonId { get { return dungeonId; } }
    public int DungeonLevel { get { return dungeonLevel; } }

    public void ManagerInitialize()
    {
        SetUIObject();
        InitializeAddListner();
    }

    public void SetUIObject()
    {
        rendPos = new GameObject[maxUser];
        playerPrefeb = new GameObject[maxUser];
        userName = new Text[maxUser];
        classIcon = new Image[maxUser];
        skillSelectImage = new Image[CharacterStatus.skillNum];
        skillAddIcon = new Button[CharacterStatus.skillNum];

        characterBackImage = new Image[maxUser];
        userSkillBtn = new Button[maxSkill];
        roomName = GameObject.Find("RoomName").GetComponent<Text>();
        roomDungeon = GameObject.Find("DungeonType").GetComponent<Text>();
        roomDungeonLevel = GameObject.Find("DungeonLevel").GetComponent<Text>();

        skillBtn = GameObject.Find("SkillBtn").GetComponent<Button>();
        equipBtn = GameObject.Find("EquipBtn").GetComponent<Button>();
        myInfoBtn = GameObject.Find("MyInfoBtn").GetComponent<Button>();

        gameStartBtn = GameObject.Find("GameStartBtn").GetComponent<Button>();
        roomExitBtn = GameObject.Find("RoomExitBtn").GetComponent<Button>();

        equipInfoUI = GameObject.Find("EquipInfoUI");
        skillAddUI = GameObject.Find("SkillAddUI");
        myInfoUI = GameObject.Find("MyInfoUI");

        equipCloseBtn = equipInfoUI.transform.FindChild("ExitBtn").GetComponent<Button>();
        skillCloseBtn = skillAddUI.transform.FindChild("ExitBtn").GetComponent<Button>();
        myInfoCloseBtn = myInfoUI.transform.FindChild("ExitBtn").GetComponent<Button>();

        equipInfoUI.SetActive(false);
        skillAddUI.SetActive(false);
        myInfoUI.SetActive(false);

        for (int i = 0; i < maxUser; i++)
        {
            rendPos[i] = GameObject.Find("RendPos" + (i + 1));
            userName[i] = GameObject.Find("UserName" + (i + 1)).GetComponent<Text>();
            classIcon[i] = GameObject.Find("ClassIcon" + (i + 1)).GetComponent<Image>();
            characterBackImage[i] = GameObject.Find("CharacterBackImage" + (i + 1)).GetComponent<Image>();
        }
        for (int i = 0; i < skillAddIcon.Length; i++) {

            skillSelectImage[i] = skillAddUI.transform.FindChild("SkillSelect" + (i + 1)).GetComponent<Image>();
            skillAddIcon[i] = skillAddUI.transform.FindChild("Skill" + (i + 1)).GetComponent<Button>();
            skillAddIcon[i].image.sprite = Resources.Load<Sprite> ("UI/SkillIcon/" + CharacterStatus.Instance.HClass.ToString ()+"/Skill"+(i+1));
            skillSelectImage[i].gameObject.SetActive(false);
		}
    }

    public void InitializeAddListner()
    {
        skillBtn.onClick.AddListener(() => OpenSkillUI());
        equipBtn.onClick.AddListener(() => OpenEquipUI());
        myInfoBtn.onClick.AddListener(() => OpenMyInfoUI());
        gameStartBtn.onClick.AddListener(() => GameStart());
        roomExitBtn.onClick.AddListener(() => RoomExit());
        equipCloseBtn.onClick.AddListener(() => CloseEquipUI());
        myInfoCloseBtn.onClick.AddListener(() => CloseMyInfoUI());
        skillCloseBtn.onClick.AddListener(() => CloseSkillUI());
    }

    public void SetRoom(RoomData newRoomData)
    {
        roomName.text = newRoomData.RoomName;
        roomNum = newRoomData.RoomNum;
        dungeonId = newRoomData.DungeonId;
        dungeonLevel = newRoomData.DungeonLevel;
        roomDungeon.text = newRoomData.DungeonName;
        roomDungeonLevel.text = dungeonLevel.ToString();

        roomData = newRoomData;
        
        for (int i = 0; i < roomData.RoomUserData.Length; i++)
        {
            if (roomData.RoomUserData[i].UserLevel > 0)
            {
                if(playerPrefeb[i] == null)
                {
                    GameObject character = Instantiate(Resources.Load<GameObject>("UI/Class" + (roomData.RoomUserData[i].UserClass + (roomData.RoomUserData[i].UserGender * CharacterCreateUI.minClass) + 1)), rendPos[i].transform) as GameObject;
                    playerPrefeb[i] = character;
                    character.transform.localPosition = Vector3.zero;
                    character.transform.localRotation = new Quaternion(0, 180, 0, 0);
                    userName[i].text = "Lv." + roomData.RoomUserData[i].UserLevel.ToString() + " " + roomData.RoomUserData[i].UserName;
                    classIcon[i].sprite = Resources.Load<Sprite>("UI/RoomClassIcon/Class" + (roomData.RoomUserData[i].UserClass + 1));
                }                
            }
            else
            {
                if(playerPrefeb != null)
                {
                    Destroy(playerPrefeb[i]);
                }                
                userName[i].text = "";
                classIcon[i].color = new Color(0,0,0,0);
            }
        }
    }

    void GameStart()
    {
        DataSender.Instance.StartGame();
    }

    void RoomExit()
    {
        DataSender.Instance.ExitRoom(roomNum);
    }

    void OpenEquipUI()
    {
        equipInfoUI.SetActive(true);
    }

    void OpenSkillUI()
    {
        skillAddUI.SetActive(true);
    }

    void OpenMyInfoUI()
    {
        myInfoUI.SetActive(true);
    }

    void CloseEquipUI()
    {
        equipInfoUI.SetActive(false);
    }

    void CloseSkillUI()
    {
        skillAddUI.SetActive(false);
    }

    void CloseMyInfoUI()
    {
        myInfoUI.SetActive(false);
    }
}
