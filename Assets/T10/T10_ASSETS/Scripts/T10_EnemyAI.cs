using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class T10_EnemyAI : MonoBehaviour
{
    public GameObject player;
    public int count = 0;
    public GameObject emoji;
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
    public int lifeEnemy = 1;
    public int damageEnemy = 1;
    // Julien
    Transform target;
    Vector3 thisPos;
    Vector3 targetPos;
    float angle;
    // EnemyShoot
    [Header("EnemyShoot")]
    public GameObject enemyShootProjectile;
    public Transform enemyParent;
    public float rangeEnemyShoot = 10.0f;
    public float enemyShootForce = 1,
        enemyShootReactivity = 50f,
        enemyShootProjectileSpeed = 5,
        enemyShootFrequency = .5F;
    float timer;
    
    //Vincent
    public bool isSlowed;
    private GameObject enemyCount;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyCount = GameObject.Find("EnemyCount");
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
            if ((player.transform.position - enemyParent.position).magnitude <= rangeEnemy && (player.transform.position - enemyParent.position).magnitude > rangeEnemyShoot)
            {
                thisPos = enemyParent.position;
                targetPos = target.position;
                targetPos.x = targetPos.x - thisPos.x;
                targetPos.y = targetPos.y - thisPos.y;
                angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * enemyShootReactivity);
                enemyParent.position = Vector2.MoveTowards(enemyParent.position, target.position, speedEnemy * Time.deltaTime);
            }
            else if ((player.transform.position - enemyParent.position).magnitude <= rangeEnemyShoot)
            {
                thisPos = enemyParent.position;
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
                        GameObject projectile = Instantiate(enemyShootProjectile, enemyParent.position, transform.rotation);
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
            if ((player.transform.position - enemyParent.position).magnitude <= rangeEnemy && enemyType != EnemyType.SHOOT)
            {
                RotateGameObject(player.transform.position, 50f, -90f);
                enemyParent.position = Vector2.MoveTowards(enemyParent.position, target.position, speedEnemy * Time.deltaTime);
            }
        }
        EnemyDeath();
    }
    void EnemyDeath()
    {
        if (lifeEnemy <= 0)
        {
            int randEmoji = Random.Range(0, 2);
            int randLoot = Random.Range(0, 5);
            if (randLoot < 4)
            {
                if (enemyType == EnemyType.NORMAL)
                {
                    GameObject newEmoji = Instantiate(emoji, enemyParent.position, enemyParent.rotation);
                    if (randEmoji == 0)
                    {
                        newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.HeartEyes;
                        newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[2];
                        Destroy(newEmoji, 5);
                    }
                    else
                    {
                        newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.Joy;
                        newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[3];
                        Destroy(newEmoji, 5);
                    }
                }
                else if (enemyType == EnemyType.BIG)
                {
                    GameObject newEmoji = Instantiate(emoji, enemyParent.position, enemyParent.rotation);
                    if (randLoot < 4)
                    {
                        newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.Rage;
                        newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[4];
                        Destroy(newEmoji, 5);
                    }
                    else
                    {
                        newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.Mad;
                        newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[6];
                        Destroy(newEmoji, 5);
                    }
                }
                else if (enemyType == EnemyType.SMALL)
                {
                    GameObject newEmoji = Instantiate(emoji, enemyParent.position, enemyParent.rotation);
                    if (randEmoji == 0)
                    {
                        newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.ColdFace;
                        newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[0];
                        Destroy(newEmoji, 5);
                    }
                    else
                    {
                        newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.Scream;
                        newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[1];
                        Destroy(newEmoji, 5);
                    }
                }
                else if (enemyType == EnemyType.SHOOT)
                {
                    GameObject newEmoji = Instantiate(emoji, enemyParent.position, enemyParent.rotation);
                    if (randEmoji == 0)
                    {
                        newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.SmilingImp;
                        newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[7];
                        Destroy(newEmoji, 5);
                    }
                    else
                    {
                        newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.SmilingImp;
                        newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[7];
                        Destroy(newEmoji, 5);
                    }
                }
            }
            FindObjectOfType<T10_AudioManager>().Play("enemyDeath");
            Debug.Log("Compteur = " + PlayerPrefs.GetInt("count"));
            enemyCount.GetComponent<T10_EnemyCount>().EnemiesDead.Value++;
            Destroy(enemyParent.gameObject);
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
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<T10_PlayerFight>().TakeDamage(damageEnemy);
            lifeEnemy -= 2;
        }
    }
    public IEnumerator BeSlow()
    {
        speedEnemy /= 2;
        isSlowed = true;
        yield return new WaitForSeconds(3);
        speedEnemy *= 2;
        isSlowed = false;
    }
    public IEnumerator AlreadySlowed()
    {
        yield return new WaitForSeconds(3);
        speedEnemy *= 2;
        isSlowed = false;
    }
}