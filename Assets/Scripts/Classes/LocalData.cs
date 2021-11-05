using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Used to save player local account data
public class LocalData
{
    private CharactersData cd;
    
    public LocalData()
    {
        cd = new CharactersData();
    }

    public void Save_PlayerData(Account acc)
    {
        AccountData accountData = new AccountData();
        accountData.name = acc.name;
        accountData.data = acc.Get_Acc_Data();
        accountData.heroes = acc.Get_Acc_Heroes_Data();
        accountData.characters = acc.Get_Acc_CharactersData();
        accountData.items = acc.Get_Acc_ItemsData();

        string jsonData = JsonUtility.ToJson(accountData);
        PlayerPrefs.SetString(acc.name, jsonData);
        PlayerPrefs.Save();
        // Debug.Log("Player data is saved : " + acc.name);
    }

    public Account Load_PlayerData(string accName)
    {
        // Debug.Log("Loading data for " + accName);
        string jsonData = PlayerPrefs.GetString(accName);
        AccountData accountData = JsonUtility.FromJson<AccountData>(jsonData);

        Account acc = new Account();
        acc.name = accountData.name;
        acc.Set_Acc_Data(accountData.data);
        acc.Set_Acc_Heroes_Data(accountData.heroes);
        acc.Set_Acc_CharactersData(accountData.characters);
        acc.Set_Acc_ItemsData(accountData.items);

        return acc;
    }
}

[System.Serializable]
public class AccountData
{
    public string name;
    public string data;
    public string heroes; // heroName:characterId - hName:1; hName:2; hName:3; ...
    public string characters; // characterIdList - 1,2,3,4 ...
    public string items; // playerItemIdList - 1,2,3,4 ...
}