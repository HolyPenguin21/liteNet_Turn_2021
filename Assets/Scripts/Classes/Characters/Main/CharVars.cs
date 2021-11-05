using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharVars
{
    public enum faction { General, Castle, Forest, Dark};
    public enum attackType { none, Melee, Ranged };
    public enum attackDmgType { none, Blade, Pierce, Impact, Magic };

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
