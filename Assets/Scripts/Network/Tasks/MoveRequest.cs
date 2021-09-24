using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class MoveRequest : GeneralNetworkTask
{
    public int start_x { get; set; }
    public int start_y { get; set; }
    public int end_x { get; set; }
    public int end_y { get; set; }

    public override IEnumerator Implementation_Server()
    {
        GameMain gm = GameObject.Find("GameMain").GetComponent<GameMain>();

        Hex from = Utility.Get_Hex_ByCoords(this.start_x, this.start_y);
        Hex to = Utility.Get_Hex_ByCoords(this.end_x, this.end_y);

        gm.On_Move(from, to);
        yield return null;
    }

    public override void SendToClients(Server server)
    {

    }

    public override IEnumerator Implementation_Client()
    {
        yield return null;
    }

    public override void RequestServer()
    {
        Client client = GameData.inst.client;
        client.netProcessor.Send(client.netManager.GetPeerById(0), this, DeliveryMethod.ReliableOrdered);
    }
}
