using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersData
{
    public void Create_Character(Hex hex, int cId, BattlePlayer owner)
    {
        Vector3 position = hex.transform.position;
        Character character = null;

        switch (cId)
        {
            case 1:
                GameObject swordman_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/Swordman/Swordman", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character = new Swordman(swordman_Obj, hex, owner);
            break;
            case 2:
                GameObject spearman_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/Spearman/Spearman", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character = new Spearman(spearman_Obj, hex, owner);
            break;
            case 3:
                GameObject knight_Obj = MonoBehaviour.Instantiate(
                    Resources.Load("Characters/Knight/Knight", typeof(GameObject)), position, Quaternion.identity) as GameObject;
                character = new Knight(knight_Obj, hex, owner);
            break;
        }

        character.hex = hex;
        character.owner = owner;

        hex.character = character;
    }

    public Character Get_Character_ById(int id)
    {
        switch(id)
        {
            case 1:
                return new Swordman(null, null, null);
            case 2:
                return new Spearman(null, null, null);
            case 3:
                return new Knight(null, null, null);
            default:
                return new Swordman(null, null, null);
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
            default:
                return Resources.Load<Sprite>("Characters/Swordman/Swordman_ii");
        }
    }
}
