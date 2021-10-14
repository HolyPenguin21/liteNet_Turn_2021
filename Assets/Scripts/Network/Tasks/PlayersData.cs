using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersData : GeneralNetworkTask
{
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
            accountData.name = acc.name;
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
            AccountData accountData = allNetworkAccountData.netAccountsData[x];

            Account acc = new Account();
            acc.name = accountData.name;
            acc.Set_Acc_Data(accountData.data);
            acc.Set_Acc_Heroes_Data(accountData.heroes);
            Debug.Log(accountData.characters);
            acc.Set_Acc_CharactersData(accountData.characters);
            acc.Set_Acc_ItemsData(accountData.items);

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
