using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginResponse : GeneralNetworkTask
{
    public string pName { get; set; }
    public int battleHeroId { get; set; }
    public string pHeroes { get; set; }
    public string pCharacters { get; set; }
    public string pItems { get; set; }

    // Data recieved from Client, should be applied to some player on Host.
    public override IEnumerator Implementation_Server()
    {
        // Debug.Log("Server : Player name : " + this.pName);
        // Debug.Log("Server : Player heroes data : " + this.pHeroes);
        // Debug.Log("Server : Player characters : " + this.pCharacters);

        Account acc = Utility.Get_Server_Player_ByName(this.pName);
        acc.battleHeroId = this.battleHeroId;

        Set_PlayerHeroes_Host(acc);
        Set_PlayerCharacters_Host(acc);
        Set_PlayerItems_Host(acc);

        yield return null;
    }

    private void Set_PlayerHeroes_Host(Account acc)
    {
        if(this.pHeroes == "") return;

        string[] heroes = this.pHeroes.Split('|');
        for(int x = 0; x < heroes.Length; x++)
        {
            string[] heroData = heroes[x].Split(':');

            string[] hero = heroData[0].Split(','); // Hero
            Character hCharacter = Utility.Get_Character_ById(Convert.ToInt32(hero[1]));
            Hero h = new Hero(hCharacter, hero[0]);

            string[] hCharacters = heroData[1].Split(','); // Selected Characters
            for(int y = 0; y < hCharacters.Length; y++)
            {
                Character c = Utility.Get_Character_ById(Convert.ToInt32(hCharacters[y]));
                h.battleCharacters.Add(c);
            }

            acc.heroes.Add(h);
        }
    }

    private void Set_PlayerCharacters_Host(Account p)
    {
        if(this.pCharacters == "") return;

        string[] characters = this.pCharacters.Split(',');
        for(int x = 0; x < characters.Length; x++)
        {
            Character c = Utility.Get_Character_ById(Convert.ToInt32(characters[x]));
            p.сharacters.Add(c);
        }
    }

    private void Set_PlayerItems_Host(Account p)
    {
        if(this.pItems == "") return;

        string[] itemsData = this.pItems.Split(',');
        for(int x = 0; x < itemsData.Length; x++)
        {
            PlayerItem pi = Utility.Get_PlayerItem_ById(Convert.ToInt32(itemsData[x]));
            p.items.Add(pi);
        }
    }

    // Data to be send to Host on login.
    public override IEnumerator Implementation_Client()
    {
        Account acc = GameData.inst.account;

        this.pName = acc.name;
        this.battleHeroId = acc.battleHeroId;
        this.pHeroes = Get_HeroesData(acc);
        this.pCharacters = Get_PlayerCharactersData(acc);
        this.pItems = Get_PlayerItemsData(acc);

        // Debug.Log("Client : Player name : " + this.pName);
        // Debug.Log("Client : Player heroes data : " + this.pHeroes);
        // Debug.Log("Client : Player characters : " + this.pCharacters);

        yield return null;
    }

    private string Get_HeroesData(Account acc)
    {
        string data = "";

        for(int x = 0; x < acc.heroes.Count; x++)
        {
            Hero hero = acc.heroes[x];

            data += hero.name + "," + hero.character.cId + ":";
            if(hero.battleCharacters.Count > 0) data += Get_HeroCharactersData(hero);

            data += "|";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_HeroCharactersData(Hero hero)
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

    private string Get_PlayerCharactersData(Account p)
    {
        string data = "";

        for(int x = 0; x < p.сharacters.Count; x++)
        {
            Character c = p.сharacters[x];
            data += c.cId + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_PlayerItemsData(Account p)
    {
        string data = "";

        for(int x = 0; x < p.items.Count; x++)
        {
            PlayerItem i = p.items[x];
            data += i.id + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    public override void SendToClients(Server server)
    {
    }

    public override void RequestServer()
    {
    }
}
