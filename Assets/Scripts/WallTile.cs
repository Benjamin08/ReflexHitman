using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : MonoBehaviour
{
    private float timer = 0f;
    private bool timing = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (timing)
        {
            if (timer < 5) { timer += Time.deltaTime; }
            else
            {
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                timing = false;
            }
        }
    }
}
