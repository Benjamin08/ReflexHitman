using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
   
    public float power = 100f;
    public Rigidbody2D rb;

    public Vector2 minPower;
    public Vector2 maxPower;

    Camera cam;

    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    private void Start()
    {
        cam = Camera.main;
    }


    private void Update()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startPoint = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
            startPoint.z = 15;
            Debug.Log("start position: " + startPoint);
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endPoint = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
            endPoint.z = 15;
            Debug.Log("end position: " + endPoint);

            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x,maxPower.x),Mathf.Clamp(startPoint.y - endPoint.y, minPower.y,maxPower.y));
            Debug.Log("force: " + force);
            rb.AddForce(force * power, ForceMode2D.Impulse);

        }
    }
}
