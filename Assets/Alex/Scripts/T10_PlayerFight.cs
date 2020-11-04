using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class T10_PlayerFight : MonoBehaviour
{
    public int playerHPValue = 3;
    public int playerHP = 0;
    // Start is called before the first frame update
    void Awake()
    {
        playerHP = playerHPValue;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerHP);
        if(playerHP <= 0) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
