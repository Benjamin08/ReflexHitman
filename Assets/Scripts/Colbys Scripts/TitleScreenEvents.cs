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
        
        FunctionTimer.Create(()=> SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1), 2f);
    }
}
