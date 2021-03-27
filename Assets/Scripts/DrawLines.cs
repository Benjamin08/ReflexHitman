using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private float counter;
    private float dist;

    public Transform origin;
    public Transform trueDestination;
    public Transform destination1;
    public Transform destination2;
    public float lineDrawingSpeed = 6f;


    // Start is called before the first frame update
    void Start()
    {


        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.startWidth = .45f;
        lineRenderer.endWidth = .3f;

        dist = Vector3.Distance(origin.position, trueDestination.position);
    }

    // Update is called once per frame
    void Update()
    {
    

        if(counter < dist)
        {
            counter += .1f / lineDrawingSpeed;

            float x = Mathf.Lerp(0, dist, counter);

            Vector3 pointA = origin.position;
            Vector3 pointB = trueDestination.position;

            Vector3 pointALongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

            lineRenderer.SetPosition(1, pointALongLine);
        }
    }
}
