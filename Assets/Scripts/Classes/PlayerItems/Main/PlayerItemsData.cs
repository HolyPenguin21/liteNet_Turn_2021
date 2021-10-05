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
            
            case 2:
                return new Token_General();
            
            case 3:
                return new Token_Castle();
            
            case 4:
                return new Token_Forest();
            
            case 5:
                return new Token_Dark();
            
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

            case 2:
                return Resources.Load<Sprite>("PlayerItem/Gold/Gold");

            case 3:
                return Resources.Load<Sprite>("PlayerItem/Gold/Gold");

            case 4:
                return Resources.Load<Sprite>("PlayerItem/Gold/Gold");

            case 5:
                return Resources.Load<Sprite>("PlayerItem/Gold/Gold");

            default:
                return Resources.Load<Sprite>("PlayerItem/Gold/Gold");
        }
    }
}
