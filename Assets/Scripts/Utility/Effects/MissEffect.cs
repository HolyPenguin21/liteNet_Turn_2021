using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissEffect : MonoBehaviour
{
    private Vector2 curPos;
    
    void Start()
    {
        curPos = new Vector3(transform.position.x + Random.Range(-0.25f,0.25f), transform.position.y + 0.5f);
        
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        curPos.y += 0.5f * Time.deltaTime;
        transform.position = curPos;
    }
}
