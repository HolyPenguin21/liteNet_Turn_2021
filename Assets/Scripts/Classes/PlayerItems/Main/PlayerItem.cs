using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerItem
{
    public int id;
    public string name;
    public Sprite image;

    public abstract void Effect();
}
