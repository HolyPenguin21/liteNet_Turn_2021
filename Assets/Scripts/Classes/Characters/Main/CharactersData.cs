using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersData
{
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
        }

        character.hex = hex;
        character.owner = owner;
        character.Get_Visual();
        character.Set_Visual();

        hex.character = character;
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
            default:
                return Resources.Load<Sprite>("Characters/Swordman/Swordman_ii");
        }
    }
}
