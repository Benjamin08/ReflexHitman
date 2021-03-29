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

    private Transform playerSpawn;

    public GameObject player;

    private TouchTwo touchInput;
    public PlayerCollisionsAndScoring playerCollisionAndScoring;

   // public List<GameObject>EnemyList;
    private GameObject[] enemyArray;
    public LevelData[] levelList;

    public LevelLoader levelTransitioner;

    //public LevelData currentLevelData;

    public EnemyData enemyData;

    public void ResetGame()
    {
        player.GetComponent<Rigidbody2D>().drag = 2000f;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Transform>().position = playerSpawn.position;
        player.GetComponent<TouchTwo>().OnSwipeDone += EndOfSwipe;


        swipesLeft = levelList[currentLevel].maxNumberOfSwipes;

        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;
        levelCompleteText.text = "Level Complete: " + levelPassed;

        foreach (GameObject enemy in enemyArray)
        {
            enemy.SetActive(true);
        }

        FunctionTimer.Create(() => player.GetComponent<PlayerCollisionsAndScoring>().SetDrag(1f), .3f);

    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();

        levelTransitioner = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();

        enemyData = GameObject.FindGameObjectWithTag("EnemyData").GetComponent<EnemyData>();

        enemyArray = enemyData.enemyArray;

        //camera.Setup(() => cameraLookPoint.position);

        CMDebug.ButtonUI(new Vector2(300, -500), "Reset", () => ResetGame());

        CMDebug.ButtonUI(new Vector2(300, 0), "Next Level", () => NextLevel());
        
        swipesLeft = levelList[currentLevel].maxNumberOfSwipes;

        numberOfSwipesText = GameObject.Find("Number Of Swipes").GetComponent<Text>();

        levelCompleteText = GameObject.Find("Level Complete Text").GetComponent<Text>();

        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;

        levelCompleteText.text = "Level Complete: " + levelPassed;

        player = GameObject.FindGameObjectWithTag("Player");

        playerCollisionAndScoring = player.GetComponent<PlayerCollisionsAndScoring>();

        touchInput = player.GetComponent<TouchTwo>();

        touchInput.OnSwipeDone += EndOfSwipe;

        playerSpawn = GameObject.FindGameObjectWithTag("Player Spawn").transform;

    }


    public void NextLevel()
    {
       Debug.Log("Next Level");
        levelTransitioner.LoadNextLevel();
        currentLevel++;
        swipesLeft = levelList[currentLevel].maxNumberOfSwipes;
        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;
        levelCompleteText.text = "Level Complete: " + levelPassed;

       // enemyData = GameObject.FindGameObjectWithTag("EnemyData").GetComponent<EnemyData>();
       // player = GameObject.FindGameObjectWithTag("Player");

        FillEnemyArray();
    }

    public void SetText()
    {
        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;
        levelCompleteText.text = "Level Complete: " + levelPassed;  
    }

    public void FillEnemyArray()
    {
        Array.Clear(enemyArray,0,enemyArray.Length);
        enemyArray = enemyData.enemyArray;

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

    private void Update()
    {

        // bugged on second level thinks player is not moving the frame I swipe
        if(touchInput.endOfTouch && swipesLeft <= 0 && playerCollisionAndScoring.rb.IsSleeping() && !levelPassed)
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
