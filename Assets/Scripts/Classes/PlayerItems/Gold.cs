using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : PlayerItem
{
    public int value = 0;

    public Gold ()
    {
        PlayerItemsData piData = new PlayerItemsData();
        this.id = 1;
        this.name = "Gold";
        this.image = piData.Get_PlayerItemImage_ById(this.id);
        this.oneTime = true;

        this.value = Random.Range(1, 6);
    }

    public override void Effect()
    {
        Account account = GameData.inst.account;
        
        account.acc_gold += value;

        LocalData ld = new LocalData();
        ld.Save_PlayerData(account);
    }
}
