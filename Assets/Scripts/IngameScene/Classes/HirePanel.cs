using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HirePanel
{
    private SceneMain_UI sceneUI;
    private SceneMain sceneMain;
    public GameObject canvas_Go;
    
    public Text accGold_Text;
    public GameObject hButton_Content;
    private Button confirm_Button;
    private Button cancel_Button;

    private List<HireButton> hireButtons = new List<HireButton>();

    private Hex hex;
    private Character selected_Character;

    private Image selected_Character_Image;
    private Text selected_Character_Text;

    public HirePanel(SceneMain_UI sceneUI, SceneMain sceneMain)
    {
        this.sceneUI = sceneUI;
        this.sceneMain = sceneMain;
        canvas_Go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Hire_Canvas", typeof(GameObject))) as GameObject;

        accGold_Text = canvas_Go.transform.Find("Panel").transform.Find("AccGold_Text").GetComponent<Text>();
        hButton_Content = canvas_Go.transform.Find("Panel").transform.Find("Hire_ScrollView").transform.Find("Viewport").transform.Find("Content").gameObject;

        selected_Character_Image = canvas_Go.transform.Find("Panel").transform.Find("Selected_Char_Image").GetComponent<Image>();
        selected_Character_Text = canvas_Go.transform.Find("Panel").transform.Find("Selected_Char_Text").GetComponent<Text>();
        selected_Character_Image.gameObject.SetActive(false);
        selected_Character_Text.gameObject.SetActive(false);

        confirm_Button = canvas_Go.transform.Find("Panel").transform.Find("Confirm_Button").GetComponent<Button>();
        confirm_Button.onClick.AddListener(Confirm_Hire);
        
        cancel_Button = canvas_Go.transform.Find("Panel").transform.Find("Cancel_Button").GetComponent<Button>();
        cancel_Button.onClick.AddListener(Hide);

        Hide();
    }

    public void Show(Hex hex)
    {
        if(!canvas_Go.activeInHierarchy) canvas_Go.SetActive(true);
        sceneUI.mouseOverUI = true;

        // Clear
        hireButtons.Clear();
        foreach (Transform child in hButton_Content.transform)
            GameObject.Destroy(child.gameObject);

        accGold_Text.text = "Player gold : " + GameData.inst.account.acc_gold;

        for(int x = 0; x < sceneMain.myBPlayer.availableCharacters.Count; x++)
        {
            Character character = sceneMain.myBPlayer.availableCharacters[x];
            HireButton hButton = new HireButton(this, x, character);
            hireButtons.Add(hButton);
        }

        confirm_Button.interactable = false;
        this.hex = hex;
    }

    public void SelectCharacter(int id)
    {
        for(int x = 0; x < hireButtons.Count; x++)
        {
            Button hButton = hireButtons[x].button;
            if(x == id)
                hButton.interactable = false;
            else
                hButton.interactable = true;
        }

        selected_Character = sceneMain.myBPlayer.availableCharacters[id];

        selected_Character_Image.gameObject.SetActive(true);
        selected_Character_Text.gameObject.SetActive(true);

        selected_Character_Image.sprite = selected_Character.image;
        selected_Character_Text.text = selected_Character.name + "\n\n" +
        "Cost : " + selected_Character.ingame_cost + "\n\n" +
        "Health : " + selected_Character.health.hp_max + "\n" +
        "Movement : " + selected_Character.movement.movePoints_max + "\n" +
        "Attack : " + Get_CharacterAttacks(selected_Character);

        if(GameData.inst.account.acc_gold >= selected_Character.ingame_cost) confirm_Button.interactable = true;
    }

    private void Confirm_Hire()
    {
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;

        if(server != null)
        {
            GameData.inst.gameMain.On_Hire(this.hex, sceneMain.myBPlayer, selected_Character);
        }
        else
        {
            GameData.inst.gameMain.Request_Hire(this.hex, sceneMain.myBPlayer, selected_Character);
        }

        Hide();
    }

    public void Hide()
    {
        if(canvas_Go.activeInHierarchy) canvas_Go.SetActive(false);
        sceneUI.mouseOverUI = false;
    }

    private string Get_CharacterAttacks(Character character)
    {
        string result = "";
        for(int x = 0; x < character.attacks.Count; x++)
        {
            CharVars.char_Attack att = character.attacks[x];
            result += "\n  - " + att.attackType + ", " + att.attackDmgType + ", " + att.attacksCount + "x" + att.attackDmg_base;
        }
        return result;
    }
}
