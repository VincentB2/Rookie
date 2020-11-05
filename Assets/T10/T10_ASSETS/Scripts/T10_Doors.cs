using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class T10_Doors : MonoBehaviour
{
    public T10_IntVariable enemiesDead;
    public enum DOORS {UN, DEUX, TROIS, QUATRE, CINQ}
    public DOORS doorID;
    public int goal;


    private void Update()
    {
        if (doorID == DOORS.UN)
        {
            if (enemiesDead.Value >= goal)
            {
                gameObject.SetActive(false);
            }
        }
        else if (doorID == DOORS.DEUX)
        {
            if (enemiesDead.Value == goal)
            {
                gameObject.SetActive(false);
            }
        }
        else if (doorID == DOORS.TROIS)
        {
            if (enemiesDead.Value == goal)
            {
                gameObject.SetActive(false);
            }
        }
        else if (doorID == DOORS.QUATRE)
        {
            if (enemiesDead.Value == goal)
            {
                gameObject.SetActive(false);
            }
        }
        else if (doorID == DOORS.CINQ)
        {
            if (enemiesDead.Value == goal)
            {
                gameObject.SetActive(false);
            }
        }


    }
}