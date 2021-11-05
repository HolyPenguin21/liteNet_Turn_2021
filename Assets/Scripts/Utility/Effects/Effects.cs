using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Effects
{
    public static void Dmg_Red(Hex hex, int dmg)
    {
        GameObject effect_go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Effects/Damage_Red", typeof(GameObject))) as GameObject;
        Debug.Log(hex);
        effect_go.transform.position = hex.tr.position;

        DamageEffect damageEffect = effect_go.GetComponent<DamageEffect>();
        damageEffect.dmg = dmg;
    }
    public static void Dmg_Blue(Hex hex, int dmg)
    {
        GameObject effect_go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Effects/Damage_Blue", typeof(GameObject))) as GameObject;
        Debug.Log(hex);
        effect_go.transform.position = hex.tr.position;

        DamageEffect damageEffect = effect_go.GetComponent<DamageEffect>();
        damageEffect.dmg = dmg;
    }

    public static void Miss(Hex hex, int dmg)
    {
        GameObject effect_go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Effects/Miss", typeof(GameObject))) as GameObject;
        Debug.Log(hex);
        effect_go.transform.position = hex.tr.position;
    }
}
