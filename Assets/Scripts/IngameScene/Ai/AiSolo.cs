using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSolo : AiBehaviour
{
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

            yield return Move_ToRandomEnemyInRange(aiChar);

            // yield return Attack_NearbyEnemy(aiChar);
            // if (aiChar == null) continue;

            // yield return Attack_NearbyEnemy(aiChar);
        }
        yield return null;
    }

    private IEnumerator Move_ToRandomEnemyInRange(Character character)
    {
        if (!character.canAct || character.movement.movePoints_cur == 0) yield break;

        List<Hex> enemysInRange = Get_Enemys_InRange(character);
        if (enemysInRange.Count == 0) yield break;

        Hex enemyHex = enemysInRange[Random.Range(0, enemysInRange.Count)];
        List<Hex> path = sceneMain_ui.pathfinding.Get_Path(character.hex, enemyHex);
        if(path == null || path.Count < 2) yield break;
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
        int randCharacterId = Random.Range(1, 5); // game characters count, edit in future

        Character character = charactersData.Get_Character_ById(randCharacterId);
        GameData.inst.gameMain.Order_CreateAICharacter(hex, aiBattlePlayer, character, 1);
    }
}
