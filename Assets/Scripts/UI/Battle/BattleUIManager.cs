using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleUIManager : MonoBehaviour
{
	private const float checkTime = 0.1f;
	private const float mouseOverUI_xPos = 125f;
	private const float mouseOverUI_yPos = 105f;
    private const int maxSkillUI = 6;
    private const int maxskillCoolTimeUI = 4;
    private const int maxUser = 4;
    private const float comboCheckTime = 1.5f;

    private Text mouseOverUI;
    private Text comboText;
    private Text monsterName;
    private Text resultText;

    private Image potionCoolTimeUI;
	private Image[] skillUI;
	private Image[] skillCoolTimeUI; // 0 - SKill1 // 1 - SKill2 // 2 - Skill3 // 3 - Skill4 //
    private Image[] partyGenderIcon;
    private Image[] partyClassIcon;
    private Image hpBar;
    private Image mpBar;
    private Image monsterHpBar;
    private Image startImage;
    private Image readyImage;
    private Image resultImage;

    private Button resultCheckBtn;

    private Animator comboAnim;
   
	public Image[] SkillCoolTimeUI{ get{ return skillCoolTimeUI; }}
	public Text MouseOverUI{ get { return mouseOverUI; } set { mouseOverUI = value; } }

    public void ManagerInitialize()
    {
        SetUIObject();

        resultImage.gameObject.SetActive(false);
        comboText.transform.parent.gameObject.SetActive(false);
        mouseOverUI.transform.parent.gameObject.SetActive(false);
      //  startImage.gameObject.SetActive(false);
       // readyImage.gameObject.SetActive(false);
      // monsterHpBar.transform.parent.gameObject.SetActive(false);
    }

    public void SetUIObject()
    {
        partyGenderIcon = new Image[maxUser];
        partyClassIcon = new Image[maxUser];
        hpBar = GameObject.Find("HPBar").GetComponent<Image>();
        mpBar = GameObject.Find("MPBar").GetComponent<Image>();
        startImage = GameObject.Find("StartImage").GetComponent<Image>();
        readyImage = GameObject.Find("ReadyImage").GetComponent<Image>();
        StartCoroutine(StartProcess());
        resultImage = GameObject.Find("ResultImage").GetComponent<Image>();
        resultCheckBtn = resultImage.transform.GetChild(0).GetComponent<Button>();
        resultText = resultImage.transform.GetChild(1).GetComponent<Text>();
       
        //monsterHpBar = GameObject.Find("MonsterHPBar").GetComponent<Image>();
        //monsterName = GameObject.Find("MonsterName").GetComponent<Text>();
        mouseOverUI = GameObject.Find("MouseOverUI").GetComponent<Text>();
        potionCoolTimeUI = GameObject.Find("Potion_CoolTime").GetComponent<Image>();

        comboText = GameObject.Find("ComBoText").GetComponent<Text>();
        comboAnim = comboText.transform.parent.GetComponent<Animator>();
    
        skillUI = new Image[maxSkillUI];
        skillCoolTimeUI = new Image[maxskillCoolTimeUI];
        EventTrigger.Entry[] enterEvent = new EventTrigger.Entry[maxSkillUI];
        EventTrigger.Entry exitEvent = new EventTrigger.Entry();
        exitEvent.eventID = EventTriggerType.PointerExit;
        exitEvent.callback.AddListener((data) => { OnPointExit(); });
        for (int i = 0; i < skillUI.Length; i++)
        {
            skillUI[i] = GameObject.Find("Skill" + (i + 1)).GetComponent<Image>();
            skillUI[i].sprite = Resources.Load<Sprite>("UI/SkillIcon/" + GameManager.Instance.CharacterStatus.HClass.ToString() + "/Skill" + (i + 1));
            enterEvent[i] = new EventTrigger.Entry();
            enterEvent[i].eventID = EventTriggerType.PointerEnter;
            skillUI[i].GetComponent<EventTrigger>().triggers.Add(enterEvent[i]);
            skillUI[i].GetComponent<EventTrigger>().triggers.Add(exitEvent);
          
            if (i < skillCoolTimeUI.Length)
            {
                skillCoolTimeUI[i] = GameObject.Find("Skill" + (i + 1) + "_CoolTime").GetComponent<Image>();
                partyClassIcon[i] = GameObject.Find("ClassIcon"+(i + 1)).GetComponent<Image>();
                partyGenderIcon[i] = GameObject.Find("GenderIcon"+(i + 1)).GetComponent<Image>();
            }
        }
        enterEvent[0].callback.AddListener((data) => { PointEnter(0); });
        enterEvent[1].callback.AddListener((data) => { PointEnter(1); });
        enterEvent[2].callback.AddListener((data) => { PointEnter(2); });
        enterEvent[3].callback.AddListener((data) => { PointEnter(3); });
        enterEvent[4].callback.AddListener((data) => { PointEnter(4); });
        enterEvent[5].callback.AddListener((data) => { PointEnter(5); });
        resultCheckBtn.onClick.AddListener(() => InGameExit());
     
    }

    public void resultOpen()
    {
        resultImage.gameObject.SetActive(true);
        resultText.text = "2";
    }

    public void InGameExit()
    {
        SceneChanger.Instance.SceneChange(SceneChanger.SceneName.WaitingScene, true);
    }

    public void SetPointEnterUI(int skillIndex, int skillLevel, int classIndex)
    {
        SkillBasicData skillData = SkillManager.Instance.SkillData.GetSkill(classIndex, skillIndex + 1);
        if (!mouseOverUI.IsActive())
        {
            mouseOverUI.gameObject.transform.parent.gameObject.SetActive(true);
        } else
        {
            skillData = null;
            skillData = SkillManager.Instance.SkillData.GetSkill(classIndex, skillIndex + 1);
        }
       mouseOverUI.transform.parent.transform.localPosition = new Vector2(skillUI[skillIndex].transform.localPosition.x + mouseOverUI_xPos, mouseOverUI_yPos);
       mouseOverUI.text = "스킬이름: " + skillData.SkillName + "  " + "쿨타임: " + skillData.SkillCoolTime.ToString() + "초" + "\n" + skillData.SkillBasicExplanation +"\n"+ skillData.GetSkillData(skillLevel).SkillExplanation;
    }

    #region 스킬쿨타임 제어 코루틴
    public IEnumerator SetSkillCoolTimeUI(int skillNum, float coolTime)
    {
        skillCoolTimeUI[skillNum].color += new Color(0, 0, 0, 1);

        float time = Time.smoothDeltaTime;

        while (skillCoolTimeUI[skillNum].fillAmount != 0.0f)
        {
            skillCoolTimeUI[skillNum].fillAmount -= 1 * time / coolTime;
            yield return null;

        }
        skillCoolTimeUI[skillNum].fillAmount = 1;
        skillCoolTimeUI[skillNum].color -= new Color(0, 0, 0, 1);
        time = 0;

        yield break;
    }
    #endregion

    #region 물약쿨타임제어
    public IEnumerator PotionCoolTimeUI()
    {
        potionCoolTimeUI.color += new Color(0, 0, 0, 1);
        float potionCoolTime = 15.0f;
        float time = Time.smoothDeltaTime;
        //    potionCoolTimeUI.gameObject.SetActive(true);
        potionCoolTimeUI.fillAmount = 1;
        while (potionCoolTimeUI.fillAmount != 0.0f)
        {
            potionCoolTimeUI.fillAmount -= 1 * time / potionCoolTime;
            yield return null;
        }
        time = 0;
        potionCoolTimeUI.color -= new Color(0, 0, 0, 0);
        yield break;
    }
    #endregion

    public void hpBarCalculation(float maxHp, float currentHP)
    {
        hpBar.fillAmount = currentHP / maxHp;
        print(hpBar.fillAmount);
    }

    public void mpBarCalculation(int maxMp, int currentMP)
    {
        mpBar.fillAmount = (float)currentMP / maxMp;
    }

    public void monsterHpBarCalculation(string name, float maxHp, float currentHP)
    {
        if (!monsterHpBar.transform.parent.gameObject.activeSelf)
        {
            monsterHpBar.transform.parent.gameObject.SetActive(true);
        }
        monsterName.text = name;
        monsterHpBar.fillAmount = currentHP / maxHp;
    }

    public void ComboProcess(int count)
    {
        if (!comboText.transform.parent.gameObject.activeSelf)
        {
            comboText.transform.parent.gameObject.SetActive(true);
        }
        ComboAction(Camera.main.transform);
        comboText.text = count.ToString();
        comboAnim.Play("ComboAnim", -1, 0);
    }

    public void ComboAction(Transform mCamera)
    {
        mCamera.gameObject.transform.position = new Vector3(Random.Range(mCamera.position.x - 0.09f, mCamera.position.x + 0.09f), Random.Range(mCamera.position.y - 0.09f, mCamera.position.y + 0.09f), mCamera.position.z);
    }

    public void ComboEnd()
    {
        comboText.transform.parent.gameObject.SetActive(false);
        comboText.text = "";
    }

    public void PointEnter(int skillIndex)
    {
        SetPointEnterUI(skillIndex, 2, (int)GameManager.Instance.CharacterStatus.HClass);
    }

    public void OnPointExit()
    {
        MouseOverUI.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public IEnumerator StartProcess()
    {
        yield return new WaitForSeconds(2.0f);
        startImage.gameObject.SetActive(false);
        readyImage.gameObject.SetActive(false);
    }
}
