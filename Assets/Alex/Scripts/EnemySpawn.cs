using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private GameObject player;
    public float rangeSpawn = 10.0f;
    public float timeBeforeSpawnValue = 2.0f;
    private float timeBeforeSpawn = 0.0f;
    public GameObject enemyType;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - transform.position).magnitude <= rangeSpawn)
        {
            if(timeBeforeSpawn <= 0) 
            {
                Instantiate(enemyType, transform.position, transform.rotation);
                timeBeforeSpawn = timeBeforeSpawnValue;
            }
            else 
            {
                timeBeforeSpawn -= Time.deltaTime;
            }
        }
    }
}
