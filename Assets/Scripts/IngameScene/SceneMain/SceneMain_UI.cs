using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMain_UI : MonoBehaviour
{
    private SceneMain sm;
    private ClickHandler clickHandler;

    private Transform hover_effect;
    private SpriteRenderer hover_default;
    private SpriteRenderer hover_attack;
    private SpriteRenderer hover_move;
    private Transform selected_effect;

    // Mouse input
    private Camera scene_Camera;
    private Ray mouseRay;
    private RaycastHit mouseHit;

    public Hex hover_Hex;
    public Hex selected_Hex;
    public Character selected_Character;

    private void Awake()
    {
        scene_Camera = Camera.main;
        sm = GetComponent<SceneMain>();
        clickHandler = new ClickHandler(this);
    }

    void Start()
    {
        hover_effect = transform.Find("HoverEffect");
        hover_default = hover_effect.Find("Default").GetComponent<SpriteRenderer>();
        hover_attack = hover_effect.Find("Attack").GetComponent<SpriteRenderer>();
        hover_move = hover_effect.Find("Move").GetComponent<SpriteRenderer>();
        selected_effect = transform.Find("SelectedEffect");
    }

    void Update()
    {
        clickHandler.Update();
        Mouse_Constant_Input();
    }

    private void Mouse_Constant_Input()
    {
        Hex someHex = HittedObject();
        if (someHex == null)
        {
            Reset_Hover();
            return;
        }

        if (someHex == hover_Hex) return;
        // pathfinding.Hide_Path();
        Set_Hover(someHex);
    }

    public void OnDoubleClick()
    {
        // if (mouseOverUI) return;
        // if (uI_Ingame.somePanelIsOn) return;
        // if (Utility.IsServer())
        // {
        //     if (!GameMain.inst.server.player.isAvailable) return;
        // }
        // else
        // {
        //     if (!GameMain.inst.client.player.isAvailable) return;
        // }

        Mouse_DoubleClick_Input();
    }

    private void Mouse_DoubleClick_Input()
    {
        Hex clickedHex = HittedObject();
        if (clickedHex == null)
        {
            Reset_All();
            return;
        }

        if (selected_Hex == null || selected_Hex != clickedHex)
        {
            SelectHex(clickedHex);
            return;
        }

        // Character selected_Character = selectedHex.character;
        // if (selectedChar == null)
        // {
        //     SelectHex(clickedHex);
        //     pathfinding.Hide_Path();
        //     return;
        // }
    }

    public void SelectHex(Hex hex)
    {
        selected_Hex = hex;

        selected_effect.gameObject.SetActive(true);
        selected_effect.position = selected_Hex.transform.position;

        // GameObject.Find("UI").GetComponent<UI_Ingame>().Show_HexInfo(selectedHex);

        // if(selectedHex.character != null)
        // {
        //     if(Utility.CharacterIsVisible(selectedHex.character))
        //         GameMain.inst.fog.UpdateFog_CharacterView(selectedHex.character);
        // }
    }

    private Hex HittedObject()
    {
        mouseRay = scene_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out mouseHit, 50.0f))
        {
            if (mouseHit.collider.CompareTag("Hex"))
                return sm.Get_Hex_ByTransform(mouseHit.collider.transform);
        }

        return null;
    }

    private void Set_Hover(Hex h)
    {
        hover_Hex = h;
        if (!hover_effect.gameObject.activeInHierarchy) hover_effect.gameObject.SetActive(true);
        hover_effect.position = hover_Hex.tr.position;
    }
    private void Reset_Hover()
    {
        if (hover_Hex == null) return;

        hover_Hex = null;
        if (hover_effect.gameObject.activeInHierarchy) hover_effect.gameObject.SetActive(false);
    }

    public void Reset_All()
    {
        hover_Hex = null;
        hover_effect.gameObject.SetActive(false);

        selected_Hex = null;
        selected_effect.gameObject.SetActive(false);

        // pathfinding.Hide_Path();

        // Set_HoverImage(0);
        // GameObject.Find("UI").GetComponent<UI_Ingame>().Hide_HexInfo();

        // GameObject.Find("UI").GetComponent<UI_Ingame>().Button_Cancel_Spell();

        // mouseOverUI = false;

        // GameMain.inst.fog.UpdateFog_PlayerView();
    }
}
