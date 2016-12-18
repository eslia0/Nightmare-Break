using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginUIManager : MonoBehaviour
{
    GameObject loginPanel;
    GameObject createAccountPanel;
    GameObject deleteAccountPanel;
    GameObject logoObject;
    GameObject[] loginObject;

    Button loginButton;
    Button createAccountButton;
    Button deleteAccountButton;
    Button createAccountYesButton;
    Button createAccountNoButton;
    Button deleteAccountYesButton;
    Button deleteAccountNoButton;
    Button exitButton;
    
    Text loginId;
    Text loginPw;
    Text createId;
    Text createPw;
    Text deleteId;
    Text deletePw;

    InputField loginPwField;
    InputField accountPwField;
    InputField deletePwField;

    bool isInput;

    public void ManagerInitialize()
    {
        SetUIObject();
        InitializeAddListener();

        createAccountPanel.SetActive(false);
    }

    public void SetUIObject()
    {
        loginPanel = GameObject.Find("LoginPanel");
        createAccountPanel = GameObject.Find("CreateAccountPanel");
        loginPwField = GameObject.Find("PassWordInputField").GetComponent<InputField>();
        accountPwField = GameObject.Find("CreatePwInputField").GetComponent<InputField>();

        loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        createAccountButton = GameObject.Find("CreateAccountButton").GetComponent<Button>();
        createAccountYesButton = GameObject.Find("CreateAccountYesButton").GetComponent<Button>();
        createAccountNoButton = GameObject.Find("CreateAccountNoButton").GetComponent<Button>();
        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();

        //deleteAccountButton = GameObject.Find("DeleteAccountButton").GetComponent<Button>();

        loginId = GameObject.Find("LoginId").GetComponent<Text>();
        loginPw = GameObject.Find("LoginPw").GetComponent<Text>();
        createId = GameObject.Find("CreateId").GetComponent<Text>();
        createPw = GameObject.Find("CreatePw").GetComponent<Text>();

        //deleteId = GameObject.Find("LoginId").GetComponent<Text>();
        //deletePw = GameObject.Find("LoginPw").GetComponent<Text>();

        logoObject = GameObject.Find("LogoObject");
        loginObject = new GameObject[2];

        for (int i = 0; i < loginObject.Length; i++)
        {
           loginObject[i] = GameObject.Find("LoginObject" + (i + 1));
           loginObject[i].SetActive(false);
        }
        StartCoroutine(LogoInputCheck());
    }

    public void InitializeAddListener()
    {
        loginButton.onClick.AddListener(() => OnClickLoginButton());
        createAccountButton.onClick.AddListener(() => OnClickCreateAccountButton());
        createAccountYesButton.onClick.AddListener(() => OnClickCreateAccountYesButton());
        createAccountNoButton.onClick.AddListener(() => OnClickCreateAccountNoButton());
        exitButton.onClick.AddListener(() => OnClickExitButton());
        //deleteAccountButton.onClick.AddListener(() => OnClickDeleteAccountButton());

        //createCharacterButton.onClick.AddListener(() => OnClickCreateCharacterButton());
        //deleteCharacterButton.onClick.AddListener(() => OnClickDeleteCharacterButton());
        //selectCharacterButton.onClick.AddListener(() => OnClickSelectCharacterButton());
    }

    public void OnClickCreateAccountButton()
    {
        loginPanel.SetActive(false);
        createAccountPanel.SetActive(true);
    }

    public void OnClickCreateAccountYesButton()
    {
        if (createId.text.Length >= 4 && accountPwField.text.Length >= 6)
        {
            DataSender.Instance.CreateAccount(createId.text, accountPwField.text);
        }
        else
        {
            Debug.Log("아이디 4글자 이상, 비밀번호 6글자 이상 입력하세요");
        }
    }

    public void OnClickCreateAccountNoButton()
    {
        createId.text = "";
        createPw.text = "";

        createAccountPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void CreateAccountSuccess()
    {
        createId.text = "";
        createPw.text = "";

        createAccountPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void OnClickDeleteAccountButton()
    {
        if (deleteId.text.Length >= 4 && deletePwField.text.Length >= 6)
        {
            DataSender.Instance.DeleteAccount(deleteId.text, deletePwField.text);
        }
        else
        {
            Debug.Log("아이디 4글자 이상, 비밀번호 6글자 이상 입력하세요");
        }
    }

    public void OnClickLoginButton()
    {
        if (loginId.text.Length >= 4 && loginPwField.text.Length >= 6)
        {
            DataSender.Instance.Login(loginId.text, loginPwField.text);
        }
        else
        {
            Debug.Log("아이디 4글자 이상, 비밀번호 6글자 이상 입력하세요");
        }
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    IEnumerator LogoInputCheck()
    {
        while (!isInput)
        {
            if (Input.anyKeyDown)
            {
                isInput = true;
                logoObject.SetActive(false);
                for (int i = 0; i < loginObject.Length; i++)
                {
                    loginObject[i].SetActive(true);
                }
            }
            yield return null;
        }
    }
}