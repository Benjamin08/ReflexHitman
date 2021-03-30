using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWall : MonoBehaviour
{
    public bool unlockedWithKills;   

    public int killsRequireToActivate;
    

    private Rigidbody2D wall;

    [Range(0f, 100f)]
    public float rotateSpeed;

    private LoadLevelData loadLevelData;

    // Start is called before the first frame update
    void Start()
    {
        wall = GetComponent<Rigidbody2D>();
        loadLevelData = GameObject.FindGameObjectWithTag("loadLevelData").GetComponent<LoadLevelData>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        wall.MoveRotation(wall.rotation + rotateSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        
    }
}




