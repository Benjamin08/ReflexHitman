using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWallController : MonoBehaviour
{
    private List<GameObject> TileList;

    void Awake()
    {
        TileList = new List<GameObject>();
        if (GetTiles()) { Debug.Log("wall tiles Loaded: " + TileList.Count + " Tiles found"); }
        else { Debug.LogError("Error loading wall tiles"); }
    }

    void Update()
    {
        
    }

    bool GetTiles()
    {
        foreach(Transform child in transform)
        {
            TileList.Add(child.gameObject);
        }
        
        if(TileList != null) { return true; }
        else { return false; }
    }
}
