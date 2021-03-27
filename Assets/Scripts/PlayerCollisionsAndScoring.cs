using CodeMonkey.MonoBehaviours;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollisionsAndScoring : MonoBehaviour
{

    Rigidbody2D rb;
    public bool hit = false;

    private int counter = 0;
    private int counter2 = 0;
    Vector3 holdVelocity;
    public float timer;

    private Transform playerSpawn;

    public EnemyData enemyData;

    public GameHandler gameHandler;

    public CameraShake camShake;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("Player Spawn").transform;
        rb = GetComponent<Rigidbody2D>();
        enemyData = GameObject.Find("EnemyData").GetComponent<EnemyData>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hit)
        {
            rb.drag = 2000f;
            FunctionTimer.Create(() => SetDrag(1f), 2f);
            hit = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //HitEnemyEvent.Invoke(this, new HitEnemyEventArgs { enemyHit = collision.gameObject });

            StartCoroutine(camShake.Shake(.2f, .4f));
           
            for(counter = 0; counter < enemyData.enemyList.Count; counter++)
            {
               if(collision.gameObject.Equals(enemyData.enemyList[counter]))
                {
                    collision.GetComponent<Enemy>().particleSystem.Play();
                    enemyData.enemyList[counter].GetComponent<SpriteRenderer>().enabled = false;
                    enemyData.enemyList[counter].GetComponent<Collider2D>().enabled = false;
                    enemyData.deadCount++;
                }
            }
           
           
            if(enemyData.deadCount.Equals(enemyData.enemyList.Count))
            {
                FunctionTimer.Create(() => gameHandler.NextLevel(), 1f);
            }
        
           
//

            hit = true;
        }
        
        if(collision.gameObject.CompareTag("Toxic Gas"))
        {
            rb.drag = 2000f;
            FunctionTimer.Create(() => SetDrag(1f), 2f);

            transform.position = playerSpawn.transform.position;
        }
    }


    public void SetDrag(float dragAmount)
    {
        rb.drag = dragAmount;
    }

}
