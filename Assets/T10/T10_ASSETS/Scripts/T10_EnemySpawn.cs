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
    public T10_IntVariable enemiesDead;
    private int enemyToKill;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timeBeforeSpawn = timeBeforeSpawnValue;
        enemyToKill = door.GetComponent<T10_Doors>().goal;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesDead.Value < enemyToKill)
        {
            if ((player.transform.position - transform.position).magnitude <= rangeSpawn)
            {
                if (timeBeforeSpawn <= 0)
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
}


