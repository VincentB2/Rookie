using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_ArrowScript : MonoBehaviour
{
    GameObject player;
    public float arrowDamage = 1f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")) 
        {
            player.GetComponent<T10_PlayerFight>().TakeDamage(arrowDamage);
            Destroy(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
