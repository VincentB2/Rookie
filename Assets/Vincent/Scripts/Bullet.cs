using UnityEngine;
class Bullet : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, 2);
    }
}