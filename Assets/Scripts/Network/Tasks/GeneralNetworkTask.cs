using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public abstract class GeneralNetworkTask
{
    public int taskId { get; set; }
    public string taskDone_pName { get; set; }

    public List<Account> assignedPlayers = new List<Account>();

    public void AssignToAll()
    {
        for(int x = 0; x < GameData.inst.server.players.Count; x++)
        {
            Account p = GameData.inst.server.players[x];
            assignedPlayers.Add(p);
        }
    }

    public abstract IEnumerator Implementation_Server();
    public abstract void SendToClients(Server server);
    public abstract IEnumerator Implementation_Client();
    public abstract void RequestServer();

    public void TaskDone(int taskId)
    {
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;

        // SERVER
        if(server != null)
        {
            server.taskCurrent.assignedPlayers.Remove(GameData.inst.account);

            if(server.taskCurrent.assignedPlayers.Count == 0)
            {
                Debug.Log("No players left remove task");
                server.taskCurrent = null;
                if(server.taskList.Count > 0)
                {
                    Debug.Log("There are new tasks to do : " + server.taskList.Count + ". New one should start...");
                }
            }
            else
            {
                Debug.Log("Task " + server.taskCurrent.taskId + " is in cours for other players. Players left : " + server.taskCurrent.assignedPlayers.Count);
            }
        }
        // CLIENT
        else
        {
            // Debug.Log("Client > Sending done to Server");
            client.netProcessor.Send(client.netManager.GetPeerById(0), new TaskDone() { taskId_done = taskId, pName = GameData.inst.account.name }, DeliveryMethod.ReliableOrdered);
        }
    }
}
