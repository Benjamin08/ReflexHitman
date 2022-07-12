using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLine : MonoBehaviour
{
    public LineRenderer lr;
    Vector3[] points = new Vector3[2];
    
     private void awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startPoint ,Vector3 endPoint)
    {
        lr.positionCount = 2;

        

        points[0] = startPoint;
        points[1] = endPoint;

        lr.SetPositions(points);
    }

    public void EndLine()
    {
        lr.positionCount = 0;
        Array.Clear(points,0,1);
    }

}
