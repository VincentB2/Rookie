﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFight : MonoBehaviour
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
            Debug.Log("vous êtes mort");
        }
    }
}
