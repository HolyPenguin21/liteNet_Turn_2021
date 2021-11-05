using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharHealth
{
    public int hp_base;
    public int hp_max;
    public int hp_cur;

    public CharHealth(int hp_base)
    {
        this.hp_base = hp_base;
        this.hp_max = this.hp_base;
        this.hp_cur = this.hp_max;
    }

    public void Reset()
    {
        this.hp_max = this.hp_base;
        this.hp_cur = this.hp_max;
    }
}
