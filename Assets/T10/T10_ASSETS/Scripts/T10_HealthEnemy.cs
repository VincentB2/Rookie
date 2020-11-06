using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class T10_HealthEnemy : MonoBehaviour
{

    //----------------------------HEALTH VARIABLES-----------
    public Image[] hearts;
    public Sprite heart;
    T10_EnemyAI enemy;
    public GameObject[] heartsGO;
    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<T10_EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {

        //-----------------------------HEALTH ENEMY-----------------------------
        //for (int i = 0; i < hearts.Length; i++)
        // {

        // if(i < enemy.lifeEnemy)
        //     {
        //         hearts[i].enabled = true;
        //         heartsGO[i].SetActive(true);
        //     }
        //     else
        //     {
        //         hearts[i].enabled = false;
        //         heartsGO[i].SetActive(false);
        //     }
         //}
    }
}
