using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSolo : AiBehaviour
{
    private AttackCalculation attackCalculation = new AttackCalculation();

    public AiSolo(SceneMain sceneMain, BattlePlayer aiBattlePlayer)
    {
        this.sceneMain = sceneMain;
        this.sceneMain_ui = sceneMain.gameObject.GetComponent<SceneMain_UI>();
        this.aiBattlePlayer = aiBattlePlayer;
    }

    public override IEnumerator AITurn()
    {
        if(aiBattlePlayer.hero == null)
        {
            Create_AiHero(Get_StartPoint());
        }
        else
        {
            yield return AiAction();
        }

        sceneMain.Button_EndTurn();
        yield return null;
    }

    private IEnumerator AiAction()
    {
        // Movement / Attack
        for (int x = 0; x < aiBattlePlayer.ingameCharacters.Count; x++)
        {
            Character aiChar = aiBattlePlayer.ingameCharacters[x];
            if (!aiChar.canAct) yield break;

            yield return Move_ToRandomEnemyInRange(aiChar);
            while(aiInAction) yield return null;

            yield return Attack_NearbyEnemy(aiChar);
            while(aiInAction) yield return null;
            // if (aiChar == null) continue;
        }
        yield return null;
    }

    private IEnumerator Attack_NearbyEnemy(Character character)
    {
        aiInAction = true;
        Hex t_Hex = Get_NearbyEnemyHex(character);
        if (t_Hex == null)
        {
            aiInAction = false;
            yield break;
        }

        Hex a_Hex = character.hex;
        CharVars.char_Attack a_Attack = attackCalculation.Get_MaxDmgAttack(character, t_Hex.character);
        int a_AttackId = attackCalculation.Get_AttackId(character, a_Attack);
        Debug.Log(a_AttackId);

        CharVars.char_Attack t_Attack = attackCalculation.Get_ReturnAttack(character, a_AttackId, t_Hex.character);
        int t_AttackId = attackCalculation.Get_AttackId(t_Hex.character, t_Attack);

        GameData.inst.gameMain.Order_Attack(a_Hex, a_AttackId, t_Hex, t_AttackId);

        yield return null;
    }

    private IEnumerator Move_ToRandomEnemyInRange(Character character)
    {
        aiInAction = true;
        if (!character.canAct || character.movement.movePoints_cur == 0) 
        {
            aiInAction = false;
            yield break;
        }

        List<Hex> enemysInRange = Get_Enemys_InRange(character);
        if (enemysInRange.Count == 0)
        {
            aiInAction = false;
            yield break;
        }

        Hex enemyHex = enemysInRange[Random.Range(0, enemysInRange.Count)];
        List<Hex> path = sceneMain_ui.pathfinding.Get_Path(character.hex, enemyHex);
        if(path == null || path.Count < 2)
        {
            aiInAction = false;
            yield break;
        }

        path.RemoveAt(path.Count - 1);

        GameData.inst.gameMain.On_Move(character.hex, path[path.Count - 1]);
    }

    private List<Hex> Get_Enemys_InRange(Character aiChar)
    {
        List<Hex> enemyHexes = new List<Hex>();

        for (int x = 0; x < sceneMain.battlePlayers.Count; x++)
        {
            BattlePlayer battlePlayer = sceneMain.battlePlayers[x];
            if(battlePlayer == aiBattlePlayer) continue;

            for(int y = 0; y < battlePlayer.ingameCharacters.Count; y++)
            {
                Character enemyCharacter = battlePlayer.ingameCharacters[y];

                List<Hex> generalPath = sceneMain_ui.pathfinding.Get_Path(aiChar.hex, enemyCharacter.hex);
                if(generalPath == null || generalPath.Count == 0) continue;
                int pathCost = aiChar.movement.movePoints_cur - sceneMain_ui.pathfinding.Get_PathCost(aiChar, generalPath);

                if (pathCost > aiChar.movement.movePoints_cur) continue;
                enemyHexes.Add(enemyCharacter.hex);
            }
        }

        return enemyHexes;
    }

    private Hex Get_NearbyEnemyHex(Character character)
    {
        List<Hex> hexesWithEnemy = new List<Hex>();
        Hex curHex = character.hex;

        for (int x = 0; x < curHex.neighbors.Count; x++)
        {
            Hex nextHex = curHex.neighbors[x];
            if (nextHex.character != null && character.owner != nextHex.character.owner)
                hexesWithEnemy.Add(nextHex);
        }

        if (hexesWithEnemy.Count > 0)
            return hexesWithEnemy[Random.Range(0, hexesWithEnemy.Count)];
        else
            return null;
    }

    private Hex Get_StartPoint()
    {
        Hex startPoint = sceneMain.startPoints[Random.Range(0, sceneMain.startPoints.Count)];

        if(startPoint == null)
        startPoint = sceneMain.bossSpawners[Random.Range(0, sceneMain.bossSpawners.Count)];

        return startPoint;
    }

    private void Create_AiHero(Hex hex)
    {
        CharactersData charactersData = new CharactersData();
        int randCharacterId = Random.Range(1, 7); // game characters count, edit in future

        Character character = charactersData.Get_Character_ById(randCharacterId);
        GameData.inst.gameMain.Order_CreateAICharacter(hex, aiBattlePlayer, character, 1);
    }
}
