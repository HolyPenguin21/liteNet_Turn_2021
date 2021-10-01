using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class HireRequest : GeneralNetworkTask
{
    public int hex_x { get; set; }
    public int hex_y { get; set; }
    public string ownerName { get; set; }
    public int characterId { get; set; }

    public override IEnumerator Implementation_Server()
    {
        GameMain gm = GameData.inst.gameMain;

        Hex hex = Utility.Get_Hex_ByCoords(hex_x, hex_y);
        BattlePlayer owner = Utility.Get_BattlePlayer_ByName(ownerName);
        Character character = owner.availableCharacters[characterId];

        gm.On_Hire(hex, owner, character);
        
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
