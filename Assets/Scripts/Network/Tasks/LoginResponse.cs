using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginResponse : GeneralNetworkTask
{
    public string acc_Data { get; set; }
    public string acc_Heroes { get; set; }
    public string acc_Characters { get; set; }
    public string acc_Items { get; set; }

    // Data recieved from Client, should be applied to some player on Host.
    public override IEnumerator Implementation_Server()
    {
        string[] acc_Data_Recieved = this.acc_Data.Split(',');

        Account acc = Utility.Get_Server_Player_ByName(acc_Data_Recieved[0]);
        acc.Set_Acc_Data(this.acc_Data);
        acc.Set_Acc_Heroes_Data(this.acc_Heroes);
        acc.Set_Acc_CharactersData(this.acc_Characters);
        acc.Set_Acc_ItemsData(this.acc_Items);

        yield return null;
    }

    // Data to be send to Host on login.
    public override IEnumerator Implementation_Client()
    {
        Account acc = GameData.inst.account;

        this.acc_Data = acc.Get_Acc_Data(); // aName,aPass,bhId
        this.acc_Heroes = acc.Get_Acc_Heroes_Data(); // hName,hcId:cId,cId,cId|
        this.acc_Characters = acc.Get_Acc_CharactersData(); // cId,cId,cId
        this.acc_Items = acc.Get_Acc_ItemsData(); // cId,cId,cId

        yield return null;
    }

    public override void SendToClients(Server server)
    {
    }

    public override void RequestServer()
    {
    }
}
