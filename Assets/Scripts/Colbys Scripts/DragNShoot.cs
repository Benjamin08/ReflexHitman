using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
            
            if(hitInfo.collider != null && GameSettings.touchPlayerToMove)
            {
                
                //Debug.Log("hit: " + hitInfo.collider.name);
                switch(hitInfo.collider.tag)
                {

                  case "Player" :
                      //Debug.Log("hit player");
                       touchingPlayer = true;
                       break;

                }
            }
            else if(!GameSettings.touchPlayerToMove)
            {
                touchingPlayer = true;
            }



            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && touchingPlayer)
            {
                startPoint = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                startPoint.z = -5;
                //Debug.Log("start position: " + startPoint);
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
                //Debug.Log("end position: " + endPoint);
                touchingPlayer = false;
                playerCollisionScore.isMoving = true;
                    
                force = CalculatePowerVectorV2(startPoint,endPoint);
                Debug.Log("force: " + force);
                
                if(invertDrag)
                {
                    rb.AddForce(force * power, ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(-force * power, ForceMode2D.Impulse);
                }


                tl.EndLine();
            }
        }    
    }
}
