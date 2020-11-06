using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class T10_EnemyCount : MonoBehaviour
{
    public T10_IntVariable EnemiesDead;
    public TextMeshProUGUI text;

    private void Start()
    {
        EnemiesDead.Value = 0;
    }

    private void Update()
    {
        text.text = EnemiesDead.Value.ToString();
    }
}
