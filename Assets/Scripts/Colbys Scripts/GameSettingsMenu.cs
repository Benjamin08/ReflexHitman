using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
  
  public Toggle playerSwipeToggle;

    void Start()
    {
        playerSwipeToggle.isOn = GameSettings.touchPlayerToSwipe;
    }
    public void ChangeSetting(bool p_touchPlayerToSwipe)
    {
        GameSettings.touchPlayerToSwipe = p_touchPlayerToSwipe;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

}
