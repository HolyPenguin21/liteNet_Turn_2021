using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeBuffData
{
    public LifetimeBuff Get_LifetimeBuff_ById(int id)
    {
        LifetimeBuff buff = null;
        switch(id)
        {
            case 1:
                buff = new Strong();
            break;
            case 2:
                buff = new BrokenLeg();
            break;

            default:
                buff = new Strong();
            break;
        }

        return buff;
    }
}
