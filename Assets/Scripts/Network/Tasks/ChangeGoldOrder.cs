using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;

public class ChangeGoldOrder : GeneralNetworkTask
{
    public int value { get; set; }
    public string accName { get; set; }

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

        yield return Implementation();

        TaskDone(taskId);
    }

    public override void RequestServer()
    {
        
    }

    private IEnumerator Implementation()
    {
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;

        LocalData localData = new LocalData();

        Account acc_ToChange = null;
        Account account = GameData.inst.account;

        if(server != null)
        {
            acc_ToChange = Utility.Get_Server_Player_ByName(accName);
            acc_ToChange.acc_gold += this.value;
            if(account == acc_ToChange) localData.Save_PlayerData(account);
        }
        else
        {
            acc_ToChange = Utility.Get_Client_Player_ByName(accName);
            acc_ToChange.acc_gold += this.value;
            if(account.name == acc_ToChange.name)
            {
                account.acc_gold += this.value;
                localData.Save_PlayerData(account);
            }
        }

        yield return null;
    }
}
