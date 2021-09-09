using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Used to save player local account data
public class LocalData
{
    public void Save_PlayerData(Account acc)
    {
        AccountData account = new AccountData();
        account.name = acc.name;
        account.password = acc.password;
        account.battleHeroId = acc.battleHeroId;

        account.pHeroes = GetHeroesData(acc);
        account.phCharacters = GetHeroesCharactersData(acc);
        account.pCharacters = GetPlayerCharactersData(acc);
        account.pItems = GetPlayerItemsData(acc);

        account.acc_gold = acc.acc_gold;

        string jsonData = JsonUtility.ToJson(account);
        PlayerPrefs.SetString(account.name, jsonData);
        PlayerPrefs.Save();
        
        Debug.Log("Player data is saved : " + account.name);
    }

    private string GetHeroesData(Account acc)
    {
        string result = "";
        for(int x = 0; x < acc.heroes.Count; x++)
        {
            Hero hero = acc.heroes[x];
            result += hero.name + ":" + hero.character.cId + ";";
        }
        if(result != "") result = result.Remove(result.Length - 1);
        // Debug.Log("Heroes : " + result);
        return result;
    }

    private string GetHeroesCharactersData(Account acc)
    {
        string result = "";

        if(acc.heroes.Count == 0) return result;
        
        for(int x = 0; x < acc.heroes.Count; x++)
        {
            Hero hero = acc.heroes[x];
            if(hero.battleCharacters.Count == 0) continue;

            result += x + ":";
            for(int y = 0; y < hero.battleCharacters.Count; y++)
            {
                result += hero.battleCharacters[y].cId + ",";
            }
            result = result.Remove(result.Length - 1);
            result += ";";
        }
        
        if(result != "") result = result.Remove(result.Length - 1);
        // Debug.Log("Heroes characters : " + result);
        return result;
    }

    private string GetPlayerCharactersData(Account acc)
    {
        string result = "";

        if(acc.сharacters.Count == 0) return result;

        for(int x = 0; x < acc.сharacters.Count; x++)
        {
            result += acc.сharacters[x].cId + ",";
        }

        if(result != "") result = result.Remove(result.Length - 1);
        // Debug.Log("Player characters : " + result);
        return result;
    }

    private string GetPlayerItemsData(Account acc)
    {
        string result = "";

        if(acc.items.Count == 0) return result;
        
        for(int x = 0; x < acc.items.Count; x++)
        {
            result += acc.items[x].id + ",";
        }
        if(result != "") result = result.Remove(result.Length - 1);
        // Debug.Log("Player items : " + result);
        return result;
    }

    public Account Load_PlayerData(string accName)
    {
        Debug.Log("Loading data for " + accName);
        string jsonData = PlayerPrefs.GetString(accName);
        AccountData account = JsonUtility.FromJson<AccountData>(jsonData);
        
        Account acc = new Account(account.name);
        acc.password = account.password;
        acc.battleHeroId = account.battleHeroId;
        acc.acc_gold = account.acc_gold;

        if(account.pHeroes != "")
        {
            string[] pHeroesData = account.pHeroes.Split(';');
            for(int x = 0; x < pHeroesData.Length; x++)
            {
                string[] hData = pHeroesData[x].Split(':');
                
                Character heroCharacter = Utility.Get_Character_ById(Convert.ToInt32(hData[1]));
                Hero someHero = new Hero(heroCharacter, hData[0]);

                heroCharacter.cHero = someHero;
                acc.heroes.Add(someHero);
            }
        }

        if(account.phCharacters != "")
        {
            string[] pHeroesCharactersData = account.phCharacters.Split(';');
            for(int x = 0; x < pHeroesCharactersData.Length; x++)
            {
                string[] hData = pHeroesCharactersData[x].Split(':');
                string[] cData = hData[1].Split(',');
                for(int y = 0; y < cData.Length; y++)
                {
                    Hero h = acc.heroes[Convert.ToInt32(hData[0])];
                    Character someCharacter = Utility.Get_Character_ById(Convert.ToInt32(cData[y]));
                    someCharacter.cHero = h;

                    h.battleCharacters.Add(someCharacter);
                }
            }
        }

        if(account.pCharacters != "")
        {
            string[] pCharactersData = account.pCharacters.Split(',');
            for(int x = 0; x < pCharactersData.Length; x++)
            {
                Character someCharacter = Utility.Get_Character_ById(Convert.ToInt32(pCharactersData[x]));
                
                acc.сharacters.Add(someCharacter);
            }
        }

        if(account.pItems != "")
        {
            string[] pItemsData = account.pItems.Split(',');
            for(int x = 0; x < pItemsData.Length; x++)
            {
                PlayerItem pi = Utility.Get_PlayerItem_ById(Convert.ToInt32(pItemsData[x]));
                acc.items.Add(pi);
            }
        }

        return acc;
    }
}

[System.Serializable]
public class AccountData
{
    public string name;
    public string password;
    public int battleHeroId;

    public string pHeroes; // heroName:characterId - hName:1; hName:2; hName:3; ...
    public string phCharacters; // heroListId,characterId - 0:1,1; 1:2,1; 2:1,10 ...
    public string pCharacters; // characterIdList - 1,2,3,4 ...
    public string pItems; // playerItemIdList - 1,2,3,4 ...

    public int acc_gold;
}