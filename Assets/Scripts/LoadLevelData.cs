using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CodeMonkey.Utils;
using CodeMonkey;

public class LoadLevelData : MonoBehaviour
{
   
    public event EventHandler<OnEnemyDeathEventArgs> OnEnemyDeath;
    public class OnEnemyDeathEventArgs : EventArgs 
    {
        public int numberOfDeadEnemysLevelData;
    }

    public List<GameObject> enemyList;
    public GameObject[] enemyArray;

    public int deadCount = 0;

    public GameHandler gameHandler;
    
    public GameObject[] playerTypes;

    public LevelData thisLevelData;

    // Start is called before the first frame update
    void Start()
    {
        Color newColor = new Color(0.3f, 0.4f, 0.6f, 0.3f);
        deadCount = 0;
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemyList = new List<GameObject>(enemyArray);

        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        
        if(GameSettings.dragToMove)
        {
            gameHandler.player = playerTypes[0];
        }
        else
        {
            gameHandler.player = playerTypes[1];
        }
        
        gameHandler.levelTransitioner = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        gameHandler.playerCollisionAndScoring = gameHandler.player.GetComponent<PlayerCollisionsAndScoring>();
        gameHandler.levelCompleteText = GameObject.Find("Level Complete Text").GetComponent<Text>();
        gameHandler.numberOfSwipesText = GameObject.Find("Number Of Swipes").GetComponent<Text>();
        //gameHandler.touchInput = gameHandler.player.GetComponent<TouchTwo>();
        gameHandler.playerSpawn = GameObject.FindGameObjectWithTag("Player Spawn").GetComponent<Transform>();
        

        gameHandler.swipesLeft = thisLevelData.GetMaxNumberOfSwipes();
       
        gameHandler.levelPassed = false;

        gameHandler.loadLevelData = this;

        gameHandler.enemyArray = gameHandler.loadLevelData.enemyArray;

        if(gameHandler.currentLevel != 0)
        {
            if(GameSettings.dragToMove)
            {
                //gameHandler.player.GetComponent<DragNShoot>().OnSwipeDone += gameHandler.EndOfSwipe;
            }
            else
            {
                gameHandler.player.GetComponent<TouchTwo>().OnSwipeDone += gameHandler.EndOfSwipe;
            }
        }
        
        gameHandler.SpawnPlayer();

        gameHandler.SetText();

        //CMDebug.ButtonUI(new Vector2(300, -500),new Vector2(300,300), () => gameHandler.ResetGame());

        //CMDebug.ButtonUI(new Vector2(300, -500), new Vector2(300,300), "Reset", () => gameHandler.ResetGame());

        //CMDebug.ButtonUI(new Vector2(300, 0), "Next Level", () => gameHandler.NextLevel());

        //CMDebug.ButtonUI(new Vector2(300, -250), "Last Level", () => gameHandler.LastLevel());

    if(gameHandler.currentLevel == 0)
    {
        Debug.Log("level 0");
        {
            FunctionTimer.Create(() => gameHandler.levelTransitioner.ColbyDisplay.GetComponent<CanvasGroup>().alpha = 1, 2f);
            
        }
    }
    else
    {
        gameHandler.levelTransitioner.ColbyDisplay.GetComponent<CanvasGroup>().alpha = 1f;
    }   

    }

    public void ResetEnemyDeaths(int resetAmount)
    {
        deadCount -= resetAmount;
        OnEnemyDeath?.Invoke(this, new OnEnemyDeathEventArgs { numberOfDeadEnemysLevelData = deadCount }); // Invokes event if its not null
    }

    public void TriggerEvent()
    {
        deadCount++;
        OnEnemyDeath?.Invoke(this, new OnEnemyDeathEventArgs{ numberOfDeadEnemysLevelData = deadCount }); // Invokes event if its not null
    }


    
}