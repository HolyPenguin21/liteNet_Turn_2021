using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class CreateAICharacter : GeneralNetworkTask
{
    public int coord_x { get; set; }
    public int coord_y { get; set; }
    public string ownerName { get; set; }
    public int characterId { get; set; }
    public int isHero { get; set; }

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
        Debug.Log("Server > Sending to clients : Create AI character, id : " + taskId);
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
        CharactersData cd = new CharactersData();

        Hex hex = Utility.Get_Hex_ByCoords(this.coord_x, this.coord_y);
        BattlePlayer owner = Utility.Get_BattlePlayer_ByName(this.ownerName);
        Character character = cd.Get_Character_ById(this.characterId);

        character.Reset();
        character.Set_CharacterLifetimeBuffs();

        if(this.isHero == 1) owner.heroCharacter = character;
        if(this.isHero == 0) 
        {
            owner.ingameCharacters.Add(character);
        }

        cd.Create_Character(hex, owner, character);

        return true;
    }
}
