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
    public bool canAct;

    // Character specific vars
    public int id;
    public int acc_cost;
    public int ingame_cost;
    public CharVars.faction faction;
    public Sprite image;
    public string name;
    
    public CharVars.char_Hp health;
    public CharVars.char_Defence defence;
    public CharVars.char_Move movement;
    public List<CharVars.char_Attack> attacks;


    #region Animations
    public IEnumerator Die_Animation()
    {
        yield return aController.Anim_Play_Death();
    }
    #endregion

    #region Attack
    public IEnumerator Attack_Start(Hex hex)
    {
        CheckOrientation(hex);

        float t = 0f;
        Vector3 attackVector = tr.position + (hex.tr.position - tr.position) / 4; // A+(B-A)/2 - vector middle
        while (t < 1f)
        {
            tr.position = Vector3.Lerp(tr.position, attackVector, t);
            t += Time.deltaTime * 6;
            yield return null;
        }
    }

    public IEnumerator Attack_Animation()
    {
        yield return aController.Anim_Play_Attack();
    }

    public IEnumerator Attack_End()
    {
        float t = 0f;
        while (t < 1f)
        {
            tr.position = Vector3.Lerp(tr.position, hex.tr.position, t);
            t += Time.deltaTime * 3;
            yield return null;
        }

        if(owner.aiPlayer)
            GameObject.Find("SceneMain").GetComponent<SceneMain>().aiBehaviour.aiInAction = false;
    }
    #endregion

    #region Movement
    public IEnumerator Move(List<Hex> somePath)
    {
        // Debug.Log("Character > " + this + " moving.");
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

        if(owner.aiPlayer)
            GameObject.Find("SceneMain").GetComponent<SceneMain>().aiBehaviour.aiInAction = false;
    }

    private IEnumerator ActualMove(Hex hexToMove)
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * 3;
            time = Mathf.Clamp(time + Time.deltaTime * 0.01f, 0f, 1f);
            tr.position = Vector3.Lerp(tr.position, hexToMove.tr.position, time);
            yield return null;
        }
        tr.position = hexToMove.tr.position;
    }
    #endregion

    #region Heath
    public IEnumerator Set_Health(int hp_target)
	{
        if(health.hp_cur > hp_target)
        {
            //Damage effect
        }
        else
        {
            //Heal effect
        }
        health.hp_cur = hp_target;

        yield return null;
	}
    #endregion

    #region Visual
    public void Set_Visual()
    {
        for(int x = 0; x < aSpriteRenderers.Count; x++)
        {
            aSpriteRenderers[x].sortingOrder = hex.rendValue;
        }
    }

    private void CheckOrientation(Hex nextHex)
    {
        float cur_hexPos = hex.tr.position.x;
        float next_hexPos = nextHex.tr.position.x;

        if(cur_hexPos < next_hexPos && aController.aOrientationLeft)
            aController.SwitchAnimOrientation();
        else if(cur_hexPos > next_hexPos && !aController.aOrientationLeft)
            aController.SwitchAnimOrientation();
    }
    #endregion
}
