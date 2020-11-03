using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementPlayerGlisse : MonoBehaviour
{

    // ------------------------------- FIRE
    private Vector2 direction;
    public Canvas canvas;
    public GameObject Arrow;
    
    public enum Weapon {MITRAILLETTE, SHOTGUN};
    public Weapon weapon;

    private bool canFire = true;
    public FloatVariable cadenceMITRAILLETTE;
    public FloatVariable cadenceSHOTGUN;
    public FloatVariable cadenceGenerale;
    public GameObject bullet;

    private Vector3 actualPos;
    private Vector3 targetPos;
    private bool recul = false;
    private float reculPower;
    float timeRecul = 0;
    float reculDistance;

    // ------------------------------- MOVE
    private bool canMove = true;
    public float speed;
    private Rigidbody2D rb;
    private bool canDash = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("space"))
        //{
        //    if(rb.velocity.magnitude != 0)
        //    {
        //        Debug.Log("dash");
        //        StartCoroutine("Dash");
        //    }
        //}

        Curseur();

        if (Input.GetMouseButton(0) && canFire)
        {
            StartCoroutine("Fire");

            actualPos = transform.position;
            

            //if (weapon == Weapon.MITRAILLETTE)
            //{
            //    reculPower = 0.005f;
            //    reculDistance = 0.2f;
            //    targetPos = transform.position - (new Vector3(direction.x, direction.y) * reculPower);
            //    recul = true;
            //}
            //if (weapon == Weapon.SHOTGUN)
            //{
            //    reculPower = 0.1f;
            //    reculDistance = 0.03f;
            //    targetPos = transform.position - (new Vector3(direction.x, direction.y) * reculPower);
            //    recul = true;
            //}
        }

        //if (recul)
        //{ 
        //        Recul(reculPower, actualPos, targetPos, reculDistance);

        //}




    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            float x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
            float y = Input.GetAxis("Vertical") * Time.fixedDeltaTime;


            rb.AddForce(new Vector2(x, y) * speed);
            rb.AddForce(-rb.velocity * 2);
        }

    }


    IEnumerator Dash()
    {
        canDash = false;
        canMove = false;
        rb.velocity = Vector2.zero;
        if(Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(new Vector2(-30, 0), ForceMode2D.Impulse);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(new Vector2(30, 0), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.18f);
        rb.velocity = Vector2.zero;
        canMove = true;
        //yield return new
        
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
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);

            Vector2 cellScreenPosition = transform.position;

            Vector2 bulletPos = cellScreenPosition + direction1;

            GameObject newBullet = Instantiate(bullet, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * 1000, ForceMode2D.Force);

            rb.AddForce(-direction * 1, ForceMode2D.Force);
            //Vector3 actualPos = transform.position;
            //Vector3 targetPos = transform.position - new Vector3(direction.x, direction.y);
            //transform.position = Vector3.Lerp(actualPos, targetPos, Time.deltaTime);

            yield return new WaitForSeconds(cadenceMITRAILLETTE.Value / cadenceGenerale.Value);
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

            //Vector3 actualPos = transform.position;
            //Vector3 targetPos = transform.position - (new Vector3(direction.x, direction.y) *10);
            //transform.position = Vector3.Lerp(actualPos, targetPos, Time.deltaTime);
            rb.AddForce(-direction * 10, ForceMode2D.Force);

            yield return new WaitForSeconds( cadenceSHOTGUN.Value / cadenceGenerale.Value);
            canFire = true;
        }
    }

    //void Recul (float puissance, Vector3 actualPos, Vector3 targetPos, float reculDistance)
    //{
    //    if(timeRecul < 1)
    //    {
    //        transform.position = Vector3.Lerp(actualPos, targetPos, timeRecul);
    //        timeRecul += reculDistance;
    //    }
    //    else
    //    {
    //        timeRecul = 0;
    //        recul = false;
    //    }
        
    //}
}