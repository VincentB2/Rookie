using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_CloseDoor : MonoBehaviour
{
    public T10_IntVariable enemiesDead;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent < CapsuleCollider2D>().enabled = false;
            enemiesDead.Value = 0;
        }
    }
}
