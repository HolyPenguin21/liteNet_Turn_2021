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

    public List<Hero> heroes;
    public List<Character> сharacters;
    public List<PlayerItem> items;

    public int acc_gold;

    public int battleHeroId = 0;

    public Account (string name)
    {
        this.name = name;

        this.heroes = new List<Hero>();
        this.сharacters = new List<Character>();
        this.items = new List<PlayerItem>();
    }

    #region Acc data : get/set
    public string Get_Acc_Data()
    {
        return name + "," + battleHeroId;
    }
    #endregion

    #region Acc heroes data : get/set
    public string Get_Heroes_Data()
    {
        string data = "";

        for(int x = 0; x < heroes.Count; x++)
        {
            Hero hero = heroes[x];

            data += hero.name + "," + hero.character.cId + ":";
            if(hero.battleCharacters.Count > 0) data += Get_Hero_CharactersData(hero);

            data += "|";
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
            data += c.cId + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }
    #endregion

    #region Acc Characters data : get/set
    public string Get_Acc_CharactersData()
    {
        string data = "";

        for(int x = 0; x < сharacters.Count; x++)
        {
            Character c = сharacters[x];
            data += c.cId + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
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
    #endregion
}
