using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Control : MonoBehaviour
{

    private List<GameObject> panelList;

    private Vector2 startPos1;
    private Vector2 startPos2;
    private float endPos1;
    private float endPos2;

    [Range(.001f, .1f)]
    public float speed;

    [Range(1f,3f)]
    public float doorMoveAmount;

    [SerializeField]
    private bool horizontal = true;
    private bool open = false;
    private Vector2 velocity;
    private bool moving = false;

    private void Awake()
    {
        panelList = new List<GameObject>();
        panelList.Add(gameObject.transform.Find("DoorPanel").gameObject);
        panelList.Add(gameObject.transform.Find("DoorPanel2").gameObject);
        startPos1 = new Vector2(panelList[0].transform.position.x, panelList[0].transform.position.y);
        startPos2 = new Vector2(panelList[1].transform.position.x, panelList[1].transform.position.y);
    }

    void Update()
    {
        if (!open && moving)
        {
            panelList[0].transform.position += new Vector3(-velocity.x, -velocity.y);
            panelList[1].transform.position += new Vector3(velocity.x, velocity.y);
            if (horizontal)
            {
                if (panelList[0].transform.position.x < endPos1 || panelList[1].transform.position.x > endPos2)
                {
                    panelList[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    panelList[1].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    open = true;
                    moving = false;
                }
            }
            else
            {
                if (panelList[0].transform.position.y < endPos1 || panelList[1].transform.position.y > endPos2)
                {
                    panelList[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    panelList[1].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    open = true;
                    moving = false;
                }
            }
        }
    }

    public void Open()
    {
        if (!open)
        {
            if (horizontal)
            {
                endPos1 = startPos1.x - doorMoveAmount;
                endPos2 = startPos2.x + doorMoveAmount;
                velocity = new Vector2(speed, 0);
            }
            else
            {
                endPos1 = startPos1.y - doorMoveAmount;
                endPos2 = startPos2.y + doorMoveAmount;
                velocity = new Vector2(0, speed);
            }
            moving = true;
        }
    }
}
