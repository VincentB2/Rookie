
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_Bullet : MonoBehaviour

{
    GameObject player;
    public float damageBullet = 2f;
    public bool isGlace = false;

    T10_CameraController camControl;
    public float shakeDur = 0.1f;
    public float shakeAm = 1f;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camControl = GameObject.Find("/Camera").GetComponent<T10_CameraController>();
        if(player.GetComponent<T10_MovementPlayer>().weapon == T10_MovementPlayer.Weapon.MITRAILLETTE) 
        {
            damageBullet = 1f;
        }else if(player.GetComponent<T10_MovementPlayer>().weapon == T10_MovementPlayer.Weapon.SHOTGUN)
        {
            damageBullet = 1f;
        }
        else 
        {
            damageBullet = 1f;
        }
        Destroy(gameObject, 2);
    }
    public void TakeDamage(float life, float dmg) 
    {
        life -= dmg;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy")) 
        {
            T10_EnemyAI scriptEnemy = col.gameObject.GetComponent<T10_EnemyAI>();
            scriptEnemy.lifeEnemy -= damageBullet;
            camControl.ShakeCamera(shakeDur, shakeAm);
            Debug.Log(scriptEnemy.lifeEnemy);
            Destroy(gameObject);
            if (isGlace)
            {
                if (!scriptEnemy.isSlowed)
                {
                    scriptEnemy.StartCoroutine("BeSlow");
                }
                else
                {
                    scriptEnemy.StartCoroutine("AlreadySlowed");
                }
            }
        }

        
    }
}