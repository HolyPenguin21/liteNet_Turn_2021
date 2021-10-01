using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginResponse : GeneralNetworkTask
{
    public string playerData { get; set; }

    // Data recieved from Client, should be applied to some player on Host.
    public override IEnumerator Implementation_Server()
    {
        AccountData accData = JsonUtility.FromJson<AccountData>(this.playerData);

        Account acc = Utility.Get_Server_Player_ByName(accData.name);
        acc.name = accData.name;
        acc.Set_Acc_Data(accData.data);
        acc.Set_Acc_Heroes_Data(accData.heroes);
        acc.Set_Acc_CharactersData(accData.characters);
        acc.Set_Acc_ItemsData(accData.items);

        yield return null;
    }

    // Data to be send to Host on login.
    public override IEnumerator Implementation_Client()
    {
        Account acc = GameData.inst.account;

        AccountData accData = new AccountData();
        accData.name = acc.name;
        accData.data = acc.Get_Acc_Data();
        accData.heroes = acc.Get_Acc_Heroes_Data();
        accData.characters = acc.Get_Acc_CharactersData();
        accData.items = acc.Get_Acc_ItemsData();

        this.playerData = JsonUtility.ToJson(accData);

        yield return null;
    }

    public override void SendToClients(Server server)
    {
    }

    public override void RequestServer()
    {
    }
}
