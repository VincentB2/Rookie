using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class T10_Bomb : MonoBehaviour
{
    private Animator camera;
    public FloatVariable duration;
    private float timeSave = 0;
    public FloatVariable rangeIncrease;
    private int damages = 10;
    T10_CameraController camControl;
    public FloatVariable shakeDur;
    public FloatVariable shakeAm;
    private GameObject player;
    public GameObject shockWave;
    private bool isUpdate = false;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<T10_AudioManager>().Play("Boom");
        camera = GameObject.Find("Camera").GetComponent<Animator>();
        StartCoroutine("StartExplode");
    }

    // Update is called once per frame
    void Update()
    {
        if(isUpdate)
        {
            if (Time.time < timeSave + duration.Value)
            {
                transform.localScale += new Vector3(rangeIncrease.Value, rangeIncrease.Value);
                Debug.Log(timeSave);
            } else
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            camera.SetTrigger("bomb");
            T10_EnemyAI scriptEnemy = collision.gameObject.GetComponent<T10_EnemyAI>();
            scriptEnemy.lifeEnemy -= damages;
            
        }
    }

    IEnumerator StartExplode()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider2D>().enabled = true;

        timeSave = Time.time;
        camControl = GameObject.Find("/Camera").GetComponent<T10_CameraController>();
        camControl.ShakeCamera(shakeDur.Value, shakeAm.Value);
        player = GameObject.FindGameObjectWithTag("Player");
        isUpdate = true;
        Destroy(gameObject, 2);
        
    }

}
