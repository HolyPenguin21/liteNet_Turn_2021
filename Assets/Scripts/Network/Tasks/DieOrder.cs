using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class DieOrder : GeneralNetworkTask
{
    public int coord_x { get; set; }
    public int coord_y { get; set; }

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
        Debug.Log("Server > Sending to clients : DieOrder, id : " + taskId);
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
        Hex hex = Utility.Get_Hex_ByCoords(coord_x, coord_y);
        Character character = hex.character;
        BattlePlayer owner = character.owner;
        SceneMain sceneMain = GameObject.Find("SceneMain").GetComponent<SceneMain>();

        yield return character.Die_Animation();

        owner.ingameCharacters.Remove(character);
        character.hex = null;
        hex.character = null;

        if(GameData.inst.server != null) yield return sceneMain.Check_Dead(character);

        MonoBehaviour.Destroy(character.go);

        yield return null;
    }
}
