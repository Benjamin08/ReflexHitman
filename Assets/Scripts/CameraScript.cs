using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform cameraLookPoint;

    private Func<Vector3> GetCameraFollowPositionFunc;          // delegate that returns a vector3

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
        
    }
}
