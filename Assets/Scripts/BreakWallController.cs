using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWallController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(this.gameObject);
            Instantiate(Resources.Load("BreakWall_Broken"), transform.position, transform.rotation);
        }
    }
}
