using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalLogin : UI_Menu_Canvas
{
    private MenuSceneMain menuSceneMain;

    private InputField loginInput;
    private InputField passwordInput;
    private Toggle rememberToggle;

    private Button singIn_Button;
    private Button createNewPlayer_Button;
    private Button resetData_Button;

    private Text message;

    public LocalLogin (MenuSceneMain menuSceneMain)
    {
        this.menuSceneMain = menuSceneMain;
        go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/LoginPanel/LocalLogin_Canvas", typeof(GameObject))) as GameObject;

        this.loginInput = go.transform.Find("Panel").transform.Find("Login_InputField").GetComponent<InputField>();
        this.passwordInput = go.transform.Find("Panel").transform.Find("Password_InputField").GetComponent<InputField>();

        this.rememberToggle = go.transform.Find("Panel").transform.Find("Remember_Toggle").GetComponent<Toggle>();
        
        this.message = go.transform.Find("Panel").transform.Find("Message_Text").GetComponent<Text>();

        this.singIn_Button = go.transform.Find("Panel").transform.Find("SingIn_Button").GetComponent<Button>();
        this.singIn_Button.onClick.AddListener(SingIn);

        this.createNewPlayer_Button = go.transform.Find("Panel").transform.Find("CreatePlayer_Button").GetComponent<Button>();
        this.createNewPlayer_Button.onClick.AddListener(Create_Player);

        this.resetData_Button = go.transform.Find("Panel").transform.Find("ResetLocalData_Button").GetComponent<Button>();
        this.resetData_Button.onClick.AddListener(ResetLocalData);

        Show_AwailableAccounts();
        Set_Prefill();
    }

    private void Create_Player()
    {
        if(!Validate_Login(loginInput.text) || !Validate_Password(passwordInput.text)) return;

        Account acc = new Account();
        acc.name = loginInput.text;
        acc.password = passwordInput.text;

        Character testChar = new Swordman();
        int rand = Random.Range(1,4);
        if(rand == 1)
            testChar.lifetimeBuffs.Add(new Strong());
        else if(rand == 2)
            testChar.lifetimeBuffs.Add(new BrokenLeg());
        else
            testChar.lifetimeBuffs.Add(new ChestWound());

        acc.??haracters.Add(testChar);        
        acc.??haracters.Add(new Spearman());

        acc.items.Add(new Gold());
        acc.items.Add(new Token_General());
        acc.items.Add(new PiWolf());

        acc.acc_gold = 45;

        LocalData localData = new LocalData();
        localData.Save_PlayerData(acc);

        Add_AwailableAccounts(acc);
        message.text = "New local account created : " + acc.name;

        SingIn();
    }

    private void SingIn()
    {
        if (!SingIn_Check()) return;
        Save_Prefill(rememberToggle.isOn);
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

    private void Save_Prefill(bool isOn)
    {
        string prefill = "";
        if (isOn) prefill += "on,";
        else prefill += "off,";
        prefill += loginInput.text + "," + passwordInput.text;

        PlayerPrefs.SetString("Prefill", prefill);
        PlayerPrefs.Save();
    }

    private void Set_Prefill()
    {
        string prefill = PlayerPrefs.GetString("Prefill");
        if(prefill == "") 
        {
            rememberToggle.isOn = false;
            return;
        }
        
        string[] prefillData = prefill.Split(',');

        if(prefillData[0] != "on") 
        {
            rememberToggle.isOn = false;
            return;
        }

        rememberToggle.isOn = true;
        loginInput.text = prefillData[1];
        passwordInput.text = prefillData[2];
    }

    private bool Validate_Password(string password)
    {
        if(password == "") return false;
        return true;
    }

    private void ResetLocalData()
    {
        GameData.inst.account = null;
        PlayerPrefs.DeleteAll();
        Show_AwailableAccounts();
    }

}
