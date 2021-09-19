using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    // Editor vars
    public GameObject go;
    public Transform tr;
    public AnimationController aController;
    public List<SpriteRenderer> aSpriteRenderers;

    // Ingame vars
    public Hex hex;
    public BattlePlayer owner;

    // Character specific vars
    public int id;
    public int cost;
    public Sprite image;
    public string name;
    
    public CharVars.char_Move movement;

    #region Movement
    public IEnumerator Move(List<Hex> somePath)
	{
        List<Hex> path = new List<Hex>(somePath);
        while (path.Count > 0)
        {
            CheckOrientation(path[0]);

            hex.character = null;
            hex = path[0];
            path[0].character = this;

            Set_Visual();
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

    private void CheckOrientation(Hex nextHex)
    {
        if(hex.tr.position.x < nextHex.tr.position.x && aController.aOrientationLeft)
            aController.SwitchAnimOrientation();
        else if(hex.tr.position.x > nextHex.tr.position.x && !aController.aOrientationLeft)
            aController.SwitchAnimOrientation();
    }
    #endregion

    #region Visual
    public void Get_Visual()
    {
        aController = go.GetComponent<AnimationController>();
        aSpriteRenderers = new List<SpriteRenderer>();
        aSpriteRenderers.Add(aController.aIdleLeft_go.GetComponent<SpriteRenderer>());
        aSpriteRenderers.Add(aController.aIdleRight_go.GetComponent<SpriteRenderer>());
        aSpriteRenderers.Add(aController.aAttackLeft_go.GetComponent<SpriteRenderer>());
        aSpriteRenderers.Add(aController.aAttackRight_go.GetComponent<SpriteRenderer>());
        aSpriteRenderers.Add(aController.aCastLeft_go.GetComponent<SpriteRenderer>());
        aSpriteRenderers.Add(aController.aCastRight_go.GetComponent<SpriteRenderer>());
        aSpriteRenderers.Add(aController.aDeathLeft_go.GetComponent<SpriteRenderer>());
        aSpriteRenderers.Add(aController.aDeathRight_go.GetComponent<SpriteRenderer>());
    }

    public void Set_Visual()
    {
        for(int x = 0; x < aSpriteRenderers.Count; x++)
        {
            aSpriteRenderers[x].sortingOrder = hex.rendValue;
        }
    }
    #endregion
}
