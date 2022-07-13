using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchTwo : MonoBehaviour
{
    
    Vector2 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;

    PlayerCollisionsAndScoring playerCollisionScore;

    public int numberOfTimesTouched = 0;

    public event EventHandler OnSwipeDone;

    [Range(0.05f, 1f)]
    public float throwForce = 0.3f;


    [Range(0.05f, 1f)]
    public float dragThreshhold;

    private float swipeLength = 0;

    public bool endOfTouch = false;

    public bool touchingPlayer;

    private void Start()
    {
        playerCollisionScore = GetComponent<PlayerCollisionsAndScoring>();
        OnSwipeDone += SwipeDone;
    }

    private void SwipeDone(object sender, EventArgs e)
    {
       
        
        numberOfTimesTouched++;
        
        GetComponent<Rigidbody2D>().AddForce(-direction / timeInterval * throwForce);
    
        touchingPlayer = false;
    }    

    void Update()
    {
        if(Input.touchCount > 0 && playerCollisionScore.isMoving == false) 
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);

            if(hitInfo.collider != null && GameSettings.touchPlayerToMove)
            {
                
            
                switch(hitInfo.collider.tag)
                {

                  case "Player" :
                      Debug.Log("hit player");
                       touchingPlayer = true;
                       break;

                }
            }
            else if(!GameSettings.touchPlayerToMove)
            {
                touchingPlayer = true;
            }
        

            if (Input.GetTouch(0).phase == TouchPhase.Began && touchingPlayer)
            {
                endOfTouch = false;
                touchTimeStart = Time.time;
                startPos = Input.GetTouch(0).position;
                playerCollisionScore.SetDrag(playerCollisionScore.dragAmount);
                
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended && touchingPlayer)
            {
                //direction = startPos - endPos;
                endPos = Input.GetTouch(0).position;


                playerCollisionScore.isMoving = true;
                endOfTouch = true;
                direction = startPos - endPos;
                touchTimeFinish = Time.time;
                timeInterval = touchTimeFinish - touchTimeStart;
                swipeLength = Vector2.Distance(startPos, endPos);

                SoundManager.PlayerMoveSound(startPos);

                if (swipeLength > 20 && playerCollisionScore.gameHandler.swipesLeft > 0 && touchingPlayer)
                {
                    OnSwipeDone?.Invoke(this, EventArgs.Empty);         // If our event is not null, then we invoke the event
                }
               
            }
        }
       
    }
}
