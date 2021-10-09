using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersData
{
    public int currentCharactersCount = 9; // +1 here

    public void Create_Character(Hex hex, BattlePlayer owner, Character character)
    {
        Vector3 position = hex.transform.position;

        switch (character.id)
        {
            case 1:
                GameObject swordman_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/Swordman/Swordman", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character.go = swordman_Obj;
                character.tr = swordman_Obj.transform;
            break;
            
            case 2:
                GameObject spearman_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/Spearman/Spearman", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character.go = spearman_Obj;
                character.tr = spearman_Obj.transform;
            break;
            
            case 3:
                GameObject knight_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/Knight/Knight", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character.go = knight_Obj;
                character.tr = knight_Obj.transform;
            break;

            case 4:
                GameObject gryphon_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/Gryphon/Gryphon", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character.go = gryphon_Obj;
                character.tr = gryphon_Obj.transform;
            break;

            case 5:
                GameObject wolf_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/Wolf/Wolf", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character.go = wolf_Obj;
                character.tr = wolf_Obj.transform;
            break;

            case 6:
                GameObject shadow_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/Shadow/Shadow", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character.go = shadow_Obj;
                character.tr = shadow_Obj.transform;
            break;

            case 7:
                GameObject goblinLeader_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/GoblinLeader/GoblinLeader", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character.go = goblinLeader_Obj;
                character.tr = goblinLeader_Obj.transform;
            break;

            case 8:
                GameObject goblinArcher_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/GoblinArcher/GoblinArcher", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character.go = goblinArcher_Obj;
                character.tr = goblinArcher_Obj.transform;
            break;
        }

        character.hex = hex;
        character.owner = owner;

        hex.character = character;

        Set_CharacterVisual(character);
        Set_CharacterIcons(character);
        Set_CharacterColor(character);
    }

    public Character Get_Character_ById(int id)
    {
        switch(id)
        {
            case 1:
                return new Swordman();
            case 2:
                return new Spearman();
            case 3:
                return new Knight();
            case 4:
                return new Gryphon();
            case 5:
                return new Wolf();
            case 6:
                return new Shadow();
            case 7:
                return new GoblinLeader();
            case 8:
                return new GoblinArcher();
            default:
                return new Swordman();
        }
    }

    public Sprite Get_CharacterImage_ById(int id)
    {
        switch(id)
        {
            case 1:
                return Resources.Load<Sprite>("Characters/Swordman/Swordman_ii");
            case 2:
                return Resources.Load<Sprite>("Characters/Spearman/Spearman_ii");
            case 3:
                return Resources.Load<Sprite>("Characters/Knight/Knight_ii");
            case 4:
                return Resources.Load<Sprite>("Characters/Gryphon/Gryphon_ii");
            case 5:
                return Resources.Load<Sprite>("Characters/Wolf/Wolf_ii");
            case 6:
                return Resources.Load<Sprite>("Characters/Shadow/Shadow_ii");
            case 7:
                return Resources.Load<Sprite>("Characters/GoblinLeader/GoblinLeader_ii");
            case 8:
                return Resources.Load<Sprite>("Characters/GoblinArcher/GoblinArcher_ii");
            default:
                return Resources.Load<Sprite>("Characters/Swordman/Swordman_ii");
        }
    }

    public string Get_Menu_Character_Tooltip(Character character)
    {
        string result = character.name;
        result += "\nCost : " + character.acc_cost;
        result += "\nSummon cost : " + character.ingame_cost;
        result += "\nHealth : " + character.health.hp_cur + "/" + character.health.hp_max;
        result += "\nDodge : " + character.defence.dodgeChance;
        result += "\nMove points : " + character.movement.movePoints_max;
        result += "\nAttack : " + Get_CharacterAttacks_Tooltip(character);

        return result;
    }

    public string Get_Ingame_Character_Tooltip(Character character)
    {
        string result = character.name;
        result += "\nHealth : " + character.health.hp_cur + "/" + character.health.hp_max;
        result += "\nDodge : " + character.defence.dodgeChance + "+" + character.hex.dodge;
        if(character.canAct) result += "\nCan act : yes";
        else result += "\nCan act : no";
        result += "\nMove points : " + character.movement.movePoints_cur + "/" + character.movement.movePoints_max;
        result += "\nAttack : " + Get_CharacterAttacks_Tooltip(character);

        return result;
    }

    private string Get_CharacterAttacks_Tooltip(Character character)
    {
        string result = "";
        for(int x = 0; x < character.attacks.Count; x++)
        {
            CharVars.char_Attack att = character.attacks[x];
            result += "\n  - " + att.attackType + ", " + att.attackDmgType + ", " + att.attacksCount + "x" + att.attackDmg_base;
        }
        return result;
    }

    private void Set_CharacterVisual(Character character)
    {
        character.aController = character.go.GetComponent<AnimationController>();
        character.aSpriteRenderers = new List<SpriteRenderer>();
        character.aSpriteRenderers.Add(character.aController.aIdleLeft_go.GetComponent<SpriteRenderer>());
        character.aSpriteRenderers.Add(character.aController.aIdleRight_go.GetComponent<SpriteRenderer>());
        character.aSpriteRenderers.Add(character.aController.aAttackLeft_go.GetComponent<SpriteRenderer>());
        character.aSpriteRenderers.Add(character.aController.aAttackRight_go.GetComponent<SpriteRenderer>());
        character.aSpriteRenderers.Add(character.aController.aCastLeft_go.GetComponent<SpriteRenderer>());
        character.aSpriteRenderers.Add(character.aController.aCastRight_go.GetComponent<SpriteRenderer>());
        character.aSpriteRenderers.Add(character.aController.aDeathLeft_go.GetComponent<SpriteRenderer>());
        character.aSpriteRenderers.Add(character.aController.aDeathRight_go.GetComponent<SpriteRenderer>());

        character.Set_Visual();
    }

    private void Set_CharacterIcons(Character character)
    {
        if(character.owner.heroCharacter != character)
            character.tr.Find("Crown").gameObject.SetActive(false);
    }

    private void Set_CharacterColor(Character character)
    {
        SceneMain sceneMain = GameData.inst.gameMain.sceneMain;

        SpriteRenderer spriteRenderer = character.tr.Find("Shadow").GetComponent<SpriteRenderer>();
        Color color = Color.white;
        for(int x = 0; x < sceneMain.battlePlayers_List.Count; x++)
        {
            BattlePlayer someBattlePlayer = sceneMain.battlePlayers_List[x];
            if(character.owner != someBattlePlayer) continue;

            switch (x)
            {
                case 0:
                    color = Color.red;
                break;
                case 1:
                    color = Color.blue;
                break;
                case 2:
                    color = Color.green;
                break;
                case 3:
                    color = Color.yellow;
                break;
            }
            color.a = 0.5f;
            spriteRenderer.color = color;
        }
    }
}
