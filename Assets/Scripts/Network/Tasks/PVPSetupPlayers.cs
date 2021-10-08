using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class PVPSetupPlayers : GeneralNetworkTask
{
    public override IEnumerator Implementation_Server()
    {
        Server server = GameData.inst.server;
        if(server == null) yield return null;
        SendToClients(server);

        yield return Server_Part(server);
       
        TaskDone(taskId);
    }

    private IEnumerator Server_Part(Server server)
    {
        LocalData localData = new LocalData();
        SceneMain sceneMain = GameData.inst.gameMain.sceneMain;

        for(int x = 0; x < server.players.Count; x++)
        {
            Account account = server.players[x];
            BattlePlayer battlePlayer = new BattlePlayer(account, false);
            sceneMain.battlePlayers_List.Add(battlePlayer);

            if(account.name != GameData.inst.account.name) continue;

            sceneMain.battlePlayer = battlePlayer;
            localData.Save_PlayerData(account);
        }

        BattlePlayer aiBattlePlayer = new BattlePlayer(null, true);
        sceneMain.battlePlayers_List.Add(aiBattlePlayer);

        yield return null;
    }

    public override void SendToClients(Server server)
    {
        Debug.Log("Server > Sending to clients : SetupPvpPlayers order, id : " + taskId);
        for (int x = 0; x < server.players.Count; x++)
        {
            Account player = server.players[x];
            if (player.isServer) continue;
            server.netProcessor.Send(player.address, this, DeliveryMethod.ReliableOrdered);
        }
    }

    public override IEnumerator Implementation_Client()
    {
        Client client = GameData.inst.client;
        if(client == null) yield return null;

        yield return Client_Part(client);

        TaskDone(taskId);
    }

    private IEnumerator Client_Part(Client client)
    {
        LocalData localData = new LocalData();
        SceneMain sceneMain = GameData.inst.gameMain.sceneMain;

        for(int x = 0; x < client.players.Count; x++)
        {
            Account account = client.players[x];
            BattlePlayer battlePlayer = new BattlePlayer(account, false);
            sceneMain.battlePlayers_List.Add(battlePlayer);

            if(account.name != GameData.inst.account.name) continue;

            sceneMain.battlePlayer = battlePlayer;
            localData.Save_PlayerData(account);
        }

        BattlePlayer aiBattlePlayer = new BattlePlayer(null, true);
        sceneMain.battlePlayers_List.Add(aiBattlePlayer);

        yield return null;
    }

    public override void RequestServer()
    {
        
    }
}
