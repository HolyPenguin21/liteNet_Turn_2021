using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_Menu_Canvas
{
    public GameObject go;

    public void Show()
    {
        go.SetActive(true);
    }

    public void Hide()
    {
        go.SetActive(false);
    }
}
