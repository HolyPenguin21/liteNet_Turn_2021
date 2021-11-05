using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharAttack
{
    public CharVars.attackType attackType;
    public CharVars.attackDmgType attackDmgType;

    public int attacksCount_base;
    public int attacksCount_cur;

    public int attackDmg_base;
    public int attackDmg_cur;

    public CharAttack(CharVars.attackType attackType, CharVars.attackDmgType attackDmgType, int attacksCount, int attackDmg)
    {
        this.attackType = attackType;
        this.attackDmgType = attackDmgType;

        this.attacksCount_base = attacksCount;
        this.attacksCount_cur = this.attacksCount_base;

        this.attackDmg_base = attackDmg;
        this.attackDmg_cur = this.attackDmg_base;
    }

    public void Reset()
    {
        this.attacksCount_cur = this.attacksCount_base;
        this.attackDmg_cur = this.attackDmg_base;
    }
}
