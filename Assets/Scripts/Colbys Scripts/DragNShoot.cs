using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragNShoot : MonoBehaviour
{
   

    public float power = 100f;
    public Rigidbody2D rb;

    public int powerLevel;

    public bool touchingPlayer;
    public bool invertDrag;

    Camera cam;

    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    Collider2D[] hitInfo;
    TrajectoryLine tl;
    PlayerCollisionsAndScoring playerCollisionScore;


    private void Start()
    {
        cam = Camera.main;
        tl = GetComponent<TrajectoryLine>();
        playerCollisionScore = GetComponent<PlayerCollisionsAndScoring>();
        rb = GetComponent<Rigidbody2D>();
        
    }

     private Vector2 CalculatePowerVectorV2(Vector2 beginPoint, Vector2 endPoint)
    {
        Vector2 difference = beginPoint-endPoint;
        float vectorPower = difference.magnitude;
        vectorPower = Mathf.Clamp(vectorPower, -powerLevel, powerLevel);

        return difference.normalized * vectorPower;


    
    }


    private void Update()
    {

        if(Input.touchCount > 0 && playerCollisionScore.isMoving == false)
        {


            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !playerCollisionScore.isMoving)
            {
                hitInfo = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
            
                    foreach (Collider2D i in hitInfo)
                    {
                        if( i != null && GameSettings.touchPlayerToMove)
                        {
                            switch(i.tag)
                            {

                                case "Player" :
                                Debug.Log("touchingPlayer is true");
                                touchingPlayer = true;
                                break;

                            }       
                        }
                        else if(!GameSettings.touchPlayerToMove)
                        {
                            touchingPlayer = true;
                        }
                    }

                startPoint = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                startPoint.z = -5;
                
            }

            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && touchingPlayer)
            {
                Vector3 currentPoint = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                
                currentPoint.z = -5;  
                tl.RenderLine(startPoint, currentPoint);
            }

            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && touchingPlayer)
            {
                endPoint = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                endPoint.z = -5;
              

                playerCollisionScore.numberOfTimesTouched++;


                Debug.Log("touchingPlayer is false");
                touchingPlayer = false;
                playerCollisionScore.isMoving = true;

                Array.Clear(hitInfo,0,hitInfo.Length);

                force = CalculatePowerVectorV2(startPoint,endPoint);
                Debug.Log("force: " + force);

                SoundManager.PlayerMoveSound(transform.position);

                if (invertDrag)
                {
                    rb.AddForce(force * power, ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(-force * power, ForceMode2D.Impulse);
                }

                tl.EndLine();
                playerCollisionScore.gameHandler.swipesLeft--;
                playerCollisionScore.gameHandler.numberOfSwipesText.text = "Number Of Swipes: " + playerCollisionScore.gameHandler.swipesLeft.ToString();
            }
        }  


    }
}
