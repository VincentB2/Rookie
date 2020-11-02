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
    
    public enum Weapon {MITRAILLETTE, SHOTGUN};

    public Weapon weapon;

    private bool canFire = true;
    public float cadence;
    public GameObject bullet;

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

        if (Input.GetMouseButton(0) && canFire)
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

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Arrow.GetComponent<RectTransform>().localPosition = ((cellScreenPosition + direction) / canvasScale);

    }


    IEnumerator Fire()
    {
        if (weapon == Weapon.MITRAILLETTE)
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

            yield return new WaitForSeconds(cadence * 0.1f);
            canFire = true;
        }

        if (weapon == Weapon.SHOTGUN)
        {
            canFire = false;
            Vector2 cellScreenPosition = transform.position;
            Vector2 direction1 = Camera.main.ScreenToWorldPoint(Arrow.transform.position) - transform.position;
            direction1 = direction1.normalized;
            direction1 *= 0.5f;
            
            Vector2 bulletPos1 = cellScreenPosition + direction1;

            Vector2 direction2 = Camera.main.ScreenToWorldPoint(Arrow.transform.GetChild(0).transform.position) - transform.position;
            direction2 = direction2.normalized;
            direction2 *= 0.5f;
            Vector2 bulletPos2 = cellScreenPosition + direction2;

            Vector2 direction3 = Camera.main.ScreenToWorldPoint(Arrow.transform.GetChild(1).transform.position) - transform.position;
            direction3 = direction3.normalized;
            direction3 *= 0.5f;

            Vector2 bulletPos3 = cellScreenPosition + direction3;

            GameObject newBullet = Instantiate(bullet, bulletPos1, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * 30, ForceMode2D.Impulse);

            GameObject newBullet1 = Instantiate(bullet, bulletPos2, transform.rotation);
            newBullet1.GetComponent<Rigidbody2D>().AddForce(direction2 * 30, ForceMode2D.Impulse);

            GameObject newBullet2 = Instantiate(bullet, bulletPos3, transform.rotation);
            newBullet2.GetComponent<Rigidbody2D>().AddForce(direction3 * 30, ForceMode2D.Impulse);

            yield return new WaitForSeconds(cadence * 0.5f);
            canFire = true;
        }
    }
}