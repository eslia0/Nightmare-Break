using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterCreateUI : MonoBehaviour {

	public const int maxClass = 4;
    private const int rotateValue = 30;
    public const int minClass = 2;
    private const int maxRotateBtn = 2;
	private string nickName; 
	private int currentPickClass;
    private int currentGender; //0은 남자, 1은 여자
    private bool[] btnPushCheck;
    private Animator characterAnim;
	private GameObject[] selectImage;
	private GameObject[] genderSelectImage;
	private Transform characterPos;
    [SerializeField]
	private GameObject[] classPrefeb;
	private InputField nickNameInputField;
	private Button characterCreateBtn;
	private Button[] rotateBtn;
	private Button cancleBtn;
	private Button[] genderBtn;
	private GameObject[] classSkill;
	private Button[] classBtn;

	void Start()
	{
        currentGender = 0;
        rotateBtn = new Button[maxRotateBtn];
        btnPushCheck = new bool[maxRotateBtn];
        genderSelectImage = new GameObject[minClass];
        genderBtn = new Button[minClass];
        classSkill = new GameObject[minClass];
		selectImage = new GameObject[maxClass];
        classPrefeb = new GameObject[maxClass];
		classBtn = new Button[maxClass];

        characterPos = GameObject.Find("CharacterPrefebPos").transform;
        nickNameInputField = GameObject.Find("NickNameInput").GetComponent<InputField>();
        characterCreateBtn = GameObject.Find("CharacterCreateBtn").GetComponent<Button>();
        cancleBtn = GameObject.Find("CancleBtn").GetComponent<Button>();

        for (int i = 0; i < maxClass; i++) {
			classBtn[i] = GameObject.Find ("ClassBtn"+(i + 1)).GetComponent<Button> ();
			classBtn[i].onClick.AddListener (() => ClassSelect (i));
			selectImage[i] = GameObject.Find("Select" + (i + 1));
			selectImage[i].SetActive(false);
		}

        for(int i = 0; i < genderSelectImage.Length; i++)
        {
            rotateBtn[i] = GameObject.Find("RotateArrow" + i).GetComponent<Button>();
            classSkill[i] = GameObject.Find("SkillUI" + i);
            genderSelectImage[i] = GameObject.Find("Gender" + i).transform.GetChild(0).gameObject;
            genderBtn[i] = GameObject.Find("Gender" + i).transform.GetChild(1).gameObject.GetComponent<Button>();
            classSkill[i].SetActive(false);
            genderSelectImage[i].SetActive(false);
        }

		for (int i = 0; i < classPrefeb.Length; i++)
		{
            classPrefeb[i] = Instantiate(Resources.Load<GameObject>("UI/Class" + i), characterPos.transform) as GameObject; 
			classPrefeb[i].transform.position = characterPos.position;
		}
    }

	public void PrefebRotate(int _index)
	{
        btnPushCheck[_index] = true;
        StartCoroutine(CharacterRotate(_index));
	}

    public void RotateCheck(int _index)
    {
        btnPushCheck[_index] = false;
    }

	public void CreateCharacter()
	{
		SceneChanger.Instance.SceneChange(SceneChanger.SceneName.SelectScene, false);
	}

	public void Cancle()
	{
		SceneChanger.Instance.SceneChange(SceneChanger.SceneName.SelectScene, false);
    }

	public void InputFinish()
	{
		nickName = nickNameInputField.text;
		characterCreateBtn.interactable = true;
	}

	public void ClassSelect(int index)
	{
		for (int i = 0; i < maxClass * minClass; i++) {
			if (i < maxClass) {
				selectImage [i].SetActive (false);
                classPrefeb [i].SetActive(false);
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
            currentPickClass = index + index;
			classPrefeb[index + index].SetActive(true);
			characterAnim = classPrefeb[index + index].GetComponent<Animator>();
            for(int i=0; i < minClass; i++)
            {
				genderBtn[i].interactable = true;
                rotateBtn[i].interactable = true;
            }
        }
	}

	public void GenderChange(int _genderindex)
	{
		if (currentGender == _genderindex) {
			return;
		} else if (_genderindex == 0) {
			classPrefeb [_genderindex + currentGender + currentPickClass].SetActive (false);
			genderSelectImage[currentGender].SetActive(false);
            currentGender = _genderindex;
			classPrefeb [currentGender + currentPickClass].SetActive (true);
			genderSelectImage[currentGender].SetActive(true);
            characterAnim = classPrefeb[currentGender].GetComponent<Animator>();
        }
        else if  (_genderindex == 1) {
			classPrefeb [currentGender + currentPickClass].SetActive (false);
			genderSelectImage[currentGender].SetActive(false);
            currentGender = _genderindex;
			genderSelectImage[currentGender].SetActive(true);
            classPrefeb [currentGender + currentPickClass].SetActive (true);
			characterAnim = classPrefeb[currentGender + currentPickClass].GetComponent<Animator>();
		}
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
				classPrefeb[currentPickClass + currentGender].transform.Rotate(0, rotateValue * time, 0);
            }
            else if (btnPushCheck[1])
            {
				classPrefeb[currentPickClass + currentGender].transform.Rotate(0, -rotateValue * time, 0);
            }
            yield return null;
        }
        time = 0;
    }

}
