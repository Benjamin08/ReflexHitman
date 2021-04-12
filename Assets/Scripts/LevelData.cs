using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Colbys Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int maxNumberOfSwipes;

    
    public int oneStarSwipeAmount;
    
    
    public int twoStarSwipeAmount;

   
    public int threeStarSwipeAmount;

    [SerializeField]
    private bool levelPassed;

    public int GetMaxNumberOfSwipes()
    {
        return maxNumberOfSwipes;
    }
}
