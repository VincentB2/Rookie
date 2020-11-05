
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_Bullet : MonoBehaviour

{
    GameObject player;
    public float damageBullet = 2f;
    public enum BULLETS { DEFAULT, GLACE, SNIPER, GRENADE }
    public BULLETS bulletType;

    T10_CameraController camControl;
    public float shakeDur = 0.1f;
    public float shakeAm = 1f;

    public GameObject bomb;
    private bool canExplode = true;
    public FloatVariable delayBeforeExplosion;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camControl = GameObject.Find("/Camera").GetComponent<T10_CameraController>();
        if (bulletType == BULLETS.SNIPER)
        {
            damageBullet = 3f;
        }
        else if (bulletType == BULLETS.GRENADE)
        {
            damageBullet = 0;
            StartCoroutine("ExplodeAfterDelay");
        }
        else
        {
            damageBullet = 1f;
        }

        if (bulletType == BULLETS.GLACE)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        Destroy(gameObject, 2);
    }

    private void Update()
    {
        if (bulletType == BULLETS.GRENADE && Input.GetMouseButtonUp(0))
        {
            Explode();
        }
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
            if (bulletType == BULLETS.GLACE)
            {
                if (!scriptEnemy.isSlowed)
                {
                    scriptEnemy.StartCoroutine("BeSlow");
                }
                else
                {
                    scriptEnemy.StartCoroutine("AlreadySlowed");
                }
            } else if(bulletType == BULLETS.GRENADE)
            {
                Explode();
            }

            

            if (bulletType != BULLETS.SNIPER)
            {
                Destroy(gameObject);
            }
        }

        
    }

    private void Explode()
    {
        if (canExplode)
        {
            canExplode = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Instantiate(bomb, transform.position, transform.rotation);
        }
        
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeExplosion.Value);
        Explode();
    }
}