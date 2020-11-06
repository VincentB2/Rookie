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
    private int lastFrameLife;
    public int damageEnemy = 1;
    public GameObject heartList;
    private GameObject hearts;
    // Julien
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
    
    //Vincent
    public bool isSlowed;
    private GameObject enemyCount;
    private bool canLoot = true;
    void Start()
    {
        hearts = Instantiate(heartList, transform.position + new Vector3(0, 1), heartList.transform.rotation);
        for (int i = 0; i < lifeEnemy; i++) {
            hearts.GetComponent<T10_LifeEnemy>().AddHeart();
        }
        hearts.GetComponent<T10_LifeEnemy>().SetHearts();
        hearts.transform.position = transform.position + new Vector3(-0.1f, 0.5f);
        player = GameObject.FindGameObjectWithTag("Player");
        enemyCount = GameObject.Find("EnemyCount");
        // Look At
        if (player)
        {
            target = player.GetComponent<Transform>();
        }

        lastFrameLife = lifeEnemy;
    }
    void Update()
    {
        hearts.transform.position = transform.position + new Vector3(-0.1f, 0.6f);
        if(lifeEnemy != lastFrameLife)
        {
            int damages = lastFrameLife - lifeEnemy;
            for (int i = 0; i < damages; i++)
            {
                hearts.GetComponent<T10_LifeEnemy>().RemoveHearts();
            }
        }
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
                base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * enemyShootReactivity);
                transform.position = Vector2.MoveTowards(transform.position, target.position, speedEnemy * Time.deltaTime);
            }
            else if ((player.transform.position - transform.position).magnitude <= rangeEnemyShoot)
            {
                thisPos = transform.position;
                targetPos = target.position;
                targetPos.x = targetPos.x - thisPos.x;
                targetPos.y = targetPos.y - thisPos.y;
                angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
                base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * enemyShootReactivity);
                if (enemyShootProjectile)
                {
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        GameObject projectile = Instantiate(enemyShootProjectile, transform.position, base.transform.rotation);
                        projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.right * enemyShootProjectileSpeed * 100);
                        projectile.GetComponent<T10_Projectile>().enemyShootDamage = damageEnemy;
                        timer = enemyShootFrequency;
                    }
                }
            }
        }
        else
        {
            Debug.Log((player.transform.position - transform.position).magnitude);
            // Alex
            if ((player.transform.position - transform.position).magnitude <= rangeEnemy && enemyType != EnemyType.SHOOT)
            {
                RotateGameObject(player.transform.position, 50f, -90f);
                //transform.position = Vector2.MoveTowards(transform.position, target.position, speedEnemy * Time.deltaTime);
                GetComponent<Rigidbody2D>().velocity = (target.position - transform.position).normalized * speedEnemy;
                Debug.Log("should run");

            }
        }
        lastFrameLife = lifeEnemy;
        EnemyDeath();
    }
    void EnemyDeath()
    {
       
        if (lifeEnemy <= 0)
        {
            Destroy(hearts);
            enemyCount.GetComponent<T10_EnemyCount>().EnemiesDead.Value++;
            int randEmoji = Random.Range(0, 2);
            int randLoot = Random.Range(0, 5);
            int randBig = Random.Range(0, 5);
            if (canLoot) {
                if (randLoot < 4)
                {
                    if (enemyType == EnemyType.NORMAL)
                    {
                        GameObject newEmoji = Instantiate(emoji, transform.position, transform.rotation);
                        if (randEmoji == 0)
                        {
                            newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.HeartEyes;
                            newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[2];
                            Destroy(newEmoji, 10);
                        }
                        else
                        {
                            newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.Joy;
                            newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[3];
                            Destroy(newEmoji, 10);
                        }
                    }
                    else if (enemyType == EnemyType.BIG)
                    {
                        GameObject newEmoji = Instantiate(emoji, transform.position, transform.rotation);
                        if (randBig < 3)
                        {
                            newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.Rage;
                            newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[4];
                            Destroy(newEmoji, 10);
                        }
                        else
                        {
                            newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.Mad;
                            newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[6];
                            Destroy(newEmoji, 10);
                        }
                    }
                    else if (enemyType == EnemyType.SMALL)
                    {
                        GameObject newEmoji = Instantiate(emoji, transform.position, transform.rotation);
                        if (randEmoji == 0)
                        {
                            newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.ColdFace;
                            newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[0];
                            Destroy(newEmoji, 10);
                        }
                        else
                        {
                            newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.Scream;
                            newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[1];
                            Destroy(newEmoji, 10);
                        }
                    }
                    else if (enemyType == EnemyType.SHOOT)
                    {
                        GameObject newEmoji = Instantiate(emoji, transform.position, transform.rotation);
                        if (randEmoji == 0)
                        {
                            newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.SmilingImp;
                            newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[7];
                            Destroy(newEmoji, 10);
                        }
                        else
                        {
                            newEmoji.GetComponent<T10_Emoji>().emojiType = T10_Emoji.Type.SmilingImp;
                            newEmoji.GetComponent<SpriteRenderer>().sprite = newEmoji.GetComponent<T10_Emoji>().emojiSprite[7];
                            Destroy(newEmoji, 10);
                        }
                    }
                }
            }
            FindObjectOfType<T10_AudioManager>().Play("enemyDeath");
            
            Destroy(transform.gameObject);
        }
    }
    private void RotateGameObject(Vector3 target, float RotationSpeed, float offset)
    {
        Vector3 dir = target - base.transform.position;
        //get the angle from current direction facing to desired target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //set the angle into a quaternion + sprite offset depending on initial sprite facing direction
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        //Roatate current game object to face the target using a slerp function which adds some smoothing to the move
        base.transform.rotation = Quaternion.Slerp(base.transform.rotation, rotation, RotationSpeed * Time.deltaTime);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<T10_PlayerFight>().TakeDamage(damageEnemy);
            lifeEnemy -= 2;
            if(lifeEnemy <= 0)
            {
                canLoot = false;
            }
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