using UnityEngine;
class T10_Projectile : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, 2);
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
}