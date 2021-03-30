using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
 
    public LevelData thisLevelData;

    // Start is called before the first frame update
    void Start()
    {
        deadCount = 0;
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemyList = new List<GameObject>(enemyArray);

        gameHandler = GameObject.FindGameObjectWithTag("GameHandler").GetComponent<GameHandler>();
        gameHandler.player = GameObject.FindGameObjectWithTag("Player");
        gameHandler.levelTransitioner = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        gameHandler.playerCollisionAndScoring = gameHandler.player.GetComponent<PlayerCollisionsAndScoring>();
        gameHandler.levelCompleteText = GameObject.Find("Level Complete Text").GetComponent<Text>();
        gameHandler.numberOfSwipesText = GameObject.Find("Number Of Swipes").GetComponent<Text>();


        gameHandler.swipesLeft = thisLevelData.GetMaxNumberOfSwipes();
       
        gameHandler.levelPassed = false;

        gameHandler.loadLevelData = this;


        if(gameHandler.currentLevel != 0)
        {
            gameHandler.player.GetComponent<TouchTwo>().OnSwipeDone += gameHandler.EndOfSwipe;
        }
        
        gameHandler.SetText();
    }

    public void TriggerEvent()
    {
        deadCount++;
        OnEnemyDeath?.Invoke(this, new OnEnemyDeathEventArgs{ numberOfDeadEnemysLevelData = deadCount }); // Invokes event if its not null
    }
}