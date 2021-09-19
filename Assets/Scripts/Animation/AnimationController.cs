using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public bool aOrientationLeft = true;

    public Animator currentAnimation;

    public GameObject aIdleLeft_go;
    public Animator aIdleLeft;

    public GameObject aIdleRight_go;
    public Animator aIdleRight;

    public GameObject aAttackLeft_go;
    public Animator aAttackLeft;

    public GameObject aAttackRight_go;
    public Animator aAttackRight;

    public GameObject aCastLeft_go;
    public Animator aCastLeft;

    public GameObject aCastRight_go;
    public Animator aCastRight;

    public GameObject aDeathLeft_go;
    public Animator aDeathLeft;

    public GameObject aDeathRight_go;
    public Animator aDeathRight;

    private void Awake()
    {
        aIdleLeft_go = transform.Find("Anim_Idle_Left").gameObject;
        aIdleLeft = aIdleLeft_go.GetComponent<Animator>();

        aIdleRight_go = transform.Find("Anim_Idle_Right").gameObject;
        aIdleRight = aIdleRight_go.GetComponent<Animator>();

        aAttackLeft_go = transform.Find("Anim_Attack_Left").gameObject;
        aAttackLeft = aAttackLeft_go.GetComponent<Animator>();

        aAttackRight_go = transform.Find("Anim_Attack_Right").gameObject;
        aAttackRight = aAttackRight_go.GetComponent<Animator>();

        aCastLeft_go = transform.Find("Anim_Cast_Left").gameObject;
        aCastLeft = aCastLeft_go.GetComponent<Animator>();

        aCastRight_go = transform.Find("Anim_Cast_Right").gameObject;
        aCastRight = aCastRight_go.GetComponent<Animator>();

        aDeathLeft_go = transform.Find("Anim_Death_Left").gameObject;
        aDeathLeft = aDeathLeft_go.GetComponent<Animator>();

        aDeathRight_go = transform.Find("Anim_Death_Right").gameObject;
        aDeathRight = aDeathRight_go.GetComponent<Animator>();

        Anim_Play_Idle();
    }

    public void Anim_Play_Idle()
    {
        Reset();
        if (aOrientationLeft)
        {
            aIdleLeft_go.SetActive(true);
            currentAnimation = aIdleLeft;
        }
        else
        {
            aIdleRight_go.SetActive(true);
            currentAnimation = aIdleRight;
        }
        currentAnimation.speed = 0.75f;
    }

    public IEnumerator Anim_Play_Attack()
    {
        Reset();
        if(aOrientationLeft)
        {
            aAttackLeft_go.SetActive(true);
            currentAnimation = aAttackLeft;
        }
        else 
        {
            aAttackRight_go.SetActive(true);
            currentAnimation = aAttackRight;
        }
        currentAnimation.speed = 0.75f;

        float timer = 0.0f;
        float length = currentAnimation.runtimeAnimatorController.animationClips[0].length / currentAnimation.speed;
        while(timer < length)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        Anim_Play_Idle();
    }

    public IEnumerator Anim_Play_Cast()
    {
        Reset();
        if(aOrientationLeft)
        {
            aCastLeft_go.SetActive(true);
            currentAnimation = aCastLeft;
        }
        else 
        {
            aCastRight_go.SetActive(true);
            currentAnimation = aCastRight;
        }
        currentAnimation.speed = 0.5f;

        float timer = 0.0f;
        float length = currentAnimation.runtimeAnimatorController.animationClips[0].length / currentAnimation.speed;
        while(timer < length)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Anim_Play_Idle();
    }

    public IEnumerator Anim_Play_Death()
    {
        Reset();
        if(aOrientationLeft)
        {
            aDeathLeft_go.SetActive(true);
            currentAnimation = aDeathLeft;
        }
        else 
        {
            aDeathRight_go.SetActive(true);
            currentAnimation = aDeathRight;
        }
        currentAnimation.speed = 0.5f;

        float timer = 0.0f;
        float length = currentAnimation.runtimeAnimatorController.animationClips[0].length / currentAnimation.speed;
        while(timer < length)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void Reset()
    {
        aIdleLeft_go.SetActive(false);
        aIdleRight_go.SetActive(false);
        aAttackLeft_go.SetActive(false);
        aAttackRight_go.SetActive(false);
        aCastLeft_go.SetActive(false);
        aCastRight_go.SetActive(false);
        aDeathLeft_go.SetActive(false);
        aDeathRight_go.SetActive(false);
        currentAnimation = null;
    }

    public void SwitchAnimOrientation()
    {
        if(aOrientationLeft) aOrientationLeft = false;
        else aOrientationLeft = true;

        Anim_Play_Idle();
    }
}
