using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings 
{
    public static bool touchPlayerToMove = true;
    public static bool dragToMove = true;


    public static void SetTouchPlayerToSwipe(bool p_touchPlayerToMove)
    {
        touchPlayerToMove= p_touchPlayerToMove;
    }

    public static void SetDragToMove(bool p_dragToMove)
    {
        dragToMove = p_dragToMove;
        Debug.Log("GameSetting Drag to Move: " + dragToMove);
    }

}
