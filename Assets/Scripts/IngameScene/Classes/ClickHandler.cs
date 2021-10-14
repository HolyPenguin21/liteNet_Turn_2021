using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler
{
    public int clickCount = 0;
    public float doubleClick_timer = 0.0f;
    private float doubleClick_delay = 0.3f;

    public float hold_timer = 0.0f;
    private float holdDelay = 0.5f;
    public bool holdOn = false;

    private SceneMain_UI input;

    public ClickHandler(SceneMain_UI input)
    {
        this.input = input;
    }

    public void Update()
    {
        if (GameData.inst.inputPc)
            Input_Pc();
        else
            Input_Android();
    }

    private void Input_Pc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            input.Reset_All();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            input.Input_SingleClick();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            input.Input_Hold();
        }
    }

    private void Input_Android()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            clickCount++;
            hold_timer = 0.0f;
        }

        if (Input.GetKey(KeyCode.Mouse0))
            hold_timer += Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            hold_timer = 0.0f;
            holdOn = false;
        }

        if (clickCount == 1)
            doubleClick_timer += Time.deltaTime;

        if (clickCount >= 2)
        {
            doubleClick_timer = 0.0f;
            clickCount = 0;
            input.Input_SingleClick();
        }

        if (doubleClick_timer >= doubleClick_delay)
        {
            doubleClick_timer = 0.0f;
            clickCount = 0;

            if (doubleClick_delay == 0.0f && !input.mouseOverUI)
                input.Input_SingleClick();
        }

        if (hold_timer >= holdDelay && !holdOn)
        {
            holdOn = true;
            input.Input_Hold();
        }
    }
}
