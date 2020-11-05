using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class T10_Doors : MonoBehaviour
{
    public T10_IntVariable enemiesDead;
    public T10_Doors doors;
    public enum DOORS {UN, DEUX, TROIS, QUATRE, CINQ}
    public DOORS doorID;
    public List<int> goal = new List<int>();
    public List<bool> isOpen = new List<bool>();


    private void Update()
    {
        if (doorID == DOORS.UN)
        {
            if (isOpen[0])
            {
                if (enemiesDead.Value >= goal[0])
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else if (doorID == DOORS.DEUX)
        {
            if (isOpen[1])
            {
                if (enemiesDead.Value == goal[1])
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else if (doorID == DOORS.TROIS)
        {
            if (isOpen[2])
            {
                if (enemiesDead.Value == goal[2])
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else if (doorID == DOORS.QUATRE)
        {
            if (isOpen[3])
            {
                if (enemiesDead.Value == goal[3])
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else if (doorID == DOORS.CINQ)
        {
            if (isOpen[4])
            {
                if (enemiesDead.Value == goal[4])
                {
                    gameObject.SetActive(false);
                }
            }
        }


    }
}