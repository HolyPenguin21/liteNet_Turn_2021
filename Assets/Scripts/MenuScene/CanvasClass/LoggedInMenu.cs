using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoggedInMenu : UI_Menu_Canvas
{
    public Text pName_Text;
    public Text selected_pHero_Text;
    public Text selected_pHero_Characters_Text;

    public LoggedInMenu (GameObject gameObject)
    {
        go = gameObject;
        this.pName_Text = go.transform.Find("Panel").transform.Find("pName_Text").GetComponent<Text>();
        this.selected_pHero_Text = go.transform.Find("Panel").transform.Find("selected_pHero_Text").GetComponent<Text>();
        this.selected_pHero_Characters_Text = go.transform.Find("Panel").transform.Find("selected_pHero_Characters_Text").GetComponent<Text>();
    }

    public void Update_PlayerInfo()
    {
        Account p = GameData.inst.account;

        pName_Text.text = p.name;

        if(p.heroes.Count != 0)
        {
            Hero h = p.heroes[p.battleHeroId];
            selected_pHero_Text.text ="Selected hero :\n - " + h.name;

            selected_pHero_Characters_Text.text = "Selected characters :\n";
            for(int x = 0; x < h.battleCharacters.Count; x++)
            {
                selected_pHero_Characters_Text.text += " - " + h.battleCharacters[x].cName + "\n";
            }
            selected_pHero_Characters_Text.text = selected_pHero_Characters_Text.text.Remove(selected_pHero_Characters_Text.text.Length - 1);
        }
        else
        {
            selected_pHero_Text.text = "Selected hero :\n - Not selected";
            selected_pHero_Characters_Text.text = "Selected characters :\n - Not selected";
        }
    }
}
