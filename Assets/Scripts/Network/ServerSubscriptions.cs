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

    public void LoginResponse()
    {
        netProcessor.SubscribeReusable<LoginResponse, NetPeer>((data, client) => {
            Debug.Log("Server > Client has logged in : " + data.pName);

            for(int x = 0; x < GameData.inst.server.players.Count; x++)
                if(GameData.inst.server.players[x].address == client)
                {
                    GameData.inst.server.players[x].name = data.pName;
                    break;
                }

            LoginResponse loginResponse = data;
            GameData.inst.StartCoroutine(loginResponse.Implementation_Server());
            
            PlayersData playersData = new PlayersData();
            GameData.inst.StartCoroutine(playersData.Implementation_Server());
            
            for (int x = 0; x < server.players.Count; x++)
            {
                if (server.players[x].isServer) continue;
                netProcessor.Send(server.players[x].address, playersData, DeliveryMethod.ReliableOrdered);
            }

            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                MenuSceneMain menuSceneMain = GameObject.Find("SceneMain").GetComponent<MenuSceneMain>();
                menuSceneMain.UpdateConnectedPlayersList();

                for (int x = 0; x < server.players.Count; x++)
                {
                    menuSceneMain.Add_PlayerPanel(server.players[x]);
                }
            }
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
