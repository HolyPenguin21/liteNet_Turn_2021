using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LifetimeBuff
{
    public int id;
    public string name;
    public string description;

    public abstract void Effect(Character character);
}
