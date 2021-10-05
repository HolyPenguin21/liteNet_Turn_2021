using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;

public class Account
{
    public NetPeer address;

    public bool isServer = false;
    public string name;
    public string password;

    public int battleHeroId = 0;
    public int acc_gold;

    public List<Hero> heroes;
    public List<Character> сharacters;
    public List<PlayerItem> items;

    public Account()
    {
        this.heroes = new List<Hero>();
        this.сharacters = new List<Character>();
        this.items = new List<PlayerItem>();
    }

    #region Acc data : get/set
    public string Get_Acc_Data()
    {
        return battleHeroId + "," + acc_gold;
    }

    public void Set_Acc_Data(string data)
    {
        string[] acc_Data = data.Split(',');
        battleHeroId = Convert.ToInt32(acc_Data[0]);
        acc_gold = Convert.ToInt32(acc_Data[1]);
    }
    #endregion

    #region Acc heroes data : get/set
    public string Get_Acc_Heroes_Data()
    {
        string data = "";

        for(int x = 0; x < heroes.Count; x++)
        {
            Hero hero = heroes[x];

            data += hero.name + "," + hero.character.id + ":";
            if(hero.battleCharacters.Count > 0) data += Get_Hero_CharactersData(hero);

            data += ";";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }
    
    private string Get_Hero_CharactersData(Hero hero)
    {
        string data = "";

        for(int x = 0; x < hero.battleCharacters.Count; x++)
        {
            Character c = hero.battleCharacters[x];
            data += c.id + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    public void Set_Acc_Heroes_Data(string data)
    {
        if(data == "") return;
        CharactersData cd = new CharactersData();
        
        string[] heroesData = data.Split(';');
        for(int x = 0; x < heroesData.Length; x++)
        {
            string[] heroData = heroesData[x].Split(':');

            string[] hero = heroData[0].Split(','); // Hero
            Character hCharacter = cd.Get_Character_ById(Convert.ToInt32(hero[1]));
            Hero h = new Hero(hCharacter, hero[0]);

            if(heroData[1] != "")
            {
                string[] hCharacters = heroData[1].Split(','); // Selected Characters
                for(int y = 0; y < hCharacters.Length; y++)
                {
                    Character c = cd.Get_Character_ById(Convert.ToInt32(hCharacters[y]));
                    h.battleCharacters.Add(c);
                }
            }

            heroes.Add(h);
        }
    }
    #endregion

    #region Acc Characters data : get/set
    public string Get_Acc_CharactersData()
    {
        string data = "";

        for(int x = 0; x < сharacters.Count; x++)
        {
            Character c = сharacters[x];
            data += c.id + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    public void Set_Acc_CharactersData(string data)
    {
        if(data == "") return;
        CharactersData cd = new CharactersData();

        string[] characters = data.Split(',');
        for(int x = 0; x < characters.Length; x++)
        {
            Character c = cd.Get_Character_ById(Convert.ToInt32(characters[x]));
            сharacters.Add(c);
        }
    }
    #endregion

    #region Acc items data : get/set
    public string Get_Acc_ItemsData()
    {
        string data = "";

        for(int x = 0; x < items.Count; x++)
        {
            PlayerItem i = items[x];
            data += i.id + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    public void Set_Acc_ItemsData(string data)
    {
        if(data == "") return;
        PlayerItemsData playerItemsData = new PlayerItemsData();

        string[] itemsData = data.Split(',');
        for(int x = 0; x < itemsData.Length; x++)
        {
            PlayerItem pi = playerItemsData.Get_PlayerItem_ById(Convert.ToInt32(itemsData[x]));
            items.Add(pi);
        }
    }
    #endregion
}
