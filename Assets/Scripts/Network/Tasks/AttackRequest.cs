using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class AttackRequest : GeneralNetworkTask
{
    public int attacker_x { get; set; }
    public int attacker_y { get; set; }
    public int a_AttackId { get; set; }

    public int target_x { get; set; }
    public int target_y { get; set; }
    public int t_AttackId { get; set; }

    public override IEnumerator Implementation_Server()
    {
        Hex a_Hex = Utility.Get_Hex_ByCoords(attacker_x, attacker_y);
        Hex t_Hex = Utility.Get_Hex_ByCoords(target_x, target_y);

        GameData.inst.gameMain.Order_Attack(a_Hex, this.a_AttackId, t_Hex, this.t_AttackId);
        yield return null;
    }    

    public override void SendToClients(Server server)
    {
        Debug.Log("Server > Sending to clients : Move, id : " + taskId);
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



        TaskDone(taskId);
    }

    public override void RequestServer()
    {
        Client client = GameData.inst.client;
        client.netProcessor.Send(client.netManager.GetPeerById(0), this, DeliveryMethod.ReliableOrdered);
    }
}
