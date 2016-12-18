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
        InGameScene,
		DefenseScene,
    }

    public enum SelectLoadingData
    {
        CharacterList = 0,
    }

    private const float fadeValue = 0.8f;
    private float fadeTime;
    private Image fadePanel;
    private LoadingSceneUI loadingScene;
    private SceneName currentScene;
    private int nextScene;

    private bool[] loadingCheck;

    public bool[] LoadingCheck { get { return loadingCheck; } set { loadingCheck = value; } }
    public SceneName CurrentScene { get { return currentScene; } }

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
            if (nextScene == (int)SceneName.SelectScene)
            {
                if (currentScene == SceneName.WaitingScene)
                {
                    GameManager.Instance.DestroyManagerInWait();
                }

                UIManager.Instance.SetUIManager(UIManagerIndex.Select);
                DataSender.Instance.RequestCharacterList();

                StartCoroutine(CheckLoading());
            }
            else if (nextScene == (int)SceneName.TitleScene)
            {
                DataSender.Instance.Logout();
                Destroy(GameManager.Instance.gameObject);
                SceneManager.LoadScene(nextScene);
            }
            else if (nextScene == (int)SceneName.WaitingScene)
            {
                GameManager.Instance.SetManagerInWait();
                UIManager.Instance.SetUIManager(UIManagerIndex.Waiting);
                DataSender.Instance.RequestCharacterStatus();
                DataSender.Instance.RequestRoomList();

                StartCoroutine(CheckLoading());
            }
            else if (nextScene == (int)SceneName.InGameScene)
            {
                GameManager.Instance.SetManagerInGame();

                DungeonManager.Instance.ManagerInitialize(UIManager.Instance.RoomUIManager.DungeonId, UIManager.Instance.RoomUIManager.DungeonLevel, UIManager.Instance.RoomUIManager.UserNum);
                UIManager.Instance.SetUIManager(UIManagerIndex.InGame);

                DataSender.Instance.RequestUdpConnection();
                DataSender.Instance.RequestMonsterSpawnList(DungeonManager.Instance.DungeonId, DungeonManager.Instance.DungeonLevel);
                DataSender.Instance.RequestMonsterStatusData(DungeonManager.Instance.DungeonId, DungeonManager.Instance.DungeonLevel);

                StartCoroutine(CheckLoading());
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
            if(currentScene == SceneName.LoadingScene)
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

        #region 던전 씬 로드
        else if (scene.name == "LostTeddyBear_Stage1")
        {
            UIManager.Instance.BattleUIManager.ManagerInitialize();

            DungeonManager.Instance.StartDungeon(NetworkManager.Instance.UserIndex.Count);

            currentScene = SceneName.InGameScene;
        }
        #endregion
        
		#region 연우씨 씬 로드
		else if (scene.name == "")
		{
			currentScene = SceneName.DefenseScene;
			//kyw
		}
		#endregion

    }

    public void SceneChange(SceneName sceneName, bool needLoadingScene)
    {
        nextScene = (int)sceneName;

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

    private IEnumerator CheckLoading()
    {
        if(nextScene == (int)SceneName.SelectScene)
        {
            loadingCheck = new bool[1];
        }
        else if (nextScene == (int)SceneName.WaitingScene)
        {
            loadingCheck = new bool[2];
        }
        else if (nextScene == (int)SceneName.InGameScene)
        {
            LoadingCheck = new bool[4];
        }

        bool checkComplete = false;

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

        SceneManager.LoadScene(nextScene);
    }
}
