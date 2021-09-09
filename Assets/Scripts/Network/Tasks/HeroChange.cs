using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LiteNetLib;
using LiteNetLib.Utils;

public class HeroChange : GeneralNetworkTask
{
    public string pName { get; set; }
	public int hId { get; set; }

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
            Account player = server.players[x];
            if (player.isServer) continue;
            server.netProcessor.Send(player.address, this, DeliveryMethod.ReliableOrdered);
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
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;

        var dropdowns = GameObject.FindObjectsOfType<Dropdown>();
        foreach(Dropdown d in dropdowns)
        {
            if(d.transform.parent.Find("PlayerName_Text").GetComponent<Text>().text == this.pName)
                d.value = this.hId;
        }

        if(server != null)
        {
            Utility.Get_Server_Player_ByName(this.pName).battleHeroId = this.hId;
        }

        if(client != null)
        {
            Utility.Get_Client_Player_ByName(this.pName).battleHeroId = this.hId;
        }

        return true;
    }
}
