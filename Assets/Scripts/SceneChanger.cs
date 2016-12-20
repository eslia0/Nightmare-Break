using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public enum SceneName
    {
        TitleScene,
        LoadingScene,
        SelectScene,
        CreateScene,
        WaitingScene,
        RoomScene,
        TeddyBearStage1,
        TeddyBearStage2,
        TeddyBearBoss,
    }

    public enum SelectLoadingData
    {
        CharacterList = 0,
    }

    private const float fadeValue = 1.5f;
    private float fadeTime;
    private Image fadePanel;
    private LoadingSceneUI loadingScene;
    private SceneName currentScene;
    private SceneName nextScene;

    private bool[] loadingCheck;

    public bool[] LoadingCheck { get { return loadingCheck; } set { loadingCheck = value; } }
    public SceneName CurrentScene { get { return currentScene; } }
    public SceneName NextScene { get { return nextScene; } }

    private static SceneChanger instance = null;
    public static SceneChanger Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        #region 타이틀 씬 로드
        if (scene.name == "TitleScene")
        {
            if (currentScene == SceneName.LoadingScene)
            {
                UIManager.Instance.SetUIManager(UIManagerIndex.Login);
                UIManager.Instance.LoginUIManager.ManagerInitialize();
            }

            currentScene = SceneName.TitleScene;            
        }
        #endregion

        #region 로딩 씬 로드
        else if (scene.name == "LoadingScene")
        {
            if (nextScene == SceneName.SelectScene)
            {
                if (currentScene == SceneName.WaitingScene)
                {
                    GameManager.Instance.DestroyManagerInWait();
                }

                UIManager.Instance.SetUIManager(UIManagerIndex.Select);
                DataSender.Instance.RequestCharacterList();

                StartCoroutine(CheckLoading(1));
            }
            else if (nextScene == (int)SceneName.TitleScene)
            {
                DataSender.Instance.Logout();
                Destroy(GameManager.Instance.gameObject);
                SceneManager.LoadScene((int)nextScene);
            }
            else if (nextScene == SceneName.WaitingScene)
            {
                if (currentScene == SceneName.TeddyBearBoss)
                {
                    GameManager.Instance.DestroyManagerInGame();
                }
                else
                {
                    GameManager.Instance.SetManagerInWait();
                }

                UIManager.Instance.SetUIManager(UIManagerIndex.Waiting);
                DataSender.Instance.RequestCharacterStatus();
                DataSender.Instance.RequestRoomList();

                StartCoroutine(CheckLoading(2));
            }
            else if (nextScene == SceneName.TeddyBearStage1)
            {
                //매니저 생성
                GameManager.Instance.SetManagerInGame();

                DungeonManager.Instance.ManagerInitialize(UIManager.Instance.RoomUIManager.DungeonId, UIManager.Instance.RoomUIManager.DungeonLevel);
                UIManager.Instance.SetUIManager(UIManagerIndex.InGame);

                DataSender.Instance.RequestUdpConnection();
            }

            currentScene = SceneName.LoadingScene;
        }
        #endregion

        #region 캐릭터 선택 씬 로드
        else if (scene.name == "SelectScene")
        {
            if (currentScene == SceneName.LoadingScene)
            {
                UIManager.Instance.SelectUIManager.ManagerInitialize();
                UIManager.Instance.SelectUIManager.SetCharacter();
                StartCoroutine(FadeIn());
            }
            else if (currentScene == SceneName.CreateScene)
            {
                UIManager.Instance.SetUIManager(UIManagerIndex.Select);
                DataSender.Instance.RequestCharacterList();
                UIManager.Instance.SelectUIManager.ManagerInitialize();
            }

            currentScene = SceneName.SelectScene;
        }
        #endregion

        #region 캐릭터 생성 씬 로드
        else if (scene.name == "CreateScene")
        {
            currentScene = SceneName.CreateScene;

            UIManager.Instance.SetUIManager(UIManagerIndex.Create);
            UIManager.Instance.CreateUIManager.ManagerInitialize();
        }
        #endregion

        #region 대기 씬 로드
        else if (scene.name == "WaitScene")
        {
            if(currentScene == SceneName.LoadingScene || currentScene == SceneName.TeddyBearBoss)
            {
                UIManager.Instance.WaitingUIManager.ManagerInitialize();
                UIManager.Instance.WaitingUIManager.SetRoom();
            }
            else if(currentScene == SceneName.RoomScene)
            {
                UIManager.Instance.SetUIManager(UIManagerIndex.Waiting);
                UIManager.Instance.WaitingUIManager.ManagerInitialize();
                DataSender.Instance.RequestRoomList();
            }

            currentScene = SceneName.WaitingScene;
        }
        #endregion

        #region 방 씬 로드
        else if (scene.name == "RoomScene")
        {
            UIManager.Instance.SetUIManager(UIManagerIndex.Room);
            UIManager.Instance.RoomUIManager.ManagerInitialize();
            DataSender.Instance.RequestRoomUserData(UIManager.Instance.WaitingUIManager.CurrentRoomNum);

            currentScene = SceneName.RoomScene;
        }
        #endregion

        #region 던전 1씬 로드
        else if (scene.name == "LostTeddyBear_Stage1")
        {
            UIManager.Instance.BattleUIManager.ManagerInitialize();

            DungeonManager.Instance.SetCurrentStageNum(0);
            DungeonManager.Instance.IsNormal = true;
            DungeonManager.Instance.StartDungeon();
            ReSendManager.Instance.characterCreating = true;

            currentScene = SceneName.TeddyBearStage1;
        }
        #endregion

        #region 던전 2씬 로드
        else if (scene.name == "LostTeddyBear_Stage2")
        {
            UIManager.Instance.BattleUIManager.ManagerInitialize();

            DungeonManager.Instance.SetCurrentStageNum(1);
            DungeonManager.Instance.IsNormal = false;
            DungeonManager.Instance.StartDungeon();
            ReSendManager.Instance.characterCreating = true;

            currentScene = SceneName.TeddyBearStage2;
        }
        #endregion

        #region 보스 씬 로드
        else if (scene.name == "LostTeddyBear_Boss")
        {
            UIManager.Instance.BattleUIManager.ManagerInitialize();

            DungeonManager.Instance.SetCurrentStageNum(2);
            DungeonManager.Instance.IsNormal = true;
            DungeonManager.Instance.StartDungeon();
            ReSendManager.Instance.characterCreating = true;

            currentScene = SceneName.TeddyBearBoss;
        }
        #endregion

    }

    public void SceneChange(SceneName sceneName, bool needLoadingScene)
    {
        nextScene = sceneName;

        if (needLoadingScene)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            SceneManager.LoadScene((int)sceneName);
        }
    }

    private IEnumerator FadeOut()
    {
        GameObject fadeCanvas = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas"));
        fadePanel = fadeCanvas.transform.GetChild(0).GetComponent<Image>();
        fadeTime = Time.deltaTime;
        while (fadePanel.color.a < 1)
        {
            fadePanel.color += new Color(0, 0, 0, (float)fadeValue * fadeTime);
            yield return null;
        }
        fadeTime = 0;
        fadePanel = null;
        SceneManager.LoadScene((int)SceneName.LoadingScene);
    }

    private IEnumerator FadeIn()
    {
        GameObject fadeCanvas = Instantiate(Resources.Load<GameObject>("UI/FadeCanvas"));
        fadePanel = fadeCanvas.transform.GetChild(0).GetComponent<Image>();
        fadeTime = Time.deltaTime;
        fadePanel.color = new Color(0, 0, 0, 1);
        while (fadePanel.color.a > 0)
        {
            fadePanel.color -= new Color(0, 0, 0, (float)fadeValue * fadeTime);
            yield return null;
        }
        fadePanel = null;
        Destroy(fadeCanvas);
        fadeTime = 0;
    }

    public IEnumerator CheckLoading(int checkSize)
    {
        loadingCheck = new bool[checkSize];

        if (nextScene == SceneName.TeddyBearStage1)
        {
            LoadingCheck[NetworkManager.Instance.MyIndex + 2] = true;
        }

        bool checkComplete = false;
        Debug.Log("로딩 체크 시작 " + checkSize);

        while (!checkComplete)
        {
            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < loadingCheck.Length; i++)
            {
                if (loadingCheck[i])
                {
                    checkComplete = true;
                }
                else
                {
                    checkComplete = false;
                    break;
                }
            }
        }

        if (nextScene == SceneName.TeddyBearStage1)
        {
            DataSender.Instance.LoadingComplete();
        }
        else
        {
            SceneManager.LoadScene((int)nextScene);
        }        
    }
}
