using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsData
{
    public PlayerItem Get_PlayerItem_ById(int id)
    {
        switch(id)
        {
            case 1:
                return new Gold();
            default:
                return new Gold();
        }
    }

    public Sprite Get_PlayerItemImage_ById(int id)
    {
        switch(id)
        {
            case 1:
                return Resources.Load<Sprite>("PlayerItem/Gold/Gold");
            default:
                return Resources.Load<Sprite>("PlayerItem/Gold/Gold");
        }
    }
}
