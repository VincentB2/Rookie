using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class T10_Doors : MonoBehaviour
{
    public T10_IntVariable enemiesDead;
    public int goal;


    private void Update()
    {
            if (enemiesDead.Value >= goal)
            {
                gameObject.SetActive(false);
            }
    }
}