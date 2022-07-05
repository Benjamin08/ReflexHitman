using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
  
  public Toggle playerMoveToggle;

    void Start()
    {
        playerMoveToggle.isOn = GameSettings.touchPlayerToMove;
    }
    public void ChangeSetting(bool p_touchPlayerToMove)
    {
        GameSettings.touchPlayerToMove = p_touchPlayerToMove;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

}
