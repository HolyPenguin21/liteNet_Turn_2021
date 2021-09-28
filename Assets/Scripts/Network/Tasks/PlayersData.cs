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

    public string playersData { get; set; }

    // Data to be send to all Clients to update them
    public override IEnumerator Implementation_Server()
    {
        Server server = GameData.inst.server;

        AllNetworkAccountData allNetworkAccountData = new AllNetworkAccountData();
        for(int x = 0; x < server.players.Count; x++)
        {
            Account acc = server.players[x];
            
            AccountData accountData = new AccountData();
            accountData.data = acc.Get_Acc_Data();
            accountData.heroes = acc.Get_Acc_Heroes_Data();
            accountData.characters = acc.Get_Acc_CharactersData();
            accountData.items = acc.Get_Acc_ItemsData();

            allNetworkAccountData.netAccountsData.Add(accountData);
        }
        this.playersData = JsonUtility.ToJson(allNetworkAccountData);

        yield return null;
    }

    public override IEnumerator Implementation_Client()
    {
        Client client = GameData.inst.client;
        client.players.Clear();

        AllNetworkAccountData allNetworkAccountData = JsonUtility.FromJson<AllNetworkAccountData>(this.playersData);
        for(int x = 0; x < allNetworkAccountData.netAccountsData.Count; x++)
        {
            AccountData netAccountData = allNetworkAccountData.netAccountsData[x];

            string[] acc_data = netAccountData.data.Split(',');
            Account acc = new Account(acc_data[0]);
            acc.Set_Acc_Data(netAccountData.data);
            acc.Set_Acc_Heroes_Data(netAccountData.heroes);
            acc.Set_Acc_CharactersData(netAccountData.characters);
            acc.Set_Acc_ItemsData(netAccountData.items);

            client.players.Add(acc);
        }

        yield return null;
    }

    public override void SendToClients(Server server)
    {
    }

    public override void RequestServer()
    {
    }

    [System.Serializable]
    public class AllNetworkAccountData
    {
        public List<AccountData> netAccountsData = new List<AccountData>();
    }
}
