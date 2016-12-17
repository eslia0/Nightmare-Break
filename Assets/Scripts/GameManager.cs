using UnityEngine;
using System.Net;

public class GameManager : MonoBehaviour
{
    DungeonManager dungeonManager;
    NetworkManager networkManager;
    InputManager inputManager;
    UIManager uiManager;
    CharacterStatus characterStatus;
    
    string myIP;    
    public string MyIP
    {
        get
        {
            return myIP;
        }
    }

    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            }

            return instance;
        }
    }

    void Start()
    {
        myIP = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
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
            DontDestroyOnLoad(uiManager);
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
            DontDestroyOnLoad(networkManager);
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
        characterStatus.tag = "CharStatus";
        DontDestroyOnLoad(characterStatus);

        networkManager.DataHandler.SetCharacterStatus();
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

        inputManager = (Instantiate(Resources.Load("Manager/InputManager")) as GameObject).GetComponent<InputManager>();
        inputManager.name = "InputManager";
        inputManager.tag = "InputManager";
    }

    public void OnApplicationQuit()
    {
        networkManager.DataSender.GameClose();
    }
}

