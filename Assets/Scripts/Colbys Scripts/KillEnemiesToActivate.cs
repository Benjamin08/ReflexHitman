using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemiesToActivate : MonoBehaviour
{
   [SerializeField]
    private int killsRequireToActivate;

    public List<GameObject> ObjectToActivate;

    private LoadLevelData loadLevelData;

    // Start is called before the first frame update
    void Start()
    {
        loadLevelData = GameObject.FindGameObjectWithTag("loadLevelData").GetComponent<LoadLevelData>();
        loadLevelData.OnEnemyDeath += GetDeadEnemies;
        ActivateGameObjects(false);
    }


    private void GetDeadEnemies(object sender, LoadLevelData.OnEnemyDeathEventArgs e)
    {
        
        if(e.numberOfDeadEnemysLevelData.Equals(killsRequireToActivate))
        {
            ActivateGameObjects(true);
        }
    }

    private void ActivateGameObjects(bool active)
    {
        foreach (GameObject gameObject in ObjectToActivate)
        {
            if(gameObject.GetComponent<Collider2D>() != null) gameObject.GetComponent<Collider2D>().enabled = active;
            if(gameObject.GetComponent<SpriteRenderer>() != null) gameObject.GetComponent<SpriteRenderer>().enabled = active;
        }
    }

}
