<<<<<<< HEAD:Assets/Vincent/Scripts/T10_Bullet.cs
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_Bullet : MonoBehaviour
=======
﻿using UnityEngine;
class Bullet : MonoBehaviour
>>>>>>> 6b002a0eb9d196a1444b4a02a12e3a42280a412e:Assets/Vincent/Scripts/Bullet.cs
{
    void Awake()
    {
        Destroy(gameObject, 2);
    }
}