using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.MonoBehaviours;
using CodeMonkey.Utils;
using CodeMonkey;
using System;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{

    public Text numberOfSwipesText;
    public Text levelCompleteText;

    public int currentLevel = 0;

    public int swipesLeft;

    public bool levelPassed = false;

    private CameraScript camera;

    // These transforms are so the camera can folllow them at any point
    //public Transform cameraLookPoint;

    public Transform playerSpawn;

    public GameObject[] playerTypes;

    public GameObject player;

    //public TouchTwo touchInput;
    public PlayerCollisionsAndScoring playerCollisionAndScoring;

    public GameObject[] enemyArray;
 

    public LevelLoader levelTransitioner;

    public LoadLevelData loadLevelData;

    public void ResetGame()
    {
        player.GetComponent<Rigidbody2D>().drag = 2000f;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Transform>().position = playerSpawn.position;
        //touchInput.OnSwipeDone += EndOfSwipe;

        loadLevelData.deadCount = 0;

        //touchInput.numberOfTimesTouched = 0;

        swipesLeft = loadLevelData.thisLevelData.GetMaxNumberOfSwipes();

        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;
        levelCompleteText.text = "Level Complete: " + levelPassed;

        foreach (GameObject enemy in enemyArray)
        {
            enemy.GetComponent<SpriteRenderer>().enabled = true;
            enemy.GetComponent<Collider2D>().enabled = true;
        }

        FunctionTimer.Create(() => player.GetComponent<PlayerCollisionsAndScoring>().SetDrag(playerCollisionAndScoring.dragAmount), .3f);

    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();

        levelTransitioner = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();

        loadLevelData = GameObject.FindGameObjectWithTag("loadLevelData").GetComponent<LoadLevelData>();

        enemyArray = loadLevelData.enemyArray;

        //camera.Setup(() => cameraLookPoint.position);

        swipesLeft = loadLevelData.thisLevelData.GetMaxNumberOfSwipes();

        numberOfSwipesText = GameObject.Find("Number Of Swipes").GetComponent<Text>();

        levelCompleteText = GameObject.Find("Level Complete Text").GetComponent<Text>();

        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;

        levelCompleteText.text = "Level Complete: " + levelPassed;

        if(GameSettings.dragToMove)
        {
            player = playerTypes[0];
        }
        else
        {
            player = playerTypes[1];
        }

        playerCollisionAndScoring = player.GetComponent<PlayerCollisionsAndScoring>();

        playerCollisionAndScoring.rb = player.GetComponent<Rigidbody2D>();
        
        //touchInput = player.GetComponent<TouchTwo>();

        //touchInput.OnSwipeDone += EndOfSwipe;

        playerSpawn = GameObject.FindGameObjectWithTag("Player Spawn").transform;

        SoundManager.Initialize();

    }


    public void NextLevel()
    {
        Debug.Log("Next Level " + playerCollisionAndScoring.numberOfTimesTouched);
        levelTransitioner.LoadNextLevel();
        //touchInput.numberOfTimesTouched = 0;
        currentLevel++;

        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;
        levelCompleteText.text = "Level Complete: " + levelPassed;

        FillEnemyArray();
    }

    public void LastLevel()
    {
        Debug.Log("Last Level");
        levelTransitioner.LoadLastLevel();
        //touchInput.numberOfTimesTouched = 0;
        currentLevel--;

        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;
        levelCompleteText.text = "Level Complete: " + levelPassed;

        FillEnemyArray();
    }

    private void SaveLevelOutcome()
    {
        //levelList[currentLevel]
    }

    public void SetText()
    {
        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;
        levelCompleteText.text = "Level Complete: " + levelPassed;  
    }

    public void FillEnemyArray()
    {
        Array.Clear(enemyArray,0,enemyArray.Length);
        enemyArray = loadLevelData.enemyArray;

    }

    public void EndOfSwipe(object sender, EventArgs e)
    {
        swipesLeft--;
        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft.ToString();
       
    }

    public void AddSwipe(int swipesAdded)
    {
        swipesLeft += swipesAdded;
        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft.ToString();
    }

    public void SpawnPlayer()
    {
        Instantiate(player, playerSpawn.position, Quaternion.identity); 
    }

    private void Update()
    {

        
        if(swipesLeft <= 0 && playerCollisionAndScoring.rb.IsSleeping() && !levelPassed)
        {
            Debug.Log("0 swipes left");
            levelTransitioner.ReloadLevel();
            if (currentLevel == 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
