using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalLogin : UI_Menu_Canvas
{
    private MenuSceneMain menuSceneMain;

    private InputField loginInput;
    private InputField passwordInput;
    private Text message;
    private Button singIn_Button;
    private Button createNewPlayer_Button;
    private Button resetData_Button;

    public LocalLogin (MenuSceneMain menuSceneMain)
    {
        this.menuSceneMain = menuSceneMain;
        go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/LoginPanel/LocalLogin_Canvas", typeof(GameObject))) as GameObject;

        this.loginInput = go.transform.Find("Panel").transform.Find("Login_InputField").GetComponent<InputField>();
        this.passwordInput = go.transform.Find("Panel").transform.Find("Password_InputField").GetComponent<InputField>();
        this.message = go.transform.Find("Panel").transform.Find("Message_Text").GetComponent<Text>();

        this.singIn_Button = go.transform.Find("Panel").transform.Find("SingIn_Button").GetComponent<Button>();
        this.singIn_Button.onClick.AddListener(SingIn);

        this.createNewPlayer_Button = go.transform.Find("Panel").transform.Find("CreatePlayer_Button").GetComponent<Button>();
        this.createNewPlayer_Button.onClick.AddListener(Create_Player);

        this.resetData_Button = go.transform.Find("Panel").transform.Find("ResetLocalData_Button").GetComponent<Button>();
        this.resetData_Button.onClick.AddListener(ResetLocalData);

        Show_AwailableAccounts();
    }

    private void Create_Player()
    {
        if(!Validate_Login(loginInput.text) || !Validate_Password(passwordInput.text)) return;

        Account acc = new Account();
        acc.name = loginInput.text;
        acc.password = passwordInput.text;

        acc.сharacters.Add(new Swordman());
        acc.сharacters.Add(new Swordman());
        acc.сharacters.Add(new Swordman());
        acc.сharacters.Add(new Spearman());
        acc.сharacters.Add(new Spearman());
        acc.сharacters.Add(new Knight());

        acc.acc_gold = 50;

        LocalData localData = new LocalData();
        localData.Save_PlayerData(acc);

        Add_AwailableAccounts(acc);
        message.text = "New local account created : " + acc.name;
    }

    private void SingIn()
    {
        if (!SingIn_Check()) return;
        Hide();

        menuSceneMain.SingIn();
    }

    private bool SingIn_Check()
    {
        string accounts = PlayerPrefs.GetString("LocalAccounts");
        string[] accountsData = accounts.Split(';');
        for(int x = 0; x < accountsData.Length; x++)
        {
            string[] accountData = accountsData[x].Split(':');
            if(accountData[0] != loginInput.text) continue;
            if(accountData[1] != passwordInput.text) continue;

            LocalData localData = new LocalData();
            GameData.inst.account = localData.Load_PlayerData(accountData[0]);
            return true;
        }

        return false;
    }

    private void ResetLocalData()
    {
        PlayerPrefs.DeleteAll();
        Show_AwailableAccounts();
    }

    public void Show_AwailableAccounts()
    {
        message.text = "";
        string accounts = PlayerPrefs.GetString("LocalAccounts");
        if (accounts == "")
        {
            message.text = "No accounts yet...";
        }
        else
        {
            string[] accountsData = accounts.Split(';');
            message.text = "Local accounts : ";
            for (int x = 0; x < accountsData.Length; x++)
            {
                string[] accountData = accountsData[x].Split(':');
                message.text += accountData[0] + " ";
            }
        }
    }

    private void Add_AwailableAccounts(Account acc)
    {
        string accounts = PlayerPrefs.GetString("LocalAccounts");
        
        accounts += acc.name + ":" + acc.password + ";";
        if(accounts != "") accounts = accounts.Remove(accounts.Length - 1);

        PlayerPrefs.SetString("LocalAccounts", accounts);
        PlayerPrefs.Save();
    }

    private bool Validate_Login(string login)
    {
        if(login == "") return false;

        string accounts = PlayerPrefs.GetString("LocalAccounts");
        string[] accountsData = accounts.Split(';');
        for(int x = 0; x < accountsData.Length; x++)
        {
            string[] accountData = accountsData[x].Split(':');
            if(accountData[0] == login) return false;
        }

        return true;
    }

    private bool Validate_Password(string password)
    {
        if(password == "") return false;
        return true;
    }
}
