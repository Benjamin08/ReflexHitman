using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;
using UnityEngine.SceneManagement;


public class TitleScreenEvents : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
        //FunctionTimer.Create(()=> SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1), 2f);
    }

    public void PlayGame()
    {
    
           SceneManager.LoadScene("1_1"); 

    }

    public void Settings()
    {
    
           SceneManager.LoadScene("Settings"); 

    }
}
