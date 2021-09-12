using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginResponse : GeneralNetworkTask
{
    public string acc_Data { get; set; }
    public string pHeroes { get; set; }
    public string pCharacters { get; set; }
    public string pItems { get; set; }

    // Data recieved from Client, should be applied to some player on Host.
    public override IEnumerator Implementation_Server()
    {
        // Debug.Log("Server : Player name : " + this.pName);
        // Debug.Log("Server : Player heroes data : " + this.pHeroes);
        // Debug.Log("Server : Player characters : " + this.pCharacters);

        string[] acc_Data_Recieved = this.acc_Data.Split(',');

        Account acc = Utility.Get_Server_Player_ByName(acc_Data_Recieved[0]);
        acc.battleHeroId = Convert.ToInt32(acc_Data_Recieved[1]);

        Set_PlayerHeroes_Host(acc);
        Set_PlayerCharacters_Host(acc);
        Set_PlayerItems_Host(acc);

        yield return null;
    }

    private void Set_PlayerHeroes_Host(Account acc)
    {
        if(this.pHeroes == "") return;
        CharactersData cd = new CharactersData();

        string[] heroes = this.pHeroes.Split('|');
        for(int x = 0; x < heroes.Length; x++)
        {
            string[] heroData = heroes[x].Split(':');

            string[] hero = heroData[0].Split(','); // Hero
            Character hCharacter = cd.Get_Character_ById(Convert.ToInt32(hero[1]));
            Hero h = new Hero(hCharacter, hero[0]);

            string[] hCharacters = heroData[1].Split(','); // Selected Characters
            for(int y = 0; y < hCharacters.Length; y++)
            {
                Character c = cd.Get_Character_ById(Convert.ToInt32(hCharacters[y]));
                h.battleCharacters.Add(c);
            }

            acc.heroes.Add(h);
        }
    }

    private void Set_PlayerCharacters_Host(Account p)
    {
        if(this.pCharacters == "") return;
        CharactersData cd = new CharactersData();

        string[] characters = this.pCharacters.Split(',');
        for(int x = 0; x < characters.Length; x++)
        {
            Character c = cd.Get_Character_ById(Convert.ToInt32(characters[x]));
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

        this.acc_Data = acc.Get_Acc_Data(); // aName,bhId
        this.pHeroes = acc.Get_Heroes_Data(); // hName,hcId:cId,cId,cId|
        this.pCharacters = acc.Get_Acc_CharactersData(); // cId,cId,cId
        this.pItems = acc.Get_Acc_ItemsData(); // cId,cId,cId

        yield return null;
    }

    public override void SendToClients(Server server)
    {
    }

    public override void RequestServer()
    {
    }
}
