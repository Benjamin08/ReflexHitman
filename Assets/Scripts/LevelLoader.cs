using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReloadLevel()
    {
        StartCoroutine(ReloadLevelCoroutine(SceneManager.GetActiveScene().buildIndex));
    }

    public void PlayGame()
    {
        StartCoroutine(ReloadLevelCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadCredits()
    {
        StartCoroutine(ReloadLevelCoroutine(12)); // 12 is credits
    }

    public void LoadMainScreen()
    {
        StartCoroutine(ReloadLevelCoroutine(0)); // 0 is Title scene
    }

    public void LoadNextLevel()
    {
        StartCoroutine(ReloadLevelCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
        
    }

    IEnumerator ReloadLevelCoroutine(int levelIndex)
    {
        // Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        // Reload level
        SceneManager.LoadScene(levelIndex);

    }
}
