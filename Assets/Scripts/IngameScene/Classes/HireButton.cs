using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HireButton
{
    private HirePanel hPanel;
    private GameObject button_Go;
    public Button button;

    private int id;
    private Character character;
    

    public HireButton(HirePanel hPanel, int id, Character character)
    {
        this.hPanel = hPanel;
        this.id = id;
        this.character = character;

        button_Go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Hire_Character_Button", typeof(GameObject))) as GameObject;
        button_Go.name = character.name;
        button_Go.transform.SetParent(hPanel.hButton_Content.transform, false);

        button = button_Go.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        button_Go.transform.Find("char_Image").GetComponent<Image>().sprite = character.image;
        button_Go.transform.Find("char_Text").GetComponent<Text>().text = character.name + " " + character.ingame_cost;
    }

    private void TaskOnClick()
    {
        hPanel.SelectCharacter(this.id);
    }
}
