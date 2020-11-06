using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class T10_Doors : MonoBehaviour
{
    public T10_IntVariable enemiesDead;
    public int goal;
    public int numberOfSniper;
    public List<GameObject> spawners = new List<GameObject>();


    private void Update()
    {
            if (enemiesDead.Value >= goal)
            {

            foreach (GameObject spawner in spawners)
            {
                spawner.GetComponent<T10_EnemySpawn>().enabled = false;
            }
            gameObject.SetActive(false);

            
            }
    }
}