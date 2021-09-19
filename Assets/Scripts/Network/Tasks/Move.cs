using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class Move : GeneralNetworkTask
{
    public int start_x { get; set; }
    public int start_y { get; set; }
    public int end_x { get; set; }
    public int end_y { get; set; }
    public int mpLeft { get; set; }
    public string path { get; set; }

    public override IEnumerator Implementation_Server()
    {
        Server server = GameData.inst.server;
        if(server == null) yield return null;
        SendToClients(server);

        Character c = Utility.Get_Hex_ByCoords(this.start_x, this.start_y).character;
        List<Hex> hexPath = Utility.Get_HexPath_ByCoords(this.path);

        c.movement.movePoints_cur = this.mpLeft;
        yield return c.Move(hexPath);

        TaskDone(taskId);
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

        Character c = Utility.Get_Hex_ByCoords(this.start_x, this.start_y).character;
        List<Hex> hexPath = Utility.Get_HexPath_ByCoords(this.path);

        c.movement.movePoints_cur = this.mpLeft;
        yield return c.Move(hexPath);

        TaskDone(taskId);
    }

    public override void RequestServer()
    {
        
    }
}
