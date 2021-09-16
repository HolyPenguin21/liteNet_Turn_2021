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
    
    public CharVars.char_Move char_Move;

    #region Movement
    public IEnumerator Move(List<Hex> somePath)
	{
        List<Hex> path = new List<Hex>(somePath);
        while (path.Count > 0)
        {
            hex.character = null;
            hex = path[0];
            path[0].character = this;
            yield return ActualMove(path[0]);
            path.RemoveAt(0);
        }

		// if (Utility.IsMyCharacter(this))
		// 	GameMain.inst.ui_Input.SelectHex(hex);

		// if (charMovement.movePoints_cur == 0)
		// 	GameMain.inst.fog.UpdateFog_PlayerView();
	}

	private IEnumerator ActualMove(Hex hexToMove)
	{
		float t2 = 0f;
		while (t2 < 1f)
		{
			t2 += Time.deltaTime * 3;
			t2 = Mathf.Clamp(t2 + Time.deltaTime * 0.01f, 0f, 1f);
			tr.position = Vector3.Lerp(tr.position, hexToMove.transform.position, t2);
			yield return null;
		}

		// GameMain.inst.fog.UpdateFog_PlayerView();
	}
    #endregion
}
