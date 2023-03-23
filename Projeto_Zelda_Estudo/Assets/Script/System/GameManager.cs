using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyState 
{
    IDLE, ALERT, PATROL, EXPLORE, FOLLOW, FURY
}



public class GameManager : MonoBehaviour
{
    [Header("Slime AI")]
    public Transform[] slimeWayPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
