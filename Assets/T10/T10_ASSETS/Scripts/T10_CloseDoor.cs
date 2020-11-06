using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T10_CloseDoor : MonoBehaviour
{
    public T10_IntVariable enemiesDead;
    public GameObject door;
    public TextMeshProUGUI text;
    private bool isReset = false;

    public List<GameObject> spawners = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            if (collision.gameObject.tag == "Player" && !isReset)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<BoxCollider2D>().enabled = true;
                GetComponent<CapsuleCollider2D>().enabled = false;
                enemiesDead.Value = 0;
                text.text = door.GetComponent<T10_Doors>().goal.ToString();

                foreach (GameObject spawner in spawners)
                {
                    spawner.GetComponent<T10_EnemySpawn>().enabled = true;
                }
            isReset = true;
        
            
            }
    }
}
