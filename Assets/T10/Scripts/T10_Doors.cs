using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class T10_Doors : MonoBehaviour
{
    int i = 0;
    public GameObject[] doors;
    public int compteur = 2;
    public int compteurAfter = 15;
    // Start is called before the first frame update
    void Start() { }
    // Update is called once per frame
    void Update()
    {
        if (i < doors.Length)
        {
            if (PlayerPrefs.GetInt("count") >= compteur)
            {
                if (i == 0)
                {
                    doors[i].SetActive(false);
                    compteur = compteurAfter;
                }
                else if (i > 0)
                {
                    doors[i - 1].SetActive(true);
                    doors[i].SetActive(false);
                }
                i++;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("count", 0);
        }
    }
}