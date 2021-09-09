using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Character
{
    public Knight(GameObject go_, Hex hex_, BattlePlayer owner_)
    {
        go = go_;
        if (go != null) tr = go_.transform;
        cHex = hex_;
        cOwner = owner_;

        // Edit
        cId = 3;
        cCost = 20;
        cName = "Knight";
        cImage = Utility.Get_CharacterImage_ById(cId);
    }
}
