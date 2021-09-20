using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharVars
{
    public enum attackType { none, Melee, Ranged };
    public enum attackDmgType { Blade, Pierce, Impact, Magic };

    [System.Serializable]
    public struct char_Move
    {
        public int movePoints_cur;
        public int movePoints_max;
    }

    [System.Serializable]
    public struct char_Attack
    {
        public attackType attackType;
        public attackDmgType attackDmgType;
        public int attacksCount;
        public int attackDmg_base;
        public int attackDmg_cur;
    }
}
