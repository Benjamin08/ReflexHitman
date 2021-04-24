using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOrderHandler : MonoBehaviour
{
    public List<GameObject> KillList;
    public GameObject Trigger;

    private LoadLevelData loadLevelData;

    void Awake()
    {
        //create and find our objects
        loadLevelData = GameObject.FindGameObjectWithTag("loadLevelData").GetComponent<LoadLevelData>();

        //set our enemies to be ordered
        foreach (GameObject obj in KillList) { obj.GetComponent<Enemy>().isOrdered = true; }

        //change sprites to display kill order
        int count;
        for (count = 0; count < KillList.Count; count++)
        {
            KillList[count].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Enemy_" + (count + 1));
        }
    }

    //Resets all our ordered enemies
    void ResetEnemies()
    {
        int counter = 0;
        foreach (GameObject obj in KillList)
        {
            if (!obj.GetComponent<SpriteRenderer>().enabled)
            {
                obj.GetComponent<SpriteRenderer>().enabled = true;
                obj.GetComponent<Collider2D>().enabled = true;
                counter++;
            }
        }
        loadLevelData.ResetEnemyDeaths(counter);
    }

    //Calls trigger function from Trigger
    void OnTrigger()
    {
        if (Trigger != null)
        {
            switch (Trigger.name)
            {
                case "door":
                    Trigger.GetComponent<Door_Control>().Open();
                    Debug.Log("Kill Order: Door triggered");
                    break;
                default:
                    Debug.Log("Kill Order: No trigger found");
                    break;
            }
        }
        else { Debug.LogError("Kill Order: Null trigger"); }
    }

    //Returns true if all enemies before the one at index are already dead
    bool OrderCheck(float index)
    {
        int counter;
        for (counter = 0; counter < index; counter++)
        {
            if(KillList[counter].GetComponent<SpriteRenderer>().enabled) { return false; }
        }
        return true;
    }

    //Returns true if the enemy killed was in the correct order
    public bool KillCheck(GameObject Killed)
    {
        int count;
        for (count = 0; count < KillList.Count; count++)
        {
            if (KillList[count].Equals(Killed))
            {
                if(count == KillList.Count - 1)
                {
                    OnTrigger();
                }
                if(OrderCheck(count)) { return true; }
                else 
                {
                    ResetEnemies();
                    return false;
                }
            }
        }
        Debug.LogError("Error finding enemy in KillList");
        return false;
    }
}
