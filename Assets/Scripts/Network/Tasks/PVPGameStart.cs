using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LiteNetLib;
using LiteNetLib.Utils;

public class PVPGameStart : GeneralNetworkTask
{
    public int sceneId { get; set; }

    public override IEnumerator Implementation_Server()
    {
        Server server = GameData.inst.server;
        if(server == null) yield return null;
        SendToClients(server);

        SceneManager.LoadScene(sceneId);

        TaskDone(taskId);

        yield return null;
    }

    public override void SendToClients(Server server)
    {
        Debug.Log("Server > Sending to clients : StartGame, id : " + taskId);
        for (int x = 0; x < server.players.Count; x++)
        {
            Account acc = server.players[x];
            if (acc.isServer) continue;
            server.netProcessor.Send(acc.address, this, DeliveryMethod.ReliableOrdered);
        }
    }

    public override IEnumerator Implementation_Client()
    {
        Client client = GameData.inst.client;
        if(client == null) yield return null;

        SceneManager.LoadScene(sceneId);

        TaskDone(taskId);

        yield return null;
    }

    public override void RequestServer()
    {
        
    }
}