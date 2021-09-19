using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Character
{
    public Knight(GameObject go_, Hex hex_, BattlePlayer owner_)
    {
        go = go_;
        if (go != null) tr = go_.transform;

        hex = hex_;
        owner = owner_;
        
        CharactersData cd = new CharactersData();
        // Edit
        id = 3;
        cost = 20;
        name = "Knight";
        image = cd.Get_CharacterImage_ById(id);

        movement.movePoints_max = 4;
    }
}
