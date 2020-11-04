
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_Bullet : MonoBehaviour

{
    void Awake()
    {
        Destroy(gameObject, 2);
    }
}