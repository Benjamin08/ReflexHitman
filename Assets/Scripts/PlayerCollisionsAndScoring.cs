using CodeMonkey.MonoBehaviours;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollisionsAndScoring : MonoBehaviour
{

    public Rigidbody2D rb;
    public bool hit = false;

    private int counter = 0;
    private int counter2 = 0;
    Vector3 holdVelocity;
    public float timer;

    [Range(10f, 100f)]
    public float dragAmount = 10f;

    private Transform playerSpawn;

    public EnemyData enemyData;

    public GameHandler gameHandler;

    public CameraShake camShake;

    public bool isMoving = true;


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
            FunctionTimer.Create(() => SetDrag(dragAmount), 2f);
            hit = false;
        }

        if(rb.velocity.sqrMagnitude < .25f)
        {
            isMoving = false;
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

                    int randNumber = UnityEngine.Random.Range(0,2);
                    switch(randNumber)
                    {
                        case 0:
                            SoundManager.PlaySound(SoundManager.Sound.enemyHurt1, collision.transform.position);
                        break;

                        case 1:
                            SoundManager.PlaySound(SoundManager.Sound.enemyHurt2, collision.transform.position);
                        break;

                        case 2:
                            SoundManager.PlaySound(SoundManager.Sound.enemyHurt3, collision.transform.position);
                        break;

                        default:

                        break;
                    }
                    
                    enemyData.deadCount++;
                }
            }
           
           
            if(enemyData.deadCount.Equals(enemyData.enemyList.Count))
            {
                gameHandler.levelPassed = true;
                gameHandler.SetText();
                FunctionTimer.Create(() => gameHandler.NextLevel(), 1f);
            }
        
           
//

            hit = true;
        }
        
        if(collision.gameObject.CompareTag("Toxic Gas"))
        {
            rb.drag = 2000f;
            FunctionTimer.Create(() => SetDrag(dragAmount), 2f);

            transform.position = playerSpawn.transform.position;
        }

        if(collision.gameObject.CompareTag("SwipePowerup"))
        {
            Destroy(collision.gameObject);
            gameHandler.AddSwipe(1);
        }
    }


    public void SetDrag(float dragAmount)
    {
        rb.drag = dragAmount;
    }

}
