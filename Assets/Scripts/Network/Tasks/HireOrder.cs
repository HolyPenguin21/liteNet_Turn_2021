using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class HireOrder : GeneralNetworkTask
{
    public int hex_x { get; set; }
    public int hex_y { get; set; }
    public string ownerName { get; set; }
    public int characterId { get; set; }

    public override IEnumerator Implementation_Server()
    {
        Server server = GameData.inst.server;
        if(server == null) yield return null;
        SendToClients(server);

        yield return Implementation();
       
        TaskDone(taskId);
    }

    public override void SendToClients(Server server)
    {
        Debug.Log("Server > Sending to clients : Hire, id : " + taskId);
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
        
    }

    private IEnumerator Implementation()
    {
        if(GameData.inst.server == null) yield break;

        GameMain gm = GameData.inst.gameMain;

        Hex hex = Utility.Get_Hex_ByCoords(hex_x, hex_y);
        BattlePlayer owner = Utility.Get_BattlePlayer_ByName(ownerName);
        Character character = owner.availableCharacters[this.characterId];

        int charId = character.id;
        gm.Order_CreateCharacter(hex, owner, character);

        yield return null;
    }
}
