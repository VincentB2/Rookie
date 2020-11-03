using System.Collections;
using UnityEngine;
public class T10_EnemyAI : MonoBehaviour
{
    public GameObject player;
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
    public float enemyShootForce = 1,
        enemyShootReactivity = 5,
        enemyShootSpeed = 1;
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
        if (gameObject.tag == "EnemyShoot")
        {
            // Look At
            if (target)
            {
                thisPos = transform.position;
                targetPos = target.position;
                targetPos.x = targetPos.x - thisPos.x;
                targetPos.y = targetPos.y - thisPos.y;
                angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * enemyShootReactivity);
                transform.position = Vector2.MoveTowards(transform.position, target.position, enemyShootSpeed * Time.deltaTime);
            }
            // EnemyShoot
            if (enemyShootProjectile)
            {
                GameObject projectile = Instantiate(enemyShootProjectile, transform.position, transform.rotation);
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
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.GetComponent<T10_PlayerFight>().playerHP -= damageEnemy;
            Destroy(gameObject);
        }
    }
}