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

        PrepareAI();
    }

    private void PrepareAI()
    {
        switch(sceneMain.eventType)
        {
            case Utility.EventType.none:
                Create_AiHero(Get_StartPoint());
            break;

            case Utility.EventType.wolf:
                Event_Wolf(Get_StartPoint());
            break;

            case Utility.EventType.goblin:
                Event_Goblin(Get_StartPoint());
            break;
        }
    }

    public override IEnumerator AITurn()
    {
        yield return AiAction();

        sceneMain.Button_EndTurn();
        yield return null;
    }

    private IEnumerator AiAction()
    {
        // Characters actions
        for (int x = 0; x < aiBattlePlayer.ingameCharacters.Count; x++)
        {
            Character aiChar = aiBattlePlayer.ingameCharacters[x];
            if (!aiChar.canAct) yield break;

            yield return Move_ToRandomEnemy(aiChar);
            while(aiInAction) yield return null;

            yield return Attack_NearbyEnemy(aiChar);
            if(aiChar == null) aiInAction = false;
            while(aiInAction) yield return null;
            // if (aiChar == null) continue;
        }

        yield return AiHeroAction();
        
        yield return null;
    }

    private IEnumerator AiHeroAction()
    {
        Character aiHero = aiBattlePlayer.heroCharacter;

        if(aiBattlePlayer.availableCharacters.Count > 0)
            yield return Hire_Character(aiHero);

        if (!aiHero.canAct) yield break;

        yield return Move_ToRandomEnemy(aiHero);
        while(aiInAction) yield return null;

        yield return Attack_NearbyEnemy(aiHero);
        while(aiInAction) yield return null;
    }

    #region Ai actions
    private IEnumerator Hire_Character(Character aiHero)
    {
        Hex hireHex = Get_FreeHirePoint(aiHero);
        if(hireHex == null) yield break;

        yield return Move_ToHireHex(aiHero, hireHex);
        while(aiInAction) yield return null;

        if(aiHero.hex != hireHex) yield break;

        for(int x = 0; x < hireHex.neighbors.Count; x++)
        {
            Hex hex = hireHex.neighbors[x];
            if(hex.character != null || hex.rootCastle == null) continue;

            GameData.inst.gameMain.Order_CreateAICharacter(hex, aiBattlePlayer, aiBattlePlayer.availableCharacters[0], 0);
            aiBattlePlayer.availableCharacters.RemoveAt(0);
            break;
        }
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

        CharVars.char_Attack t_Attack = attackCalculation.Get_ReturnAttack(character, a_AttackId, t_Hex.character);
        int t_AttackId = attackCalculation.Get_AttackId(t_Hex.character, t_Attack);

        GameData.inst.gameMain.Order_Attack(a_Hex, a_AttackId, t_Hex, t_AttackId);

        yield return null;
    }

    private IEnumerator Move_ToRandomEnemy(Character character)
    {
        aiInAction = true;
        if (!character.canAct || character.movement.movePoints_cur == 0) 
        {
            aiInAction = false;
            yield break;
        }

        List<Hex> enemysInRange = Get_Enemys(character);
        if (enemysInRange.Count == 0)
        {
            aiInAction = false;
            yield break;
        }

        Hex enemyHex = enemysInRange[Random.Range(0, enemysInRange.Count)];
        if(enemyHex == null)
        {
            aiInAction = false;
            yield break; 
        }
        List<Hex> path = sceneMain_ui.pathfinding.Get_Path(character.hex, enemyHex);
        if(path == null || path.Count < 2)
        {
            aiInAction = false;
            yield break;
        }

        path.RemoveAt(path.Count - 1);

        GameData.inst.gameMain.On_Move(character.hex, path[path.Count - 1]);
    }
    #endregion

    private Hex Get_FreeHirePoint(Character aiHero)
    {
        List<Hex> hireHexes = new List<Hex>();
        for(int x = 0; x < sceneMain.grid.Length; x++)
        {
            Hex hex = sceneMain.grid[x];
            if(!hex.isStartPoint) continue;
            if(hex.character != null && hex.character != aiHero) continue;

            hireHexes.Add(hex);
        }

        if(hireHexes.Count == 0) return null;

        int curMplose = 99;
        Hex closestHireHex = null;
        for(int x = 0; x < hireHexes.Count; x++)
        {
            Hex hireHex = hireHexes[x];

            if(aiHero.hex == hireHex)
            {
                closestHireHex = hireHex;
                break;
            }
            
            List<Hex> generalPath = sceneMain_ui.pathfinding.Get_Path(aiHero.hex, hireHex);
            if(generalPath == null || generalPath.Count == 0) continue;

            List<Hex> realPath = sceneMain_ui.pathfinding.Get_RealPath(aiHero, generalPath);
            if(realPath == null || realPath.Count == 0) continue;

            int pathCost = aiHero.movement.movePoints_cur - sceneMain_ui.pathfinding.Get_PathCost(aiHero, realPath);
            if(pathCost < curMplose)
            {
                curMplose = pathCost;
                closestHireHex = hireHex;
            }
        }

        if(aiHero.hex != closestHireHex)
        {
            List<Hex> generalPath_ = sceneMain_ui.pathfinding.Get_Path(aiHero.hex, closestHireHex);
            if(generalPath_ == null || generalPath_.Count == 0) return null;

            List<Hex> realPath_ = sceneMain_ui.pathfinding.Get_RealPath(aiHero, generalPath_);
            if(realPath_ == null || realPath_.Count == 0) return null;

            int pathCost_ = aiHero.movement.movePoints_cur - sceneMain_ui.pathfinding.Get_PathCost(aiHero, realPath_);
            if(aiHero.movement.movePoints_cur >= pathCost_)
            {
                return closestHireHex;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return closestHireHex;
        }
    }

    private IEnumerator Move_ToHireHex(Character aiHero, Hex hireHex)
    {
        if(aiHero.hex == hireHex) yield break;
        aiInAction = true;

        List<Hex> generalPath = sceneMain_ui.pathfinding.Get_Path(aiHero.hex, hireHex);
        if(generalPath == null || generalPath.Count == 0)
        {
            aiInAction = false;
            yield break;
        }

        List<Hex> realPath = sceneMain_ui.pathfinding.Get_RealPath(aiHero, generalPath);
        if(realPath == null || realPath.Count == 0)
        {
            aiInAction = false;
            yield break;
        }

        GameData.inst.gameMain.On_Move(aiHero.hex, realPath[realPath.Count - 1]);
    }

    private List<Hex> Get_Enemys(Character aiChar)
    {
        List<Hex> enemyHexes = new List<Hex>();

        for (int x = 0; x < sceneMain.battlePlayers_List.Count; x++)
        {
            BattlePlayer battlePlayer = sceneMain.battlePlayers_List[x];
            if(battlePlayer == aiBattlePlayer) continue;

            if(battlePlayer.heroCharacter != null) enemyHexes.Add(battlePlayer.heroCharacter.hex);

            for(int y = 0; y < battlePlayer.ingameCharacters.Count; y++)
            {
                Character enemyCharacter = battlePlayer.ingameCharacters[y];

                List<Hex> generalPath = sceneMain_ui.pathfinding.Get_Path(aiChar.hex, enemyCharacter.hex);
                if(generalPath == null || generalPath.Count == 0) continue;
                
                List<Hex> realPath = sceneMain_ui.pathfinding.Get_RealPath(aiChar, generalPath);
                if(realPath == null || realPath.Count == 0) continue;
                
                int pathCost = aiChar.movement.movePoints_cur - sceneMain_ui.pathfinding.Get_PathCost(aiChar, realPath);

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
        Hex startPoint = null;
        if(sceneMain.startPoints.Count != 0)
            startPoint = sceneMain.startPoints[Random.Range(0, sceneMain.startPoints.Count)];

        if(startPoint == null && sceneMain.bossSpawners.Count != 0)
            startPoint = sceneMain.bossSpawners[Random.Range(0, sceneMain.bossSpawners.Count)];

        return startPoint;
    }

    private void Create_AiHero(Hex hex)
    {
        CharactersData charactersData = new CharactersData();
        int randCharacterId = Random.Range(1, charactersData.currentCharactersCount); // game characters count, edit in future

        Character character = charactersData.Get_Character_ById(randCharacterId);
        GameData.inst.gameMain.Order_CreateAICharacter(hex, aiBattlePlayer, character, 1);

        aiBattlePlayer.availableCharacters.Add(charactersData.Get_Character_ById(1));
        aiBattlePlayer.availableCharacters.Add(charactersData.Get_Character_ById(2));
    }

    private void Event_Wolf(Hex hex)
    {
        CharactersData charactersData = new CharactersData();

        // Hero character
        Character character = charactersData.Get_Character_ById(5);
        GameData.inst.gameMain.Order_CreateAICharacter(hex, aiBattlePlayer, character, 1);

        // Other characters
        for(int x = 0; x < hex.neighbors.Count; x++)
        {
            int rand = UnityEngine.Random.Range(1, 101);
            if(rand < 50) continue;

            Hex crHex = hex.neighbors[x];
            GameData.inst.gameMain.Order_CreateAICharacter(crHex, aiBattlePlayer, character, 0);
        }

        // Rewards
        int randRewardCount = UnityEngine.Random.Range(1, 6);
        for(int x = 0; x < randRewardCount; x++)
        {
            int rarityValue = UnityEngine.Random.Range(1, 101);
            if(rarityValue <= 5)
            {
                int rareDropValue = UnityEngine.Random.Range(4, 5);
                switch (rareDropValue)
                {
                    case 4:
                        sceneMain.rewards.Add(new Token_Forest());
                    break;
                }
            }
            else if(rarityValue > 5 && rarityValue <= 30)
            {
                sceneMain.rewards.Add(new PiWolf());
            }
            else if(rarityValue > 30 && rarityValue <= 100)
            {
                sceneMain.rewards.Add(new Gold());
            }
        }
    }

    private void Event_Goblin(Hex hex)
    {
        CharactersData charactersData = new CharactersData();

        // Hero character
        Character character = charactersData.Get_Character_ById(7);
        GameData.inst.gameMain.Order_CreateAICharacter(hex, aiBattlePlayer, character, 1);

        // Other characters
        for(int x = 0; x < hex.neighbors.Count; x++)
        {
            int rand = UnityEngine.Random.Range(1, 101);
            if(rand < 30) continue;

            character = charactersData.Get_Character_ById(8);
            Hex crHex = hex.neighbors[x];
            GameData.inst.gameMain.Order_CreateAICharacter(crHex, aiBattlePlayer, character, 0);
        }

        // Rewards
        int randRewardCount = UnityEngine.Random.Range(1, 10);
        for(int x = 0; x < randRewardCount; x++)
        {
            int rarityValue = UnityEngine.Random.Range(1, 101);
            if(rarityValue <= 5)
            {
                int rareDropValue = UnityEngine.Random.Range(4, 6);
                switch (rareDropValue)
                {
                    case 4:
                        sceneMain.rewards.Add(new Token_Forest());
                    break;
                    case 5:
                        sceneMain.rewards.Add(new Token_Dark());
                    break;
                }
            }
            else if(rarityValue > 5 && rarityValue <= 30)
            {
                sceneMain.rewards.Add(new PiSpearman());
            }
            else if(rarityValue > 30 && rarityValue <= 100)
            {
                sceneMain.rewards.Add(new Gold());
            }
        }
    }
}
