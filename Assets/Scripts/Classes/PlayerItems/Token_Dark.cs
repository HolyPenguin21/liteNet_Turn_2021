using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token_Dark : PlayerItem
{
    public Token_Dark()
    {
        PlayerItemsData piData = new PlayerItemsData();
        this.id = 5;
        this.name = "Dark token";
        this.image = piData.Get_PlayerItemImage_ById(this.id);
    }

    public override void Effect()
    {

    }
}
