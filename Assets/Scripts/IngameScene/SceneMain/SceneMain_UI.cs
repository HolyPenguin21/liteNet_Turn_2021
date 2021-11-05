using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMain_UI : MonoBehaviour
{
    private Server server;
    private GameMain gm;
    private SceneMain sceneMain;
    private ClickHandler clickHandler;
    public Pathfinding pathfinding;

    public HexInfoPanel hexInfoPanel;
    public AttackPanel attackPanel;
    public HirePanel hirePanel;
    public WinLosePanel winLosePanel;

    private LineRenderer pathVisual;
    private Transform hover_effect;
    private SpriteRenderer hover_default;
    private SpriteRenderer hover_attack;
    private SpriteRenderer hover_move;
    private Transform selected_effect;

    // Mouse input
    public bool mouseOverUI = false;
    private Camera scene_Camera;
    private Ray mouseRay;
    private RaycastHit mouseHit;

    public Hex hover_Hex;
    public Hex selected_Hex;
    public Character selected_Character;

    private void Awake()
    {
        scene_Camera = Camera.main;
        sceneMain = GetComponent<SceneMain>();
        gm = GameData.inst.gameMain;
        gm.sceneMain_ui = this;

        clickHandler = new ClickHandler(this);
        hexInfoPanel = new HexInfoPanel(this);
        attackPanel = new AttackPanel(this);
        hirePanel = new HirePanel(this, sceneMain);
        winLosePanel = new WinLosePanel(this, sceneMain);
    }

    void Start()
    {
        server = GameData.inst.server;

        pathVisual = transform.Find("Path_Line").GetComponent<LineRenderer>();
        hover_effect = transform.Find("HoverEffect");
        hover_default = hover_effect.Find("Default").GetComponent<SpriteRenderer>();
        hover_attack = hover_effect.Find("Attack").GetComponent<SpriteRenderer>();
        hover_move = hover_effect.Find("Move").GetComponent<SpriteRenderer>();
        selected_effect = transform.Find("SelectedEffect");

        pathfinding = new Pathfinding(pathVisual);
        pathfinding.Hide_Path();
    }

    void Update()
    {
        clickHandler.Update();

        Mouse_Hover_Input();
    }

    private void Mouse_Hover_Input()
    {
        if(mouseOverUI) return;

        Hex someHex = HittedObject();
        if (!IsHittingSomething(someHex)) return;
        if (someHex == hover_Hex) return;

        Set_Hover(someHex);
        Path_Display();
    }

    public void Input_SingleClick()
    {
        if (mouseOverUI) return;

        Hex clicked_Hex = HittedObject();
        if (!IsHittingSomething(clicked_Hex)) return;

        if(selected_Hex == null)
        {
           SelectHex(clicked_Hex);
           return;
        }

        Character selected_Character = selected_Hex.character;
        if (selected_Character == null)
        {
            SelectHex(clicked_Hex);
            return;
        }

        if (Utility.IsMyCharacter(selected_Character))
        {
            if(clicked_Hex.character != null) { // there is character in clicked hex
                if(!Utility.IsMyCharacter(clicked_Hex.character))
                {
                    if(selected_Character.canAct)
                    {
                        gm.On_Attack(selected_Hex, clicked_Hex);
                        pathfinding.Hide_Path();
                    }
                    else
                    {
                        SelectHex(clicked_Hex);
                        return;
                    }
                }
                else
                {
                    SelectHex(clicked_Hex);
                    return;
                }
            }
            else { // clicked hex is empty
                if(selected_Character.movement.movePoints_cur > 0)
                {
                    if(server != null) gm.On_Move(selected_Hex, clicked_Hex);
                    else gm.Request_Move(selected_Hex, clicked_Hex);
                    pathfinding.Hide_Path();
                    return;
                }
                else
                {
                    SelectHex(clicked_Hex);
                    return;
                }
            }
        }
        else
        {
            SelectHex(clicked_Hex);
            return;
        }
    }

    public void Input_Hold()
    {
        if (mouseOverUI) return;

        if(sceneMain.currentTurn != sceneMain.battlePlayer) return;

        Hex clicked_Hex = HittedObject();
        if (!IsHittingSomething(clicked_Hex)) return;

        if(clicked_Hex.rootCastle == null || clicked_Hex.character != null) return;
        if(clicked_Hex.rootCastle.character == null || !Utility.IsMyCharacter(clicked_Hex.rootCastle.character)) return;

        hirePanel.Show(clicked_Hex);
    }

    #region Other
    public void SelectHex(Hex hex)
    {
        selected_Hex = hex;

        selected_effect.gameObject.SetActive(true);
        selected_effect.position = selected_Hex.transform.position;

        hexInfoPanel.Show_HexInfo(selected_Hex);

        if(selected_Hex.character != null)
            hexInfoPanel.Show_CharacterInfo(selected_Hex.character);
        else
            hexInfoPanel.Hide_CharacterInfo();
    }

    private void Path_Display()
    {
        if(!hover_Hex.groundMove ||
        selected_Hex == null ||
        selected_Hex.character == null ||
        !Utility.IsMyCharacter(selected_Hex.character)) {
            pathfinding.Hide_Path();
            return;
        }

        pathfinding.Show_RealPath(selected_Hex.character, selected_Hex, hover_Hex);
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

        hexInfoPanel.Hide_All();

        // Set_HoverImage(0);
        // GameObject.Find("UI").GetComponent<UI_Ingame>().Hide_HexInfo();

        // GameObject.Find("UI").GetComponent<UI_Ingame>().Button_Cancel_Spell();

        // mouseOverUI = false;

        // GameMain.inst.fog.UpdateFog_PlayerView();
    }
    #endregion

    #region Checkers
    private Hex HittedObject()
    {
        mouseRay = scene_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out mouseHit, 50.0f))
        {
            if (mouseHit.collider.CompareTag("Hex"))
                return Utility.Get_Hex_ByTransform(mouseHit.collider.transform);
        }

        return null;
    }

    private bool IsHittingSomething(Hex someHex)
    {
        if (someHex == null)
        {
            Reset_Hover();
            pathfinding.Hide_Path();
            return false;
        }
        return true;
    }
    #endregion
}
