using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharVars
{
    public enum faction { General, Castle, Forest, Dark};
    public enum attackType { none, Melee, Ranged };
    public enum attackDmgType { Blade, Pierce, Impact, Magic };

    [System.Serializable]
    public struct char_Move
    {
        public int movePoints_cur;
        public int movePoints_max;
        public char_Move(int movePoints_max)
        {
            this.movePoints_max = movePoints_max;
            this.movePoints_cur = 0;
        }
    }

    [System.Serializable]
    public struct char_Hp
    {
        public int hp_cur;
        public int hp_max;

        public char_Hp(int hp_max)
        {
            this.hp_max = hp_max;
            this.hp_cur = hp_max;
        }
    }

    [System.Serializable]
    public struct char_Defence
    {
        public int dodgeChance;
        public float blade_resistance;
        public float pierce_resistance;
        public float impact_resistance;
        public float magic_resistance;
    }
}
