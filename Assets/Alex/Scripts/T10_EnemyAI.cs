﻿using System.Collections;
using UnityEngine;
public class T10_EnemyAI : MonoBehaviour
{
    public GameObject player;
    public enum EnemyType
    {
        NORMAL,
        SMALL,
        BIG,
        SHOOT
    }
    public EnemyType enemyType;
    [Header("EnemyCarac")]
    public float speedEnemy = 1.5f;
    public float rangeEnemy = 4.0f;
    public float lifeEnemy = 1.5f;
    public int damageEnemy = 1;
    // Julien
    // Look At
    Transform target;
    Vector3 thisPos;
    Vector3 targetPos;
    float angle;
    // EnemyShoot
    [Header("EnemyShoot")]
    public GameObject enemyShootProjectile;
    public float rangeEnemyShoot = 10.0f;
    public float enemyShootForce = 1,
        enemyShootReactivity = 50f,
        enemyShootProjectileSpeed = 5,
        enemyShootFrequency = .5F;
    float timer;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Julien
        // Look At
        if (player)
            target = player.GetComponent<Transform>();
    }
    void Update()
    {

        // Julien
        if (enemyType == EnemyType.SHOOT)
        {
            // Look At
            if ((player.transform.position - transform.position).magnitude <= rangeEnemy && (player.transform.position - transform.position).magnitude > rangeEnemyShoot)
            {
                thisPos = transform.position;
                targetPos = target.position;
                targetPos.x = targetPos.x - thisPos.x;
                targetPos.y = targetPos.y - thisPos.y;
                angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * enemyShootReactivity);
                transform.position = Vector2.MoveTowards(transform.position, target.position, speedEnemy * Time.deltaTime);
            }
            else if ((player.transform.position - transform.position).magnitude <= rangeEnemyShoot)
            {
                thisPos = transform.position;
                targetPos = target.position;
                targetPos.x = targetPos.x - thisPos.x;
                targetPos.y = targetPos.y - thisPos.y;
                angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * enemyShootReactivity);
                if (enemyShootProjectile)
                {
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        GameObject projectile = Instantiate(enemyShootProjectile, transform.position, transform.rotation);
                        projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.right * enemyShootProjectileSpeed * 100);
                        projectile.GetComponent<T10_Projectile>().enemyShootDamage = damageEnemy;
                        timer = enemyShootFrequency;
                    }
                }
            }
        }
        else
        {
            // Alex
            float step = speedEnemy * Time.deltaTime;
            if ((player.transform.position - transform.position).magnitude <= rangeEnemy)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
            }
        }
        EnemyDeath();
    }
    void EnemyDeath()
    {
        if (lifeEnemy <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void RotateGameObject(Vector3 target, float RotationSpeed, float offset)
    {
        Vector3 dir = target - transform.position;
        //get the angle from current direction facing to desired target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //set the angle into a quaternion + sprite offset depending on initial sprite facing direction
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        //Roatate current game object to face the target using a slerp function which adds some smoothing to the move
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Wtf");
            col.gameObject.GetComponent<T10_PlayerFight>().TakeDamage(damageEnemy);
            lifeEnemy -= 1.5f;
        }
    }
}