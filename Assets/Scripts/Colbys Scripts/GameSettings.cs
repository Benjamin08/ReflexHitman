using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings 
{
    public static bool touchPlayerToMove = true;
 


    public static void SetTouchPlayerToSwipe(bool p_touchPlayerToMove)
    {
        touchPlayerToMove= p_touchPlayerToMove;
    }
}
