﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class T10_Bomb : MonoBehaviour
{

    public FloatVariable duration;
    private float timeSave = 0;
    public FloatVariable rangeIncrease;
    private int damages = 10;
    T10_CameraController camControl;
    public FloatVariable shakeDur;
    public FloatVariable shakeAm;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        timeSave = Time.time;
        camControl = GameObject.Find("/Camera").GetComponent<T10_CameraController>();
        camControl.ShakeCamera(shakeDur.Value, shakeAm.Value);
        player = GameObject.FindGameObjectWithTag("Player");
        ReculPlayer();
        Destroy(gameObject, 2);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < timeSave + duration.Value)
        {
            transform.localScale += new Vector3(rangeIncrease.Value, rangeIncrease.Value);
            Debug.Log(timeSave);
        } else
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            T10_EnemyAI scriptEnemy = collision.gameObject.GetComponent<T10_EnemyAI>();
            scriptEnemy.lifeEnemy -= damages;
            
        }
    }

    private void ReculPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        
            T10_MovementPlayer playerScript = player.GetComponent<T10_MovementPlayer>();
            StartCoroutine(playerScript.Recul(direction, direction.magnitude / 20, 20 / direction.magnitude));
        

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 10);
    }
}