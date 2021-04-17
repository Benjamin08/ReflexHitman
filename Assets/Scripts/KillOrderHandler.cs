using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOrderHandler : MonoBehaviour
{
    public List<GameObject> KillList;

    private GameHandler gameHandler;
    private GameObject Player;
    private LoadLevelData loadLevelData;

    void Awake()
    {
        //create and find our objects
        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        Player = GameObject.FindGameObjectWithTag("Player");
        loadLevelData = GameObject.FindGameObjectWithTag("loadLevelData").GetComponent<LoadLevelData>();

        //set our enemies to be ordered
        foreach(GameObject obj in KillList) { obj.GetComponent<Enemy>().isOrdered = true; }
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

    //returns true if the enemy killed was in the correct order
    public bool KillCheck(GameObject Killed)
    {
        int count;
        for (count = 0; count < KillList.Count; count++)
        {
            if (KillList[count].Equals(Killed))
            {
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
