using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AssignLevelButtons : MonoBehaviour
{

    public GameObject[] ButtonList;
    public LevelTracker levelTracker;

    // Start is called before the first frame update
    void Start()
    {
        levelTracker = GameObject.FindGameObjectWithTag("LevelTracker").GetComponent<LevelTracker>();
        ButtonList = GameObject.FindGameObjectsWithTag("Level Icon");
        for(int i = 0; i <= ButtonList.Length -1; i++)
        {
            ButtonList[i].GetComponent<Button>().onClick.AddListener(delegate{SetLevelButtonOnClick(levelTracker.currentLevel);});
            levelTracker.currentLevel++;
        }
    }

    private void SetLevelButtonOnClick(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
