using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementPlayer : MonoBehaviour
{

    // ------------------------------- FIRE
    private Vector2 direction;
    public Canvas canvas;
    public GameObject Arrow;
    public GameObject bullet;

    private bool canFire = true;
    public float cadence;


    // ------------------------------- MOVE
    public float speed;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if(rb.velocity.magnitude != 0)
            {
                Debug.Log("dash");
                StartCoroutine("Dash");
            }
        }

        Curseur();

        if (Input.GetMouseButton(0))
        {
            StartCoroutine("Fire");
        }
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
        float y = Input.GetAxis("Vertical") * Time.fixedDeltaTime;

        rb.velocity = new Vector2(x, y) * speed;

    }


    IEnumerator Dash()
    {
        speed *= 3;
        yield return new WaitForSeconds(0.3f);
        speed /= 3;
        
    }

    void Curseur()
    {
        float canvasScale = canvas.scaleFactor;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - transform.position;
        direction = direction.normalized;

        Vector2 screenPos = Camera.main.WorldToScreenPoint(Vector3.right * transform.localScale.x / 2.0f) - Camera.main.WorldToScreenPoint(Vector2.zero);
        direction *= screenPos.x - 80*canvasScale;
        Vector2 cellScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

        Arrow.GetComponent<RectTransform>().localPosition = ((cellScreenPosition + direction) / canvasScale);

    }


    IEnumerator Fire()
    {
        canFire = false;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        direction = direction.normalized;
        direction *= transform.localScale.x / (2 * transform.localScale.x);

        Vector2 cellScreenPosition = transform.position;

        Vector2 bulletPos = cellScreenPosition + direction;

        GameObject newBullet = Instantiate(bullet, bulletPos, transform.rotation);
        newBullet.GetComponent<Rigidbody2D>().AddForce(direction * 30, ForceMode2D.Impulse);

        yield return new WaitForSeconds(cadence);
        canFire = true;
    }
}