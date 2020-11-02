using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    public float speedEnemy1 = 1.0f;
    public float speedEnemy2;
    public float speedEnemy3;

    public float rangeEnemy1 = 4.0f;
    public float rangeEnemy2 = 4.0f;
    public float rangeEnemy3 = 4.0f;
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
        /*if(player.transform.position.x - transform.position.x <= rangeEnemy1 && player.transform.position.y - transform.position.y <= rangeEnemy1) 
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
        }*/
        if((player.transform.position - transform.position).magnitude <= rangeEnemy1) 
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
        }
        
    }
}
