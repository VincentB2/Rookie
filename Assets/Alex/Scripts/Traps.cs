using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
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
    public float arrowSpeed = 50.0f;
    public GameObject[] arrowSpawns;
    public GameObject arrow;
    int randomSpawn;
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
                iceArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(arrowSpeed, 0));
                //iceArrow.GetComponent<Rigidbody2D>().velocity = iceArrow.transform.forward * iceArrow.GetComponent<ArrowScript>().arrowSpeed;
            }
            else if (trap == TrapType.UP)
            {
                GameObject iceArrow = Instantiate(arrowSpawns[randomSpawn], arrowSpawns[randomSpawn].transform.position, transform.rotation);
            }
            else if (trap == TrapType.LEFT)
            {
                GameObject iceArrow = Instantiate(arrowSpawns[randomSpawn], arrowSpawns[randomSpawn].transform.position, transform.rotation);
            }
            else if (trap == TrapType.RIGHT)
            {
                GameObject iceArrow = Instantiate(arrowSpawns[randomSpawn], arrowSpawns[randomSpawn].transform.position, transform.rotation);
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
