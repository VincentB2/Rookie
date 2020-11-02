using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;

    [Header("Speed")]
    public float speedEnemy1 = 1.5f;
    public float speedEnemy2 = 3.0f;
    public float speedEnemy3 = 0.5f;
    [Header("Range")]
    public float rangeEnemy1 = 4.0f;
    public float rangeEnemy2 = 4.0f;
    public float rangeEnemy3 = 4.0f;
    [Header("HP")]
    public float lifeEnemy1 = 1.5f;
    public float lifeEnemy2 = 3.0f;
    public float lifeEnemy3 = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float step = speedEnemy1 * Time.deltaTime;
        if (Input.GetButtonDown("Jump")) 
        {
            Debug.Log("Hello Bitch");
            FindObjectOfType<AudioManager>().Play("win");
        }
        if((player.transform.position - transform.position).magnitude <= rangeEnemy1) 
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
        }
        
    }
}
