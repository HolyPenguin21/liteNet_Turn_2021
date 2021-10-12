using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageEffect : MonoBehaviour
{
    public int dmg;
    public bool attacker;
    private GameObject redImage;
    private GameObject blueImage;
    private TextMeshPro dmg_Text;
    private Vector2 curPos;

    private void Awake()
    {
        dmg_Text = transform.Find("Text").GetComponent<TextMeshPro>();
        redImage = transform.Find("Red").gameObject;
        blueImage = transform.Find("Blue").gameObject;
    }

    private void Start()
    {
        curPos = new Vector3(transform.position.x + Random.Range(-0.25f,0.25f), transform.position.y + 0.5f);

        dmg_Text.SetText("" + dmg);

        if(attacker)
        {
            redImage.SetActive(true);
            blueImage.SetActive(false);
        }
        else
        {
            redImage.SetActive(false);
            blueImage.SetActive(true);
        }

        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        curPos.y += 0.5f * Time.deltaTime;
        transform.position = curPos;
    }
}
