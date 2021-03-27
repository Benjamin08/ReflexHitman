using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    public GameObject Door;

    private Door_Control doorControl;

    void Awake()
    {
        doorControl = Door.GetComponent<Door_Control>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            doorControl.Open();
        }
    }
}
