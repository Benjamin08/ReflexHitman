using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DragToShoot : MonoBehaviour
{
    public event EventHandler OnDragDone;
    private Touch touch;

    private float dragMax;

    private Vector2 startPos;

    private Vector2 endPos;
    private Vector2 dragDirection;

    private float dragLength;
    private bool touchingPlayer = false;

    PlayerCollisionsAndScoring playerCollisionScore;

    private int numberOfTimesTouched = 0;

    [Range(0.05f, 1f)]
    public float force = 0.3f;

    private void DragDone(object sender, EventArgs e)
    {
       
        numberOfTimesTouched++;
        
        GetComponent<Rigidbody2D>().AddForce(dragDirection /  force);
    
        touchingPlayer = false;
    }   
    // Start is called before the first frame update
    void Start()
    {
         playerCollisionScore = GetComponent<PlayerCollisionsAndScoring>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0) 
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);

            if(hitInfo.collider.tag == "Player")
            {
                
            
                switch(hitInfo.collider.name)
                {

                  case "Player" :
                      Debug.Log("hit player");
                       touchingPlayer = true;
                       break;

                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began && touchingPlayer)
            {
                startPos = Input.GetTouch(0).position;
            }
            
            if (Input.GetTouch(0).phase == TouchPhase.Ended && touchingPlayer)
            {
                endPos = Input.GetTouch(0).position;
                dragDirection = startPos - endPos;

                dragLength = Vector2.Distance(startPos, endPos);

                if(dragLength > dragMax)
                {
                    dragLength = dragMax;
                }

                if(dragLength > 20 && playerCollisionScore.gameHandler.swipesLeft > 0 && touchingPlayer)
                {
                    OnDragDone?.Invoke(this, EventArgs.Empty);         // If our event is not null, then we invoke the event
                }

            }
        }
    }
}
