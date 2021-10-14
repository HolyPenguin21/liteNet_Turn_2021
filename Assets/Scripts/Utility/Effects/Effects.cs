using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Effects
{
    public static void Dmg(Hex hex, int dmg, bool attacker)
    {
        GameObject effect_go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Effects/Damage", typeof(GameObject))) as GameObject;
        effect_go.transform.position = hex.tr.position;

        DamageEffect damageEffect = effect_go.GetComponent<DamageEffect>();
        damageEffect.dmg = dmg;
        damageEffect.attacker = attacker;
    }

    public static void Miss(Hex hex, int dmg)
    {
        GameObject effect_go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Effects/Miss", typeof(GameObject))) as GameObject;
        effect_go.transform.position = hex.tr.position;
    }
}
