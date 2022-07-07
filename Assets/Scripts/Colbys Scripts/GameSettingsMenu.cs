using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
  
    public Toggle playerMoveToggle;
    public Toggle DragToMove;

    void Start()
    {
        playerMoveToggle.isOn = GameSettings.touchPlayerToMove;
        DragToMove.isOn = GameSettings.dragToMove;
    }
    public void touchPlayerToMove(bool p_touchPlayerToMove)
    {
        if(playerMoveToggle.isOn)
        {
            GameSettings.touchPlayerToMove = !p_touchPlayerToMove;
        }
        else
        {
            GameSettings.touchPlayerToMove = p_touchPlayerToMove;
        }

        Debug.Log("GameSettingMenu Drag to Move: " + GameSettings.dragToMove);
    }

    public void dragToMove(bool p_dragToMove)
    {
        if(DragToMove.isOn)
        {
            GameSettings.dragToMove = !p_dragToMove;
        }
        else
        {
            GameSettings.dragToMove = p_dragToMove;
        }
        
        Debug.Log("GameSettingMenu Drag to Move: " + GameSettings.dragToMove);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

}
