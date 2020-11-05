using UnityEngine;
class T10_Projectile : MonoBehaviour
{
    public float enemyShootDamage = 1f;
    void Awake()
    {
        Destroy(gameObject, 2);
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            c.gameObject.GetComponent<T10_PlayerFight>().TakeDamage(enemyShootDamage);
            Destroy(gameObject);
        }
        if (c.gameObject.CompareTag("Wall"))
            Destroy(gameObject);
    }
}