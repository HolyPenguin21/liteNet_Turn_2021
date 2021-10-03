using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSolo : AiBehaviour
{
    AttackOrder attackOrder = new AttackOrder();

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
            // if (aiChar == null) continue;
        }
        yield return null;
    }

    private IEnumerator Attack_NearbyEnemy(Character character)
    {
        Hex enemyHex = Get_NearbyEnemyHex(character);
        if (enemyHex == null) yield break;

        int a_attackId = 0;
        int curMaxDmg = 0;
        int t_attackId = 0;
        for (int x = 0; x < character.attacks.Count; x++)
        {
            CharVars.char_Attack a_attack = character.attacks[x];
            int maxDmg = a_attack.attacksCount * a_attack.attackDmg_base;
            if (maxDmg > curMaxDmg)
            {
                curMaxDmg = maxDmg;
                a_attackId = x;
            }

            CharVars.char_Attack t_attack = Get_TargetAttack(character, a_attack, enemyHex.character);
            t_attackId = Get_TargetAttackId(enemyHex.character, t_attack);
        }

        GameData.inst.gameMain.Order_Attack(character.hex, a_attackId, enemyHex, t_attackId);

        yield return null;
    }

    private CharVars.char_Attack Get_TargetAttack(Character a_Character, CharVars.char_Attack a_Attack, Character t_Character)
    {        
        List<CharVars.char_Attack> t_Attack_List = new List<CharVars.char_Attack>();
        for(int x = 0; x < t_Character.attacks.Count; x++)
            if(t_Character.attacks[x].attackType == a_Attack.attackType)
                t_Attack_List.Add(t_Character.attacks[x]);

        CharVars.char_Attack t_Attack = new CharVars.char_Attack();
        t_Attack.attackType = CharVars.attackType.none;

        for(int x = 0; x < t_Attack_List.Count; x++)
        {
            int dmgCur = attackOrder.DmgCalculation(t_Attack, a_Character);
            if(dmgCur < attackOrder.DmgCalculation(t_Attack_List[x], a_Character))
                t_Attack = t_Attack_List[x];
        }

        return t_Attack;
    }

    private int Get_TargetAttackId(Character character, CharVars.char_Attack attack)
    {
        for(int x = 0; x < character.attacks.Count; x++)
            if(character.attacks[x].attackType == attack.attackType && character.attacks[x].attackDmg_cur == attack.attackDmg_cur)
                return x;

        return -1;
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
