using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class T10_Bullet : MonoBehaviour
{
    GameObject player;
    public int damageBullet = 2;
    public enum BULLETS { DEFAULT, SHOTGUN, GLACE, SNIPER, GRENADE }
    public BULLETS bulletType;
    T10_CameraController camControl;
    public float shakeDur = 0.1f;
    public float shakeAm = 1f;
    public GameObject bomb;
    private bool canExplode = true;
    public FloatVariable delayBeforeExplosion;
    public Animator camera;
    void Awake()
    {
        camera = GameObject.Find("Camera").GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        camControl = GameObject.Find("/Camera").GetComponent<T10_CameraController>();
        if (bulletType == BULLETS.SNIPER)
        {
            damageBullet = 3;
        }
        else if (bulletType == BULLETS.GRENADE)
        {
            damageBullet = 0;
            StartCoroutine("ExplodeAfterDelay");
        }
        else if (bulletType == BULLETS.GLACE)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            damageBullet = 1;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Julien
        if (col.gameObject.CompareTag("Wall"))
        {
            if(bulletType == BULLETS.GRENADE)
            {
                Explode();
            }
            Destroy(gameObject);
        }
           
        if (col.CompareTag("Enemy"))
        {
            camera.SetTrigger("enemyHit");
            T10_EnemyAI scriptEnemy = col.gameObject.GetComponent<T10_EnemyAI>();
            scriptEnemy.lifeEnemy -= damageBullet;
            camControl.ShakeCamera(shakeDur, shakeAm);

            // anim camera
            if (bulletType != BULLETS.GRENADE && bulletType != BULLETS.SHOTGUN) {
                camera.SetTrigger("enemyHit");
            }
            else if (bulletType == BULLETS.SHOTGUN)
            {
                camera.SetTrigger("shotgun");
            }

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
            }
            else if (bulletType == BULLETS.GRENADE)
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