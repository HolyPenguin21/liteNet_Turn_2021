using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LiteNetLib;
using LiteNetLib.Utils;

// Client responces for Server
public class ServerSubscriptions
{
    private Server server;
    private NetPacketProcessor netProcessor;
    private GameMain gameMain;

    public ServerSubscriptions(Server server, NetPacketProcessor netProcessor)
    {
        this.server = server;
        this.netProcessor = netProcessor;
        this.gameMain = GameObject.Find("GameMain").GetComponent<GameMain>();
    }

    public void HireRequest()
    {
        netProcessor.SubscribeReusable<HireRequest>((data) => {
            Debug.Log("Server > HireRequest request recieved.");
            HireRequest hireRequest = data;
            GameData.inst.StartCoroutine(hireRequest.Implementation_Server());
        });
    }

    public void AttackRequest()
    {
        netProcessor.SubscribeReusable<AttackRequest>((data) => {
            Debug.Log("Server > RequestAttack request recieved.");
            AttackRequest attackRequest = data;
            GameData.inst.StartCoroutine(attackRequest.Implementation_Server());
        });
    }

    public void MoveRequest()
    {
        netProcessor.SubscribeReusable<MoveRequest>((data) => {
            // Debug.Log("Server > RequestMove request recieved.");
            MoveRequest moveRequest = data;
            GameData.inst.StartCoroutine(moveRequest.Implementation_Server());
        });
    }

    public void SetTurn()
    {
        netProcessor.SubscribeReusable<SetTurn>((data) => {
            // Debug.Log("Server > ChangeTurn request recieved.");
            SceneMain sm = GameObject.Find("SceneMain").GetComponent<SceneMain>();
            sm.Button_EndTurn();
        });
    }

    public void LoginResponse()
    {
        netProcessor.SubscribeReusable<LoginResponse, NetPeer>((data, client) => {
            Debug.Log("Server > Client has logged in : " + client);

            Account acc = Utility.Get_Server_Player_ByAddress(client);
            AccountData accData = JsonUtility.FromJson<AccountData>(data.playerData);
            acc.name = accData.name;

            LoginResponse loginResponse = data;
            GameData.inst.StartCoroutine(loginResponse.Implementation_Server());
            
            PlayersData playersData = new PlayersData();
            GameData.inst.StartCoroutine(playersData.Implementation_Server());
            
            for (int x = 0; x < server.players.Count; x++)
            {
                if (server.players[x].isServer) continue;
                netProcessor.Send(server.players[x].address, playersData, DeliveryMethod.ReliableOrdered);
            }

            MenuSceneMain menuSceneMain = GameObject.Find("SceneMain").GetComponent<MenuSceneMain>();
            menuSceneMain.p2PLobbyMenu.UpdateConnectedPlayersList();
            menuSceneMain.Add_PlayerPanel(acc);
        });
    }

    public void HeroChange()
    {
        netProcessor.SubscribeReusable<HeroChange>((data) => {
            // Debug.Log("Server > HeroChange request recieved.");
            gameMain.Order_HeroChange(data.pName, data.hId);
        });
    }

    public void Update_PlayersListOnClients()
    {
        PlayersData playersData = new PlayersData();
        GameData.inst.StartCoroutine(playersData.Implementation_Server());

        for (int x = 0; x < server.players.Count; x++)
        {
            if (server.players[x].isServer) continue;
            netProcessor.Send(server.players[x].address, playersData, DeliveryMethod.ReliableOrdered);
        }
    }

    public void TaskDone()
    {
        netProcessor.SubscribeReusable<TaskDone>((data) => {
            Debug.Log("Server > Task done by client : " + data.pName + ", task id : " + data.taskId_done);
            
            if(data.taskId_done == server.taskCurrent.taskId)
                server.taskCurrent.assignedPlayers.Remove(Utility.Get_Server_Player_ByName(data.pName));
        });
    }
}
