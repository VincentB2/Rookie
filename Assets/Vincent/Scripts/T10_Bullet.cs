
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_Bullet : MonoBehaviour

{
    GameObject player;
    public float damageBullet = 1f;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player.GetComponent<T10_MovementPlayer>().weapon == T10_MovementPlayer.Weapon.MITRAILLETTE) 
        {
            damageBullet = 1f;
        }else if(player.GetComponent<T10_MovementPlayer>().weapon == T10_MovementPlayer.Weapon.SHOTGUN)
        {
            damageBullet = 2f;
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
            col.gameObject.GetComponent<T10_EnemyAI>().lifeEnemy -= damageBullet;
            Debug.Log(col.gameObject.GetComponent<T10_EnemyAI>().lifeEnemy);
            Destroy(gameObject);
        }
    }
}