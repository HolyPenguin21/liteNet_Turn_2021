using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LiteNetLib;
using LiteNetLib.Utils;

public class SetTurn : GeneralNetworkTask
{
    public int bpId { get; set; }

    public override IEnumerator Implementation_Server()
    {
        Server server = GameData.inst.server;
        if(server == null) yield return null;
        SendToClients(server);

        yield return Implementation();

        TaskDone(taskId);
        yield return null;
    }

    public override void SendToClients(Server server)
    {
        Debug.Log("Server > Sending to clients : HeroChange, id : " + taskId);
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

        yield return Implementation();

        TaskDone(taskId);
    }

    public override void RequestServer()
    {
        Client client = GameData.inst.client;
        client.netProcessor.Send(client.netManager.GetPeerById(0), this, DeliveryMethod.ReliableOrdered);
    }

    private bool Implementation()
    {
        SceneMain sm = GameObject.Find("SceneMain").GetComponent<SceneMain>();
        sm.currentTurn = sm.bPlayers[bpId];
        sm.currentTurn_Text.text = "Current turn for : " + sm.currentTurn.name;

        return true;
    }
}
