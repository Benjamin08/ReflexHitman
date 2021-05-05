using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;


    private GameHandler gameHandler;

    public GameObject[] Stars;

    public GameObject ColbyDisplay;

    void Start()
    {


        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();


        Debug.Log("SceneManager.sceneCountInBuildSettings " + SceneManager.sceneCountInBuildSettings);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReloadLevel()
    {
        StartCoroutine(LevelChangeCoroutine(SceneManager.GetActiveScene().buildIndex));
    }

    public void PlayGame()
    {
        StartCoroutine(LevelChangeCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadCredits()
    {
        StartCoroutine(LevelChangeCoroutine(12)); // 12 is credits
    }

    public void LoadMainScreen()
    {
        StartCoroutine(LevelChangeCoroutine(0)); // 0 is Title scene
    }

    public void LoadScene(int sceneRef)
    {
        StartCoroutine(LevelChangeCoroutine(sceneRef));
    }

    public void LoadNextLevel()
    {
        //StartCoroutine(ReloadLevelCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
        StartCoroutine(ShowLevelDisplay());
        
    }
    public void LoadLastLevel()
    {
        
        StartCoroutine(LevelChangeCoroutine(SceneManager.GetActiveScene().buildIndex - 1));
        
    }
    public void LoadNextLevelButton()
    {
        if(SceneManager.GetActiveScene().buildIndex <= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("SceneManager.GetActiveScene().buildIndex " + SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
        else
        {
            SceneManager.LoadScene("Title Screen");
        }
       
    }

    public void ShowStars(int amountToShow)
    {
        for(int counter = 0; counter < amountToShow; counter++)
        {
            Stars[counter].SetActive(true);
        }
    }

    private void DisableStars()
    {
        foreach (GameObject star in Stars)
        {
            star.SetActive(false);
        }
    }

    IEnumerator ShowLevelDisplay()
    {
        transition.SetTrigger("Start");

        CalculateStars();

        yield return new WaitForSeconds(transitionTime);
        //return null;
    }

    IEnumerator LevelChangeCoroutine(int levelIndex)
    {
        // Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        // Reload level
        SceneManager.LoadScene(levelIndex);

    }

    private void CalculateStars()
    {
        if(gameHandler.touchInput.numberOfTimesTouched <= gameHandler.loadLevelData.thisLevelData.threeStarSwipeAmount)
        {
            // 3 
            Debug.Log("3 star");
            ShowStars(3);
            return;
        }
        else if(gameHandler.touchInput.numberOfTimesTouched <= gameHandler.loadLevelData.thisLevelData.twoStarSwipeAmount)
        {
            // 2 STARS
            Debug.Log("2 Star");
            ShowStars(2);
            return;
        }
        else
        {
            //1 STAR
            Debug.Log("1 Star");
            ShowStars(1);
            return;
        }

        

    }
}
