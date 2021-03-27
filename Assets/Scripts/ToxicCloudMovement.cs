using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicCloudMovement : MonoBehaviour
{

    private Rigidbody2D rb;

    public bool horizontalOccilation = true;

    public enum OcilationFunction { Sine, Cosine}

    [Range(0f, 100f)]
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        
        StartCoroutine(OscillateHorizontal(OcilationFunction.Sine, moveSpeed, horizontalOccilation));
        
       
    }

    private IEnumerator OscillateHorizontal (OcilationFunction method, float scalar, bool moveHorizontal)
    {
        if(moveHorizontal)
        {
            while (true)
            {
                if (method == OcilationFunction.Sine)
                {
                    transform.position = new Vector3(Mathf.Sin(Time.time) * scalar, 0, 0);
                }
                else if (method == OcilationFunction.Cosine)
                {
                    transform.position = new Vector3(Mathf.Cos(Time.time) * scalar, 0, 0);
                }
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (true)
            {
                if (method == OcilationFunction.Sine)
                {
                    transform.position = new Vector3(0, Mathf.Sin(Time.time) * scalar, 0);
                }
                else if (method == OcilationFunction.Cosine)
                {
                    transform.position = new Vector3(0, Mathf.Cos(Time.time) * scalar, 0);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
