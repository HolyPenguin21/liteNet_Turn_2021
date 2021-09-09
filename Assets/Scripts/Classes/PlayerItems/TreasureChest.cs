using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : PlayerItem
{
    public TreasureChest ()
    {
        this.name = "Treasure chest";
    }

    public override void Effect()
    {
        Debug.Log("Treasure chest");
    }
}
