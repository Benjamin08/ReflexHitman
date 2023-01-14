using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [SerializeField]
    int health = 1;

    [SerializeField]
    UnityEngine.Object dustructableRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("hit player box");
            health--;
            if(health <= 0)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        
        GameObject destructable = (GameObject) Instantiate(dustructableRef);
        
        //map the newly loading derstructable to the x and y position to the destroyed object
        destructable.transform.position = transform.position;
    
        Destroy(gameObject);
    }
}
