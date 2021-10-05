using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token_Castle : PlayerItem
{
    public Token_Castle()
    {
        PlayerItemsData piData = new PlayerItemsData();
        this.id = 3;
        this.name = "Castle token";
        this.image = piData.Get_PlayerItemImage_ById(this.id);
    }

    public override void Effect()
    {

    }
}
