﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum UIManagerIndex
{
    Login = 0,
    Select,
    Create,
    Waiting,
    Room,
    InGame,
}

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(UIManager)) as UIManager;

                if (!instance)
                {
                    GameObject container = new GameObject();
                    container.name = "UIManager";
                    container.tag = "UIManager";
                    instance = container.AddComponent(typeof(UIManager)) as UIManager;
                }
            }

            return instance;
        }
    }

    CharacterManager charManager;
    LoginUIManager loginUIManager;
    SelectUIManager selectUIManager;
    CreateUIManager createUIManager;
    WaitingUIManager waitingUIManager;
    RoomUIManager roomUIManager;
    BattleUIManager battleUIManager;

    public LoginUIManager LoginUIManager { get { return loginUIManager; } }
    public SelectUIManager SelectUIManager { get { return selectUIManager; } }
    public CreateUIManager CreateUIManager { get { return createUIManager; } }
    public WaitingUIManager WaitingUIManager { get { return waitingUIManager; } }
    public RoomUIManager RoomUIManager { get { return roomUIManager; } }
    public BattleUIManager BattleUIManager { get { return battleUIManager; } }

    public GameObject dialogPanel;
    public Text dialog;

    public void InitializeUIManager()
    {
        if (loginUIManager != null) Destroy(loginUIManager);
        if (selectUIManager != null) Destroy(selectUIManager);
        if (createUIManager != null) Destroy(createUIManager);
        if (waitingUIManager != null) Destroy(waitingUIManager);
        if (roomUIManager != null) Destroy(roomUIManager);
        if (battleUIManager != null) Destroy(battleUIManager);
    }

    public void SetUIManager(UIManagerIndex index)
    {
        InitializeUIManager();

        switch (index)
        {
            case UIManagerIndex.Login:
                loginUIManager = gameObject.AddComponent<LoginUIManager>();
                break;

            case UIManagerIndex.Select:
                selectUIManager = gameObject.AddComponent<SelectUIManager>();
                break;

            case UIManagerIndex.Create:
                createUIManager = gameObject.AddComponent<CreateUIManager>();
                break;

            case UIManagerIndex.Waiting:
                waitingUIManager = gameObject.AddComponent<WaitingUIManager>();
                break;

            case UIManagerIndex.Room:
                roomUIManager = gameObject.AddComponent<RoomUIManager>();
                break;

            case UIManagerIndex.InGame:
                battleUIManager = gameObject.AddComponent<BattleUIManager>();
                break;
        }
    }

    public void SetBattleUIManager()
    {
        InitializeUIManager();

        charManager = GameObject.FindWithTag("Player").GetComponent<CharacterManager>();
        battleUIManager = GetComponent<BattleUIManager>();
        battleUIManager.SetUIObject();
    }

    public void SetDialog()
    {
        try
        {
            dialogPanel = GameObject.Find("DialogPanel");
            dialog = dialogPanel.transform.FindChild("Dialog").GetComponent<Text>();
            dialogPanel.SetActive(false);
        }
        catch
        {
            Debug.Log("Dialog Error");
        }
    }

    public IEnumerator Dialog(float delay, string text)
    {
        //dialogPanel.SetActive(true);

        //dialog.text = text;

        yield return new WaitForSeconds(delay);

        //dialog.text = "";

        //dialogPanel.SetActive(false);
    }
		
}

    

    
