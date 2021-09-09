using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] spritesToUse;

    private void Awake()
    {
        spriteRenderer = transform.Find("background").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = spritesToUse[UnityEngine.Random.Range(0, spritesToUse.Length)];
    }
}
