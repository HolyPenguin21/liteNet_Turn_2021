using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton
{
    private GameObject button_Go;

    public AttackButton (CharVars.char_Attack attacker, CharVars.char_Attack target)
    {
        button_Go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/AttackButton", typeof(GameObject))) as GameObject;
        button_Go.transform.SetParent(GameObject.Find("Attack_Canvas").transform, false);

        button_Go.transform.Find("a_Image").GetComponent<Image>().sprite = Get_DmgTypeImage(attacker.attackDmgType);
        // button_Go.transform.Find("a_Text").text = ;
        button_Go.transform.Find("t_Image").GetComponent<Image>().sprite = Get_DmgTypeImage(target.attackDmgType);
        // button_Go.transform.Find("t_Text").text = ;
    }

    private Sprite Get_DmgTypeImage(CharVars.attackDmgType type)
    {
        switch(type)
        {
            case CharVars.attackDmgType.Blade:
                return Resources.Load("Images/Blade", typeof(Sprite)) as Sprite;

            case CharVars.attackDmgType.Pierce:
                return Resources.Load("Images/Pierce", typeof(Sprite)) as Sprite;

            case CharVars.attackDmgType.Impact:
                return Resources.Load("Images/Impact", typeof(Sprite)) as Sprite;

            case CharVars.attackDmgType.Magic:
                return Resources.Load("Images/Magic", typeof(Sprite)) as Sprite;

            default :
                return Resources.Load("Images/Blade", typeof(Sprite)) as Sprite;
        }
    }
}
