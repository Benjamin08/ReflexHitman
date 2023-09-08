using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform cameraLookPoint;
    private Camera camera;
    private Func<Vector3> GetCameraFollowPositionFunc;          // delegate that returns a vector3

    public Vector2 curDist { get; private set; }
    public Vector2 prevDist { get; private set; }
    public float touchDelta { get; private set; }
    public float speedTouch0 { get; private set; }
    public float speedTouch1 { get; private set; }
    public float varianceInDistances { get; private set; }
    public float minPinchSpeed { get; private set; }
    public int speed { get; private set; }

    private bool cameraMoveState = false;

    private string numberOfTouches;

    public void Setup(Func<Vector3> GetCameraFollowPositionFuncPar)
    {
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFuncPar;
    }

    public void SetGetCameraFollowPositionFunc(Func<Vector3> GetCameraFollowPositionFuncPar)
    {
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFuncPar;
    }

    void Start()
    {
        cameraLookPoint = GameObject.FindGameObjectWithTag("CameraLookPoint").transform;
        Setup(() => cameraLookPoint.position);
        camera = GetComponent<Camera>();
        minPinchSpeed = .1f;
        speed = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
        cameraFollowPosition.z = transform.position.z;


        Vector3 cameraMoveDirection = cameraFollowPosition - transform.position.normalized;

        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        float cameraMoveSpeed = 2f;

        if(distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMoveDirection * distance * cameraMoveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);
        
            if(distanceAfterMoving > distance)
            {
                // Overshot the target
                newCameraPosition = cameraFollowPosition;
            }
            transform.position = newCameraPosition;
        }


          // Handle screen touches.
        if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) 
            {
                cameraMoveState = true;
                curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
                prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
                touchDelta = curDist.magnitude - prevDist.magnitude;
                speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
                speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;
                if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
                {
                    camera.fieldOfView = Mathf.Clamp(camera.fieldOfView + (1 * speed),15,90);
                }
                if ((touchDelta +varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
                {
                    camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - (1 * speed),15,90);
                }
            }else
            {
                cameraMoveState = false;
            }       
        
    }

    public string GetNumberOfTouches()
    {
        numberOfTouches = Input.touchCount.ToString();
        if(cameraMoveState)
        {
             return numberOfTouches + " in camera move state";
        }
        else
        {
            return numberOfTouches;
        }
    }
}
