using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAssets : MonoBehaviour
{
 
    public bool touchPlayerToSwipe = false;
        
    public void SetTouchPlayerToSwipe(bool p_touchPlayerToSwipe)
    {
        touchPlayerToSwipe = p_touchPlayerToSwipe;
    }

}
