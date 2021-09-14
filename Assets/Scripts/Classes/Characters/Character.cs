using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    // Editor vars
    public GameObject go;
    public Transform tr;

    // Ingame vars
    public Hex hex;
    public BattlePlayer owner;

    // Character specific vars
    public int cId;
    public int cCost;
    public Sprite cImage;
    public string cName;
}
