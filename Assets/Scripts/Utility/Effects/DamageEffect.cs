using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageEffect : MonoBehaviour
{
    public int dmg;
    private TextMeshPro dmg_Text;
    private Vector2 curPos;

    private void Awake()
    {
        dmg_Text = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        dmg_Text.SetText("" + dmg);

        curPos = new Vector3(transform.position.x + Random.Range(-0.25f,0.25f), transform.position.y + 0.5f);

        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        curPos.y += 0.5f * Time.deltaTime;
        transform.position = curPos;
    }
}
