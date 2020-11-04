using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class T10_MovementPlayer : MonoBehaviour
{

    // ------------------------------- FIRE
    private Vector2 direction;
    public Canvas canvas;
    public GameObject Arrow;
    
    public enum Weapon {DEFAULT, MITRAILLETTE, SHOTGUN};
    public Weapon weapon;
 

    // --------------------GUNS
    private bool canFire = true;
    public FloatVariable cadenceGenerale;
    
    // --------DEFAULT
    public FloatVariable cadenceDEFAULT;
    public FloatVariable reculGlaceDEFAULT;
    public FloatVariable PuissReculTerreDEFAULT;
    public FloatVariable DistReculTerreDEFAULT;
    // --------MITRAILLETTE
    public FloatVariable cadenceMITRAILLETTE;
    public FloatVariable reculGlaceMITRAILLETTE;
    public FloatVariable PuissReculTerreMITRAILLETTE;
    public FloatVariable DistReculTerreMITRAILLETTE;
    // --------SHOTGUN
    public FloatVariable cadenceSHOTGUN;
    public FloatVariable reculGlaceSHOTGUN;
    public FloatVariable DistReculTerreSHOTGUN;
    public FloatVariable PuissReculTerreSHOTGUN;

    // ---------------RECUL
    private Vector3 actualPos;
    private Vector3 targetPos;
    private bool recul = false;
    private float reculPower;
    float timeRecul = 0;
    float reculDistance;

    // ---------------------BULLETS
    public FloatVariable speedBullets;
    private enum BULLETS { DEFAULT, COLD};
    BULLETS bullets;
    public GameObject bulletInUse;

    // ------------------------------- MOVE
    public FloatVariable Speed;
    private Rigidbody2D rb;
    private bool canDash = true;

    // ------------------------------- SOL
    public bool isGlace = false;

    // ------------------------------- COLLIDER
    private Collider2D col;
    public LayerMask player;

    // ------------------------------- SMILEY

    public enum SMILEY {JOY, RAGE, COLDFACE, HEARTEYES, SLIGHTSMILE, SCREAM, SMILINGIMP };
    public SMILEY smiley;
    SMILEY lastSmiley;
    public FloatVariable SpeedIncrease;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        smiley = SMILEY.SLIGHTSMILE;
        lastSmiley = smiley;
        WhichSmiley(smiley);
        bullets = BULLETS.DEFAULT ;
        weapon = Weapon.DEFAULT;
    }

    // Update is called once per frame
    void Update()
    {
        if(smiley != lastSmiley)
        {
            if(lastSmiley == SMILEY.SCREAM)
            {
                Speed.Value /= SpeedIncrease.Value;
            }
            WhichSmiley(smiley);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, ~player);;
        if (hit)
        {
            if (hit.transform.gameObject.layer == 8)
            {
                timeRecul = 1;
                isGlace = true;
            }

        }
        else
        {
            isGlace = false;
        }

        if (!isGlace)
        {

            if (Input.GetKeyDown("space"))
            {
                if (rb.velocity.magnitude != 0)
                {
                    Debug.Log("dash");
                    StartCoroutine("Dash");
                }
            }
            
            Curseur();

            if (Input.GetMouseButton(0) && canFire)
            {
                StartCoroutine("Fire");

                actualPos = transform.position;

                timeRecul = 0;

                Vector3 dir = direction.normalized;
                Vector3 vit = rb.velocity.normalized;
                if (weapon == Weapon.DEFAULT)
                {
                    reculDistance = DistReculTerreDEFAULT.Value;
                    reculPower = PuissReculTerreDEFAULT.Value;
                    targetPos = transform.position - ((new Vector3(direction.x, direction.y) * reculPower) + (dir - vit) / 4);
                    recul = true;
                }
                else if (weapon == Weapon.MITRAILLETTE)
                {
                    reculDistance = DistReculTerreMITRAILLETTE.Value;
                    reculPower = PuissReculTerreMITRAILLETTE.Value;
                    targetPos = transform.position - ((new Vector3(direction.x, direction.y) * reculPower) + (dir - vit)/4);
                    recul = true;
                }
                else if (weapon == Weapon.SHOTGUN)
                {
                    reculPower = PuissReculTerreSHOTGUN.Value;
                    reculDistance = DistReculTerreSHOTGUN.Value;
                    targetPos = transform.position - ((new Vector3(direction.x, direction.y) * reculPower) + (dir-vit)) ;
                    recul = true;
                }
            }

            if (recul)
            {
                Recul(reculPower, actualPos, targetPos, reculDistance);

            }

        }

        if (isGlace)
        {
            Curseur();

            if (Input.GetMouseButton(0) && canFire)
            {
                StartCoroutine("FireGlace");

                actualPos = transform.position;
            }
        }

        lastSmiley = smiley;
    }

    private void FixedUpdate()
    {
        if (!isGlace)
        {
            float x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
            float y = Input.GetAxis("Vertical") * Time.fixedDeltaTime;

            rb.velocity = new Vector2(x, y) * Speed.Value;
        }

        if (isGlace)
        {
  
            float x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
            float y = Input.GetAxis("Vertical") * Time.fixedDeltaTime;


            rb.AddForce(new Vector2(x, y) * Speed.Value);
            rb.AddForce(-rb.velocity * 2);
            
        }
        

    }

    void Curseur()
    {
        float canvasScale = canvas.scaleFactor;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - transform.position;
        direction = direction.normalized;

        Vector2 screenPos = Camera.main.WorldToScreenPoint(Vector3.right * transform.localScale.x / 2.0f) - Camera.main.WorldToScreenPoint(Vector2.zero);
        direction *= ((screenPos.x / Camera.main.orthographicSize / 5) + (500 / Camera.main.orthographicSize) / 5) * canvasScale ;
        Vector2 cellScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Arrow.GetComponent<RectTransform>().localPosition = ((cellScreenPosition + direction) / canvasScale);

    }
    // ---------------------------------------------------------------- NOT GLACE
    #region notGlace
    IEnumerator Dash()
    {
        canDash = false;
        Speed.Value *= 3;
        yield return new WaitForSeconds(0.3f);
        Speed.Value /= 3;
        //yield return new
        
    }

    IEnumerator Fire()
    {
        if (weapon == Weapon.DEFAULT)
        {
            canFire = false;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);

            Vector2 cellScreenPosition = transform.position;

            Vector2 bulletPos = cellScreenPosition + direction1;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            yield return new WaitForSeconds(cadenceDEFAULT.Value / cadenceGenerale.Value);
            canFire = true;
        }

        if (weapon == Weapon.MITRAILLETTE)
        {
            canFire = false;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);

            Vector2 cellScreenPosition = transform.position;

            Vector2 bulletPos = cellScreenPosition + direction1;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

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

            GameObject newBullet = Instantiate(bulletInUse, bulletPos1, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            GameObject newBullet1 = Instantiate(bulletInUse, bulletPos2, transform.rotation);
            newBullet1.GetComponent<Rigidbody2D>().AddForce(direction2 * speedBullets.Value, ForceMode2D.Impulse);

            GameObject newBullet2 = Instantiate(bulletInUse, bulletPos3, transform.rotation);
            newBullet2.GetComponent<Rigidbody2D>().AddForce(direction3 * speedBullets.Value, ForceMode2D.Impulse);

            yield return new WaitForSeconds( cadenceSHOTGUN.Value / cadenceGenerale.Value);
            canFire = true;
        }
    }


    void Recul (float puissance, Vector3 actualPos, Vector3 targetPos, float reculDistance)
    {
        if(timeRecul < 1)
        {
            transform.position = Vector3.Lerp(actualPos, targetPos, timeRecul);
            timeRecul += (reculDistance * Time.deltaTime);
        }
        else
        {
            recul = false;
        }
        
    }
    #endregion  // NOT GLACE --------------------------
    // ----------------------------------------------------------------
    IEnumerator FireGlace()
    {
        if (weapon == Weapon.DEFAULT)
        {
            canFire = false;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);

            Vector2 cellScreenPosition = transform.position;

            Vector2 bulletPos = cellScreenPosition + direction1;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            rb.AddForce(-direction * reculGlaceDEFAULT.Value, ForceMode2D.Force);

            yield return new WaitForSeconds(cadenceDEFAULT.Value / cadenceGenerale.Value);
            canFire = true;
        }
        if (weapon == Weapon.MITRAILLETTE)
        {
            canFire = false;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);

            Vector2 cellScreenPosition = transform.position;

            Vector2 bulletPos = cellScreenPosition + direction1;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            rb.AddForce(-direction * reculGlaceMITRAILLETTE.Value, ForceMode2D.Force);

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

            GameObject newBullet = Instantiate(bulletInUse, bulletPos1, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            GameObject newBullet1 = Instantiate(bulletInUse, bulletPos2, transform.rotation);
            newBullet1.GetComponent<Rigidbody2D>().AddForce(direction2 * speedBullets.Value, ForceMode2D.Impulse);

            GameObject newBullet2 = Instantiate(bulletInUse, bulletPos3, transform.rotation);
            newBullet2.GetComponent<Rigidbody2D>().AddForce(direction3 * speedBullets.Value, ForceMode2D.Impulse);

            rb.AddForce(-direction * reculGlaceSHOTGUN.Value, ForceMode2D.Force);

            yield return new WaitForSeconds(cadenceSHOTGUN.Value / cadenceGenerale.Value);
            canFire = true;
        }
    }

    private void WhichSmiley(SMILEY smiley)
    {

        if(smiley == SMILEY.SLIGHTSMILE)
        {

            weapon = Weapon.DEFAULT;
            bullets = BULLETS.DEFAULT;


        } else if(smiley == SMILEY.JOY)
        {

            weapon = Weapon.MITRAILLETTE;
            bullets = BULLETS.DEFAULT;

        } else if(smiley == SMILEY.RAGE)
        {

            weapon = Weapon.SHOTGUN;
            bullets = BULLETS.DEFAULT;

        }
        else if(smiley == SMILEY.COLDFACE)
        {

            weapon = Weapon.DEFAULT;
            bullets = BULLETS.COLD;

        }
        //else if(smiley == SMILEY.HEARTEYES)
        //{



        //}
        else if (smiley == SMILEY.SCREAM)
        {
            weapon = Weapon.DEFAULT;
            Speed.Value *= SpeedIncrease.Value;
            bullets = BULLETS.DEFAULT;

        }
        else if (smiley == SMILEY.SMILINGIMP)
        {
            weapon = Weapon.DEFAULT;
            bullets = BULLETS.DEFAULT;

        }

    }

}