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
    
    public ParticleSystem particleSystem;
    public Text numberOfSwipesText;
    public Text levelCompleteText;

    public int swipesAllowedThisLevel;

    public int currentLevel = 0;

    public int swipesLeft;

    public bool levelPassed = false;

    private CameraScript camera;

    // These transforms are so the camera can folllow them at any point
    //public Transform cameraLookPoint;

    private Transform playerSpawn;

    public GameObject player;

    private TouchTwo touchInput;
    private PlayerCollisionsAndScoring playerCollisionAndScoring;

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

        Debug.Log("swiptes allowed this level: " + swipesAllowedThisLevel);

        swipesLeft = swipesAllowedThisLevel;
        Debug.Log("swipes now: " + swipesLeft);

        numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft;
        levelCompleteText.text = "Level Complete: " + levelPassed;


        FunctionTimer.Create(() => player.GetComponent<PlayerCollisionsAndScoring>().SetDrag(1f), .3f);

    }

    public void EnemyDeathParticles(Vector3 enemyPosition)
    {
        particleSystem.transform.position = enemyPosition;
        
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        particleSystem = GameObject.FindGameObjectWithTag("ParticleSystem").GetComponent<ParticleSystem>();

        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();

        levelTransitioner = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();

        enemyData = GameObject.FindGameObjectWithTag("EnemyData").GetComponent<EnemyData>();

        enemyArray = enemyData.enemyArray;

        //camera.Setup(() => cameraLookPoint.position);

        CMDebug.ButtonUI(new Vector2(300, -500), "Reset", () => ResetGame());

        CMDebug.ButtonUI(new Vector2(300, 0), "Next Level", () => NextLevel());

        swipesAllowedThisLevel = levelList[currentLevel].maxNumberOfSwipes;
        
        swipesLeft = swipesAllowedThisLevel;

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
       
        levelTransitioner.LoadNextLevel();
        currentLevel++;
        swipesLeft = levelList[currentLevel].maxNumberOfSwipes;
        levelPassed = false;
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

    private void EndOfSwipe(object sender, EventArgs e)
    {
        swipesLeft--;
       //numberOfSwipesText.text = "Number Of Swipes: " + swipesLeft; 
    }

    private void Update()
    {
        
    }

}
