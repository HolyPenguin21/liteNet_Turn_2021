using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token_Forest : PlayerItem
{
    public Token_Forest()
    {
        PlayerItemsData piData = new PlayerItemsData();
        this.id = 4;
        this.name = "Forest token";
        this.image = piData.Get_PlayerItemImage_ById(this.id);
    }

    public override void Effect()
    {

    }
}
