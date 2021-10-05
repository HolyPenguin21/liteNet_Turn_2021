using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token_General : PlayerItem
{
    public Token_General()
    {
        PlayerItemsData piData = new PlayerItemsData();
        this.id = 2;
        this.name = "General token";
        this.image = piData.Get_PlayerItemImage_ById(this.id);
    }

    public override void Effect()
    {

    }
}
