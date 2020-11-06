using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_EnemySpawn : MonoBehaviour
{
    private GameObject player;
    public float rangeSpawn = 10.0f;
    public float timeBeforeSpawnValue = 2.0f;
    private float timeBeforeSpawn;
    public GameObject enemyType;
    public GameObject door;
    public T10_IntVariable nbrEnemySpawn;
    private int enemyToKill;
    
    // Start is called before the first frame update
    void Start()
    {
        nbrEnemySpawn.Value = 0;
       player = GameObject.FindGameObjectWithTag("Player");
        timeBeforeSpawn = timeBeforeSpawnValue;
        enemyToKill = door.GetComponent<T10_Doors>().goal - door.GetComponent<T10_Doors>().numberOfSniper;
    }

    // Update is called once per frame
    void Update()
    {
        if (nbrEnemySpawn.Value < enemyToKill)
        {
            
            if ((player.transform.position - transform.position).magnitude <= rangeSpawn)
            {
                if (timeBeforeSpawn <= 0)
                {
                    Instantiate(enemyType, transform.position, transform.rotation);
                    timeBeforeSpawn = timeBeforeSpawnValue;
                    nbrEnemySpawn.Value++;
                }
                else
                {
                    timeBeforeSpawn -= Time.deltaTime;
                }
            }
        }
        Debug.Log(enemyToKill);
    }

}


