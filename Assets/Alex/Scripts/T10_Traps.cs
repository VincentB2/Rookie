using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T10_Traps : MonoBehaviour
{
    public enum TrapType{
        DOWN,
        UP,
        LEFT,
        RIGHT
    }
    public TrapType trap;
    public float timeBeforeArrowValue = 1.0f;
    private float timeBeforeArrow;
    public float arrowSpeed = 150.0f;
    public GameObject[] arrowSpawns;
    public GameObject arrow;
    int randomSpawn;

    Quaternion quat;
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeArrow = timeBeforeArrowValue;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBeforeArrow <= 0)
        {
            if (trap == TrapType.DOWN)
            {
                GameObject iceArrow = Instantiate(arrow, arrowSpawns[randomSpawn].transform.position, transform.rotation);
                //quat =
                iceArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -arrowSpeed));
                iceArrow.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if (trap == TrapType.UP)
            {
                GameObject iceArrow = Instantiate(arrow, arrowSpawns[randomSpawn].transform.position, transform.rotation);
                iceArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, arrowSpeed));
                iceArrow.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else if (trap == TrapType.LEFT)
            {
                GameObject iceArrow = Instantiate(arrow, arrowSpawns[randomSpawn].transform.position, transform.rotation);
                iceArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(-arrowSpeed, 0));
                iceArrow.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if (trap == TrapType.RIGHT)
            {
                GameObject iceArrow = Instantiate(arrow, arrowSpawns[randomSpawn].transform.position, transform.rotation);
                iceArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(arrowSpeed, 0));
                iceArrow.transform.eulerAngles = new Vector3(0f, 0f, -90f);
            }
            timeBeforeArrow = timeBeforeArrowValue;
        }
        else 
        {
            timeBeforeArrow -= Time.deltaTime;
            randomSpawn = Random.Range(0, arrowSpawns.Length);
        }
        
    }
}
