using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMain : MonoBehaviour
{
    private SoloScene soloSc;
    private PVPScene pvpSc;
    private CharactersData cData;

    public Hex[] grid;
    public List<Hex> startPoints = new List<Hex>();
    public List<Hex> bossSpawners = new List<Hex>();

    public List<BattlePlayer> bPlayers = new List<BattlePlayer>();
    public BattlePlayer currentTurn;

    public Utility.GameType gameType;
    public List<PlayerItem> rewards = new List<PlayerItem>();

    private void Awake()
    {
        cData = new CharactersData();
    }

    private IEnumerator Start()
    {
        yield return Setup_Game();

        yield return null;
    }

    private IEnumerator Setup_Game()
    {
        gameType = GameData.inst.gameType;

        if(gameType == Utility.GameType.solo)
        {
            soloSc = new SoloScene(this);
            yield return soloSc.Setup_Players();
            yield return soloSc.Setup_Characters();
        }
        else if(gameType == Utility.GameType.pvp)
        {
            pvpSc = new PVPScene(this);
            yield return pvpSc.Setup_Players();
            yield return pvpSc.Setup_Characters();
        }

        yield return Setup_FirstTurn();
    }

    #region Characters management
    public IEnumerator Create_Character(Hex hex, int cId, BattlePlayer owner)
    {
        yield return cData.Create_Character(hex, cId, owner);
        Debug.Log("Character created");
    }
    #endregion

    #region Turn management
    public IEnumerator Setup_FirstTurn()
    {
        Debug.Log("Setup_FirstTurn");

        if(gameType == Utility.GameType.solo)
        {
            yield return soloSc.Setup_FirstTurn();
        }
        else if(gameType == Utility.GameType.pvp)
        {

        }

        yield return Update_OnTurn();
    }
    public IEnumerator ChangeTurn()
    {
        Debug.Log("ChangeTurn");

        yield return null;
    }
    public IEnumerator Update_OnTurn()
    {
        Debug.Log("Update_OnTurn");

        yield return null;
    }
    #endregion

    #region Other
    public Hex Get_Hex_ByTransform(Transform tr)
    {
        for(int x = 0 ; x < grid.Length; x++)
        {
            Hex h = grid[x];
            if(h.tr == tr)
            {
                return h;
            }
        }

        return null;
    }
    #endregion
}
