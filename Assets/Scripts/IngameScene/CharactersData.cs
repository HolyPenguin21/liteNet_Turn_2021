using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersData
{
    public IEnumerator Create_Character(Hex hex, int cId, BattlePlayer owner)
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

        yield return null;
    }
}
