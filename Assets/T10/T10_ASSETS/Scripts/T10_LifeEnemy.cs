using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_LifeEnemy : MonoBehaviour
{
    public List<GameObject> hearts = new List<GameObject>();
    public GameObject heartGO;

    public void SetHearts()
    {
        if(hearts.Count != 0)
        {
            for (int i = 0; i< hearts.Count; i++)
            {
                hearts[i].transform.position += new Vector3(0.17f * i, 0) ;
            }
        }
    }

    public void AddHeart()
    {
        GameObject newHeartGO = Instantiate(heartGO, transform.position, transform.rotation);
        newHeartGO.transform.parent = transform;
        hearts.Add(newHeartGO);
    }

    public void RemoveHearts()
    {
        if (hearts.Count != 0)
        {
            Destroy(hearts[hearts.Count - 1]);
            hearts.RemoveAt(hearts.Count - 1);
        }
    }
}
