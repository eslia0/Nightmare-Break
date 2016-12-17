using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateUIManager : MonoBehaviour
{
	private const int maxClass = 4;
    private const int rotateValue = 30;
    private const int currentMaxClass = 2;
    private const int maxRotateBtn = 2;
    private const int maxGender = 2;

    private GameObject[] classPrefeb;
    private Transform characterPos;
    private Animator characterAnim;

    private int currentClass;
    private int currentGender; //0은 남자, 1은 여자
    private int currentCharacter
    {
        get
        {
            return (currentClass * maxGender) + currentGender;
        }
    }

    private bool[] btnPushCheck;

	private GameObject[] selectImage;
	private GameObject[] genderSelectImage;
    private GameObject[] classSkill;

    private Text nickName;

    private Button[] classBtn;
    private Button[] rotateBtn;
    private Button[] genderBtn;
    private Button[] warriorSkillBtn;
    private Button[] mageSkillBtn;
    private Button characterCreateBtn;
    private Button cancleBtn;

    private EventTrigger.Entry[] exitEvent;
	private EventTrigger.Entry[] downEvent;
	private EventTrigger.Entry[] upEvent;
 
    public void ManagerInitialize()
    {
        SetUIObject();
        InitializeAddListner();
    }

    public void SetUIObject()
    {
        currentGender = 0;
        rotateBtn = new Button[maxRotateBtn];
        btnPushCheck = new bool[maxRotateBtn];
        genderSelectImage = new GameObject[currentMaxClass];
        genderBtn = new Button[currentMaxClass];
        classSkill = new GameObject[currentMaxClass];
        selectImage = new GameObject[maxClass];
        classPrefeb = new GameObject[maxClass];
		classBtn = new Button[maxClass];
        warriorSkillBtn = new Button[maxClass];
        mageSkillBtn = new Button[maxClass];

        characterPos = GameObject.Find("CharacterPrefebPos").transform;
        nickName = GameObject.Find("NickName").GetComponent<Text>();
        characterCreateBtn = GameObject.Find("CharacterCreateBtn").GetComponent<Button>();
        cancleBtn = GameObject.Find("CancleBtn").GetComponent<Button>();

		exitEvent = new EventTrigger.Entry[rotateBtn.Length];
		downEvent = new EventTrigger.Entry[rotateBtn.Length];
		upEvent = new EventTrigger.Entry[rotateBtn.Length];

        for (int i = 0; i < maxClass; i++)
        {
			classBtn[i] = GameObject.Find ("ClassBtn"+(i + 1)).GetComponent<Button> ();
            selectImage[i] = GameObject.Find("Select" + (i + 1));
            warriorSkillBtn[i] = GameObject.Find("WarriorSkill" + (i + 1)).GetComponent<Button>();
            mageSkillBtn[i] = GameObject.Find("MageSkill" + (i + 1)).GetComponent<Button>();
            selectImage[i].SetActive(false);
        }

        for (int i = 0; i < genderSelectImage.Length; i++)
        {
            exitEvent[i] = new EventTrigger.Entry();
            downEvent[i] = new EventTrigger.Entry();
            upEvent[i] = new EventTrigger.Entry();
			exitEvent [i].eventID = EventTriggerType.PointerExit;
			downEvent [i].eventID = EventTriggerType.PointerDown;
			upEvent [i].eventID = EventTriggerType.PointerUp;
            rotateBtn[i] = GameObject.Find("RotateArrow" + i).GetComponent<Button>();
			rotateBtn [i].GetComponent<EventTrigger> ().triggers.Add (exitEvent [i]);
			rotateBtn [i].GetComponent<EventTrigger> ().triggers.Add (downEvent [i]);
			rotateBtn [i].GetComponent<EventTrigger> ().triggers.Add (upEvent [i]);
            classSkill[i] = GameObject.Find("SkillUI" + i);
            genderSelectImage[i] = GameObject.Find("Gender" + i).transform.GetChild(0).gameObject;
            genderBtn[i] = GameObject.Find("Gender" + i).transform.GetChild(1).gameObject.GetComponent<Button>();
			genderBtn [i].onClick.AddListener (() => GenderChange (i));
            classSkill[i].SetActive(false);
            genderSelectImage[i].SetActive(false);
        }

        for (int i = 0; i < classPrefeb.Length; i++)
        {
            classPrefeb[i] = Instantiate(Resources.Load<GameObject>("UI/Class" + (i + 1)), characterPos.transform) as GameObject;
            classPrefeb[i].transform.position = characterPos.position;
            classPrefeb[i].SetActive(false);
        }

    }

    public void InitializeAddListner()
    {
        characterCreateBtn.onClick.AddListener(() => OnClickCreateCharacterButton());
        cancleBtn.onClick.AddListener(() => Cancle());
        warriorSkillBtn[0].onClick.AddListener(() => StartSkillAnim(1));
        warriorSkillBtn[1].onClick.AddListener(() => StartSkillAnim(2));
        warriorSkillBtn[2].onClick.AddListener(() => StartSkillAnim(3));
        warriorSkillBtn[3].onClick.AddListener(() => StartSkillAnim(4));

        mageSkillBtn[0].onClick.AddListener(() => StartSkillAnim(1));
        mageSkillBtn[1].onClick.AddListener(() => StartSkillAnim(2));
        mageSkillBtn[2].onClick.AddListener(() => StartSkillAnim(3));
        mageSkillBtn[3].onClick.AddListener(() => StartSkillAnim(4));

        genderBtn[0].onClick.AddListener (() => GenderChange (0));
		genderBtn [1].onClick.AddListener (() => GenderChange (1));
		classBtn [0].onClick.AddListener (() => ClassSelect (0));
		classBtn [1].onClick.AddListener (() => ClassSelect (1));
		classBtn [2].onClick.AddListener (() => ClassSelect (2));
		classBtn [3].onClick.AddListener (() => ClassSelect (3));

		exitEvent[0].callback.AddListener((data)=> RotateCheckOut(0));
		exitEvent[1].callback.AddListener((data)=> RotateCheckOut(1));
		downEvent[0].callback.AddListener((data)=> PrefebRotate(0));
		downEvent[1].callback.AddListener((data)=> PrefebRotate(1));

		upEvent[0].callback.AddListener((data)=> RotateCheckOut(0));
		upEvent[1].callback.AddListener((data)=> RotateCheckOut(1));
    }

	public void PrefebRotate(int _index)
	{
        btnPushCheck[_index] = true;
        StartCoroutine(CharacterRotate(_index));
	}

    public void RotateCheckOut(int _index)
    {
        btnPushCheck[_index] = false;
    }

    public void CreateCharacterResult(bool result)
    {
        if (result)
        {
            Debug.Log("캐릭터 생성 성공");
            SceneChanger.Instance.SceneChange(SceneChanger.SceneName.SelectScene, false);
        }
        else
        {
            Debug.Log("캐릭터 생성 실패");
        }
    }

	public void Cancle()
	{
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.SelectScene, false);
    }

	public void ClassSelect(int index)
	{
		for (int i = 0; i < maxClass * currentMaxClass; i++) {
			if (i < maxClass) {
				selectImage [i].SetActive (false);
                classPrefeb[currentCharacter].transform.rotation = new Quaternion(0, 180, 0, 0);
            }

            if (i < genderSelectImage.Length) {
				genderSelectImage [i].SetActive (false);
                classSkill[i].SetActive(false);
            }
        }

		if (!selectImage [index].activeSelf) {
            selectImage [index].SetActive (true);
			classSkill [index].SetActive (true);
			currentGender = 0;
			genderSelectImage [currentGender].SetActive (true);
            currentClass = index;
			
            for(int i=0; i < currentMaxClass; i++)
            {
				genderBtn[i].interactable = true;
                rotateBtn[i].interactable = true;
            }
        }
        SetCharacterImage();

        characterAnim = classPrefeb[currentCharacter].GetComponent<Animator>();
    }

	public void GenderChange(int _genderindex)
	{
		if (currentGender == _genderindex) {
			return;
		} else if (_genderindex == 0) {
			genderSelectImage[currentGender].SetActive(false);
            currentGender = _genderindex;
			genderSelectImage[currentGender].SetActive(true);
            characterAnim = classPrefeb[currentGender].GetComponent<Animator>();
        }
        else if  (_genderindex == 1) {
			genderSelectImage[currentGender].SetActive(false);
            currentGender = _genderindex;
			genderSelectImage[currentGender].SetActive(true);
			characterAnim = classPrefeb[currentGender + currentClass].GetComponent<Animator>();
        }
        SetCharacterImage();
    }

    public void SetCharacterImage()
    {
        for (int classIndex = 0; classIndex < currentMaxClass; classIndex++)
        {
            for (int genderIndex = 0; genderIndex < maxGender; genderIndex++)
            {
                if (classPrefeb[(classIndex * maxGender) + genderIndex].activeSelf)
                {
                    classPrefeb[(classIndex * maxGender) + genderIndex].SetActive(false);
                    classPrefeb[(classIndex * maxGender) + genderIndex].transform.rotation = new Quaternion(0, 180, 0, 0);
                }
            }
        }
        classPrefeb[currentCharacter].SetActive(true);
        characterAnim = classPrefeb[currentCharacter].GetComponent<Animator>();
    }

	public void StartSkillAnim(int _skillNum)
	{
		characterAnim.SetTrigger ("Skill" + _skillNum);
	}

    IEnumerator CharacterRotate(int _index)
    {
        float time = Time.smoothDeltaTime;
        while(btnPushCheck[_index])
        {
            if (btnPushCheck[0])
            {
				classPrefeb[currentCharacter].transform.Rotate(0, rotateValue * time, 0);
            }
            else if (btnPushCheck[1])
            {
				classPrefeb[currentCharacter].transform.Rotate(0, -rotateValue * time, 0);
            }
            yield return null;
        }
        time = 0;
    }

    public void OnClickCreateCharacterButton()
    {
        if (nickName.text.Length < 1)
        {
            Debug.Log("캐릭터 이름을 1글자 이상 입력하세요");
        }
        else
        {
            DataSender.Instance.CreateCharacter(currentGender, currentClass, nickName.text);
        }
    }
}
