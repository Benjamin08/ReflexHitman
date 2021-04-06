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

    public bool passThrough = false;

    private int counter = 0;
    private int counter2 = 0;
    Vector3 holdVelocity;
    public float timer;

    [Range(.1f, 100f)]
    public float dragAmount = 10f;

    private Transform playerSpawn;

    public LoadLevelData loadLevelData;

    public GameHandler gameHandler;

    public CameraShake camShake;

    public bool isMoving = true;


    // Start is called before the first frame update
    void Start()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("Player Spawn").transform;
        rb = GetComponent<Rigidbody2D>();
        loadLevelData = GameObject.FindGameObjectWithTag("loadLevelData").GetComponent<LoadLevelData>();
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hit && !passThrough)
        {
            rb.drag = 2000f;
            FunctionTimer.Create(() => SetDrag(dragAmount), 2f);
            hit = false;
        }
        else if(hit && passThrough)
        {
            hit = false;
            passThrough = false;
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
           
            for(counter = 0; counter < loadLevelData.enemyList.Count; counter++)
            {
               if(collision.gameObject.Equals(loadLevelData.enemyList[counter]))
                {
                    collision.GetComponent<Enemy>().particleSystem.Play();
                    loadLevelData.enemyList[counter].GetComponent<SpriteRenderer>().enabled = false;
                    loadLevelData.enemyList[counter].GetComponent<Collider2D>().enabled = false;

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
                    
                    
                    loadLevelData.TriggerEvent();
                }
            }
           
           
            if(loadLevelData.deadCount.Equals(loadLevelData.enemyList.Count))
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
            SoundManager.PlaySound(SoundManager.Sound.powerup_swipe);
        }
        if(collision.gameObject.CompareTag("PassThroughPowerup"))
        {
            Destroy(collision.gameObject);
            passThrough = true;
        }
    }


    public void SetDrag(float dragAmount)
    {
        rb.drag = dragAmount;
    }

}
