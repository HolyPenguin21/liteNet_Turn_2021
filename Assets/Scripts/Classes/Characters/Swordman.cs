using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : Character
{
    public Swordman(GameObject go_, Hex hex_, BattlePlayer owner_)
    {
        go = go_;
        if (go != null) tr = go_.transform;

        hex = hex_;
        owner = owner_;

        CharactersData cd = new CharactersData();
        // Edit
        id = 1;
        cost = 15;
        name = "Swordman";
        image = cd.Get_CharacterImage_ById(id);

        movement.movePoints_max = 4;

        attacks = new List<CharVars.char_Attack>();
        CharVars.char_Attack attack1 = new CharVars.char_Attack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Impact;
        attack1.attacksCount = 3;
        attack1.attackDmg_base = 5;
        attack1.attackDmg_cur = attack1.attackDmg_base;
        attacks.Add(attack1);

        CharVars.char_Attack attack2 = new CharVars.char_Attack();
        attack2.attackType = CharVars.attackType.Melee;
        attack2.attackDmgType = CharVars.attackDmgType.Blade;
        attack2.attacksCount = 2;
        attack2.attackDmg_base = 4;
        attack2.attackDmg_cur = attack2.attackDmg_base;
        attacks.Add(attack2);
    }
}
