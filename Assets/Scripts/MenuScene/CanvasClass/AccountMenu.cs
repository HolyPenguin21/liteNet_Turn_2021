using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountMenu : UI_Menu_Canvas
{
    public InputField loginInput;
    public InputField passwordInput;
    public Text message;

    public AccountMenu (GameObject gameObject)
    {
        go = gameObject;

        this.loginInput = go.transform.Find("Panel").transform.Find("Login_InputField").GetComponent<InputField>();
        this.passwordInput = go.transform.Find("Panel").transform.Find("Password_InputField").GetComponent<InputField>();
        this.message = go.transform.Find("Panel").transform.Find("Message_Text").GetComponent<Text>();
    }

    public void Create_Player()
    {
        if(!Validate_Login(loginInput.text) || !Validate_Password(passwordInput.text)) return;

        Account player = new Account(loginInput.text);
        player.password = passwordInput.text;

        player.сharacters.Add(new Swordman(null, null, null));
        player.сharacters.Add(new Swordman(null, null, null));
        player.сharacters.Add(new Swordman(null, null, null));
        player.сharacters.Add(new Spearman(null, null, null));
        player.сharacters.Add(new Spearman(null, null, null));
        player.сharacters.Add(new Knight(null, null, null));

        player.acc_gold = 50;

        LocalData localData = new LocalData();
        localData.Save_PlayerData(player);

        Add_IntoPlayerList(player);
    }

    private void Add_IntoPlayerList(Account acc)
    {
        string accounts = PlayerPrefs.GetString("LocalAccounts");
        string[] accountsData = accounts.Split(';');
        if(accounts.Length == 0)
        {
            accounts += acc.name + ":" + acc.password;
        }
        else
        {
            accounts += ";" + acc.name + ":" + acc.password;
        }

        PlayerPrefs.SetString("LocalAccounts", accounts);
        PlayerPrefs.Save();
    }

    public bool SingIn()
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

    public void Awailable_Accounts()
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
