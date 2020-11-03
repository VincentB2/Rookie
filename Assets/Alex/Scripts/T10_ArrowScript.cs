using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_ArrowScript : MonoBehaviour
{
    GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")) 
        {
            player.GetComponent<T10_PlayerFight>().playerHP -= 1;
            Destroy(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
