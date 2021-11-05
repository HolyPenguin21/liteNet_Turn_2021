using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharMove
{
    public int movePoints_base;
    public int movePoints_max;
    public int movePoints_cur;

    public CharMove(int movePoints_base)
    {
        this.movePoints_base = movePoints_base;
        this.movePoints_max = this.movePoints_base;
        this.movePoints_cur = 0;
    }

    public void Reset()
    {
        this.movePoints_max = this.movePoints_base;
        this.movePoints_cur = 0;
    }
}
