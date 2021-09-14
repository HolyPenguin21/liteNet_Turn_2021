using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersData : GeneralNetworkTask
{
    public string accounts { get; set; }
    public string pHeroes { get; set; }
    public string pCharacters { get; set; }
    public string pItems { get; set; }

    // Data to be send to all Clients to update them
    public override IEnumerator Implementation_Server()
    {
        Server server = GameData.inst.server;

        this.accounts = Get_All_AccountsData(server); // aName,aPass,aSelHero,aGold|
        this.pHeroes = Get_All_AccountsHeroesData(server); // 0,qwe_Hero_1,1:1,2,3|1,asd_Hero_1,1:1,1,2,1;asd_Hero_2,3:
        this.pCharacters = Get_All_AccountsCharactersData(server);
        this.pItems = Get_All_AccountsItemsData(server);

        yield return null;
    }

    private string Get_All_AccountsData(Server serv)
    {
        string data = "";

        for(int x = 0; x < serv.players.Count; x++)
        {
            Account acc = serv.players[x];
            data += acc.Get_Acc_Data() + "|";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_All_AccountsHeroesData(Server s)
    {
        string data = "";

        for(int x = 0; x < s.players.Count; x++)
        {
            Account acc = s.players[x];
            data += x + ";" + acc.Get_Acc_Heroes_Data() + "|";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_All_AccountsCharactersData(Server s)
    {
        string data = "";

        for(int x = 0; x < s.players.Count; x++)
        {
            Account acc = s.players[x];
            data += x + ":" + acc.Get_Acc_CharactersData() + "|";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_All_AccountsItemsData(Server s)
    {
        string data = "";

        for(int x = 0; x < s.players.Count; x++)
        {
            Account acc = s.players[x];

            data += x + ":" + acc.Get_Acc_ItemsData() + "|";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    public override IEnumerator Implementation_Client()
    {
        Client client = GameData.inst.client;

        Set_Accounts(client);           // aName,aPass,aBattleHero,aGold|
        Set_AccountsHeroes(client);      // 0;qwe_Hero_1,1:1,2,3|1;asd_Hero_1,1:1,1,2,1;asd_Hero_2,3:
        Set_AccountsCharacters(client);  // 0:1|1:1
        Set_AccountsItems(client);       // 0:1|1:1

        yield return null;
    }

    private void Set_Accounts(Client c)
    {
        // aName,aPass,aBattleHero,aGold|
        c.players.Clear();

        string[] data = this.accounts.Split('|');
        for(int x = 0; x < data.Length; x++)
        {
            string[] acc_data = data[x].Split(',');

            Account acc = new Account(acc_data[0]);
            acc.Set_Acc_Data(data[x]);

            c.players.Add(acc);
        }
    }

    private void Set_AccountsHeroes(Client c)
    {
        CharactersData cd = new CharactersData();

        string[] data = this.pHeroes.Split('|');
        for(int x = 0; x < data.Length; x++)
        {
            string[] heroes = data[x].Split(';');
            Account acc = c.players[Convert.ToInt32(heroes[0])];

            for(int y = 1; y < heroes.Length; y++)
            {
                acc.Set_Acc_Heroes_Data(heroes[y]);
            }
        }
    }

    private void Set_AccountsCharacters(Client c)
    {
        CharactersData cd = new CharactersData();

        string[] data = this.pCharacters.Split('|');
        for(int x = 0; x < data.Length; x++)
        {
            string[] acc_data = data[x].Split(':');
            Account acc = c.players[Convert.ToInt32(acc_data[0])];
            acc.Set_Acc_CharactersData(acc_data[1]);
        }
    }

    private void Set_AccountsItems(Client c)
    {
        string[] data = this.pItems.Split('|');
        for(int x = 0; x < data.Length; x++)
        {
            string[] acc_data = data[x].Split(':');
            Account acc = c.players[Convert.ToInt32(acc_data[0])];
            acc.Set_Acc_ItemsData(acc_data[1]);
        }
    }

    public override void SendToClients(Server server)
    {
    }

    public override void RequestServer()
    {
    }
}
