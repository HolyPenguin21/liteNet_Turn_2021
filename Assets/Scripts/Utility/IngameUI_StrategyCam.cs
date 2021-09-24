using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI_StrategyCam : MonoBehaviour
{
    public float camMoveSpeed = 0.05f;
    public float[] BoundsX = new float[] { -4f, 10f };
    public float[] BoundsY = new float[] { -4f, 4f };

    private Camera s_Camera;
    private IngameUI_Camera IngameUI_Camera;

    private void Awake()
    {
        s_Camera = GetComponent<Camera>();
        IngameUI_Camera = GetComponent<IngameUI_Camera>();
    }

    void LateUpdate()
    {
        if (!GameData.inst.inputPc) return;

        Vector3 curPos = transform.position;
        curPos += new Vector3(Input.GetAxis("Horizontal") * camMoveSpeed, Input.GetAxis("Vertical") * camMoveSpeed, 0);

        curPos.x = Mathf.Clamp(curPos.x, BoundsX[0], BoundsX[1]);
        curPos.y = Mathf.Clamp(curPos.y, BoundsY[0], BoundsY[1]);
        transform.position = curPos;
    }
}
