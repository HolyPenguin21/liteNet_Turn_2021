using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : Character
{
    public Swordman(GameObject go_, Hex hex_, BattlePlayer owner_)
    {
        go = go_;
        if (go != null) tr = go_.transform;
        cHex = hex_;
        owner = owner_;

        // Edit
        cId = 1;
        cCost = 15;
        cName = "Swordman";
        cImage = Utility.Get_CharacterImage_ById(cId);
    }
}
