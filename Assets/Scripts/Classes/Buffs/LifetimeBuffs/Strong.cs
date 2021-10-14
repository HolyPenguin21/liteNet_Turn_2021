using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strong : LifetimeBuff
{
    public Strong()
    {
        this.id = 1;
        this.name = "Strong";
        this.description = "Strong description ...";
    }

    public override void Effect(Character character)
    {
        int hpBonus = (character.health.hp_max * 10) / 100;
        character.health.hp_max += hpBonus;
        character.health.hp_cur += hpBonus;

        for(int x = 0; x < character.attacks.Count; x++)
        {
            CharAttack attack = character.attacks[x];
            attack.attackDmg += 1;
        }
    }
}
