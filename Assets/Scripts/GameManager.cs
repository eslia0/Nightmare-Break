using UnityEngine;
using System.Net;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!instance)
                {
                    GameObject container = new GameObject();
                    container.name = "GameManager";
                    container.tag = "GameManager";
                    instance = container.AddComponent(typeof(GameManager)) as GameManager;
                }
            }

            return instance;
        }
    }

    DungeonManager dungeonManager;
    NetworkManager networkManager;
    InputManager inputManager;
    UIManager uiManager;
    CharacterStatus characterStatus;
    SkillManager skillManager;
    
    string myIP;
    public string MyIP
    {
        get
        {
            if(myIP == null)
            {
                for (int addressIndex = 0; addressIndex < Dns.GetHostAddresses(Dns.GetHostName()).Length; addressIndex++)
                {
                    if (Dns.GetHostAddresses(Dns.GetHostName())[addressIndex].ToString().Length == 13)
                    {
                        myIP = Dns.GetHostAddresses(Dns.GetHostName())[addressIndex].ToString();
                    }
                }
            }
            
            return myIP;
        }
    }
    public CharacterStatus CharacterStatus { get { return characterStatus; } }

    void Start()
    {
        InitializeManager();
        Application.runInBackground = true;
    }

    void Update()
    {

    }

    void InitializeManager()
    {
        name = "GameManager";
        tag = "GameManager";
        
        if (GameObject.FindWithTag("UIManager") == null)
        {
            uiManager = (Instantiate(Resources.Load("Manager/UIManager")) as GameObject).GetComponent<UIManager>();
            uiManager.name = "UIManager";
            uiManager.tag = "UIManager";
            //uiManager.SetDialog();

            uiManager.SetUIManager(UIManagerIndex.Login);
            uiManager.LoginUIManager.ManagerInitialize();
            DontDestroyOnLoad(uiManager.gameObject);
        }
        else
        {
            uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        }

        if (GameObject.FindWithTag("NetworkManager") == null)
        {
            networkManager = (Instantiate(Resources.Load("Manager/NetworkManager")) as GameObject).GetComponent<NetworkManager>();
            networkManager.name = "NetworkManager";
            networkManager.tag = "NetworkManager";

            networkManager.InitializeManager();
            DontDestroyOnLoad(networkManager.gameObject);
        }
        else
        {
            networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetManagerInWait()
    {
        characterStatus = (Instantiate(Resources.Load("Manager/CharacterStatus")) as GameObject).GetComponent<CharacterStatus>();
        characterStatus.name = "CharacterStatus";
        characterStatus.tag = "CharacterStatus";

        SkillManager skillManager = (Instantiate(Resources.Load("Manager/SkillManager")) as GameObject).GetComponent<SkillManager>();
        skillManager.name = "SkillManager";
        DontDestroyOnLoad(skillManager);

        DontDestroyOnLoad(characterStatus);
    }

    public void DestroyManagerInWait()
    {
        Destroy(characterStatus.gameObject);
    }

    public void SetManagerInGame()
    {
        dungeonManager = (Instantiate(Resources.Load("Manager/DungeonManager")) as GameObject).GetComponent<DungeonManager>();
        dungeonManager.name = "DungeonManager";
        dungeonManager.tag = "DungeonManager";
        DontDestroyOnLoad(dungeonManager);

        inputManager = (Instantiate(Resources.Load("Manager/InputManager")) as GameObject).GetComponent<InputManager>();
        inputManager.name = "InputManager";
        inputManager.tag = "InputManager";
        DontDestroyOnLoad(inputManager);
    }

    public void DestroyManagerInGame()
    {
        Destroy(dungeonManager);
        Destroy(inputManager);
    }

    public void OnApplicationQuit()
    {
        NetworkManager.Instance.DataSender.GameClose();
    }
}

