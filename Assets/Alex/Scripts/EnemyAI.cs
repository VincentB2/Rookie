using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;

    [Header("EnemyCarac")]
    public float speedEnemy = 1.5f;
    public float rangeEnemy = 4.0f;
    public float lifeEnemy = 1.5f;
    public int damageEnemy = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        float step = speedEnemy * Time.deltaTime;

        if((player.transform.position - transform.position).magnitude <= rangeEnemy) 
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
        }

        EnemyDeath();
    }

    void EnemyDeath()
    {
        if (lifeEnemy <= 0)
        {
            Destroy(gameObject);
        }
    }


    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerFight>().playerHP -= damageEnemy;
            Destroy(gameObject);
        }
    }

    
}
