using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class T10_MovementPlayer : MonoBehaviour
{
    //C-------------------------CAMERA SHAKE
    T10_CameraController camControl;
    public float shakeDurShotGun = 0.1f;
    public float shakeAmShotGun = 1f;
    public float shakeDurDefault = 0.1f;
    public float shakeAmDefault = 1f;
    public float shakeDurSMG = 0.1f;
    public float shakeAmSMG = 1f;
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
    public FloatVariable TimeReculTerreDEFAULT;
    // --------MITRAILLETTE
    public FloatVariable cadenceMITRAILLETTE;
    public FloatVariable reculGlaceMITRAILLETTE;
    public FloatVariable PuissReculTerreMITRAILLETTE;
    public FloatVariable TimeReculTerreMITRAILLETTE;
    // --------SHOTGUN
    public FloatVariable cadenceSHOTGUN;
    public FloatVariable reculGlaceSHOTGUN;
    public FloatVariable TimeReculTerreSHOTGUN;
    public FloatVariable PuissReculTerreSHOTGUN;

    // ---------------RECUL
    private Vector3 actualPos;
    private Vector3 targetPos;
    private bool isRecul = false;
    private float reculPower;
    float timeSave;

    // ---------------------BULLETS
    public FloatVariable speedBullets;
    private enum BULLETS { DEFAULT, COLD};
    BULLETS bullets;
    public GameObject bulletInUse;

    // ------------------------------- MOVE
    private bool canMove = true;
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

        camControl = GameObject.Find("/Camera").GetComponent<T10_CameraController>();
        smiley = SMILEY.SLIGHTSMILE;
        lastSmiley = smiley;
        WhichSmiley(smiley);
    }

    // Update is called once per frame
    void Update()
    {
        if(smiley != lastSmiley)
        {
            if(lastSmiley == SMILEY.SCREAM)
            {
                Speed.Value /= SpeedIncrease.Value;
            } else if(lastSmiley == SMILEY.COLDFACE)
            {
                bulletInUse.GetComponent<T10_Bullet>().isGlace = false;
            }
            WhichSmiley(smiley);
            StartCoroutine("ResetFireAfterNewSmiley");
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, ~player);;
        if (hit)
        {
            if (hit.transform.gameObject.layer == 8)
            {
                //timeRecul = 1;
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

            // Curseur();
            FaceMouse();

            if (Input.GetMouseButton(0) && canFire)
            {
                canFire = false;
                StartCoroutine("Fire");

                actualPos = transform.position;

                Vector3 dir = direction.normalized;
                Vector3 vit = rb.velocity.normalized;
                if (weapon == Weapon.DEFAULT)
                {
                    targetPos = transform.position - ((new Vector3(direction.x, direction.y) * reculPower) + (dir - vit));
                    StartCoroutine(Recul(targetPos, TimeReculTerreDEFAULT.Value, PuissReculTerreDEFAULT.Value));
                    camControl.ShakeCamera(shakeDurDefault, shakeAmDefault);
                }
                else if (weapon == Weapon.MITRAILLETTE)
                {                   
                    targetPos = transform.position - ((new Vector3(direction.x, direction.y) * reculPower) + (dir - vit));
                    StartCoroutine(Recul(targetPos, TimeReculTerreMITRAILLETTE.Value, PuissReculTerreMITRAILLETTE.Value));
                    camControl.ShakeCamera(shakeDurSMG, shakeAmSMG);
                }
                else if (weapon == Weapon.SHOTGUN)
                {
                    
                    targetPos = transform.position - ((new Vector3(direction.x, direction.y) * reculPower) + (dir-vit)) ;

                    camControl.ShakeCamera(shakeDurShotGun, shakeAmShotGun);
                    StartCoroutine(Recul(targetPos, TimeReculTerreSHOTGUN.Value, PuissReculTerreSHOTGUN.Value));
                }
            }

        }

        if (isGlace)
        {
            //Curseur();
            FaceMouse();

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
        if (canMove)
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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);

            Vector2 cellScreenPosition = transform.position;

            Vector2 bulletPos = cellScreenPosition + direction1;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            FindObjectOfType<T10_AudioManager>().Play("SMGShot");

            yield return new WaitForSeconds(cadenceDEFAULT.Value / cadenceGenerale.Value);
        }

        else if (weapon == Weapon.MITRAILLETTE)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);

            Vector2 cellScreenPosition = transform.position;

            Vector2 bulletPos = cellScreenPosition + direction1;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            FindObjectOfType<T10_AudioManager>().Play("rifleShot");

            yield return new WaitForSeconds(cadenceMITRAILLETTE.Value / cadenceGenerale.Value);
        }

         else if (weapon == Weapon.SHOTGUN)
        {
            Vector2 cellScreenPosition = transform.position;
            Vector2 direction1 = Arrow.transform.position - transform.position;
            direction1 = direction1.normalized;
            direction1 *= 0.5f;
            
            Vector2 bulletPos1 = cellScreenPosition + direction1;

            Vector2 direction2 = Arrow.transform.GetChild(0).transform.position - transform.position;
            direction2 = direction2.normalized;
            direction2 *= 0.5f;
            Vector2 bulletPos2 = cellScreenPosition + direction2;

            Vector2 direction3 = Arrow.transform.GetChild(1).transform.position - transform.position;
            direction3 = direction3.normalized;
            direction3 *= 0.5f;

            Vector2 bulletPos3 = cellScreenPosition + direction3;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos1, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            GameObject newBullet1 = Instantiate(bulletInUse, bulletPos2, transform.rotation);
            newBullet1.GetComponent<Rigidbody2D>().AddForce(direction2 * speedBullets.Value, ForceMode2D.Impulse);

            GameObject newBullet2 = Instantiate(bulletInUse, bulletPos3, transform.rotation);
            newBullet2.GetComponent<Rigidbody2D>().AddForce(direction3 * speedBullets.Value, ForceMode2D.Impulse);

            FindObjectOfType<T10_AudioManager>().Play("shotgun");

            yield return new WaitForSeconds( cadenceSHOTGUN.Value / cadenceGenerale.Value);
        }
        canFire = true;
    }

    IEnumerator Recul(Vector2 targetPos, float timeRecul, float puissanceRecul)
    {
        canMove = false;
        rb.velocity = (new Vector3(targetPos.x, targetPos.y) - transform.position) * puissanceRecul;
        yield return new WaitForSeconds(timeRecul);
        canMove = true;
    }

    #endregion  // NOT GLACE --------------------------
    // ----------------------------------------------------------------
    IEnumerator FireGlace()
    {
        canFire = false;
        if (weapon == Weapon.DEFAULT)
        {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);

            Vector2 cellScreenPosition = transform.position;

            Vector2 bulletPos = cellScreenPosition + direction1;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            rb.AddForce(-direction * reculGlaceDEFAULT.Value, ForceMode2D.Impulse);

            FindObjectOfType<T10_AudioManager>().Play("SMGShot");

            yield return new WaitForSeconds(cadenceDEFAULT.Value / cadenceGenerale.Value);
            
        }
        else if (weapon == Weapon.MITRAILLETTE)
        {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);

            Vector2 cellScreenPosition = transform.position;

            Vector2 bulletPos = cellScreenPosition + direction1;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            rb.AddForce(-direction * reculGlaceMITRAILLETTE.Value, ForceMode2D.Impulse);

            FindObjectOfType<T10_AudioManager>().Play("rifleShot");

            yield return new WaitForSeconds(cadenceMITRAILLETTE.Value / cadenceGenerale.Value);
            
        }

        if (weapon == Weapon.SHOTGUN)
        {
            Vector2 cellScreenPosition = transform.position;
            Vector2 direction1 = Arrow.transform.position - transform.position;
            direction1 = direction1.normalized;
            direction1 *= 0.5f;

            Vector2 bulletPos1 = cellScreenPosition + direction1;

            Vector2 direction2 = Arrow.transform.GetChild(0).transform.position - transform.position;
            direction2 = direction2.normalized;
            direction2 *= 0.5f;
            Vector2 bulletPos2 = cellScreenPosition + direction2;

            Vector2 direction3 = Arrow.transform.GetChild(1).transform.position - transform.position;
            direction3 = direction3.normalized;
            direction3 *= 0.5f;

            Vector2 bulletPos3 = cellScreenPosition + direction3;

            GameObject newBullet = Instantiate(bulletInUse, bulletPos1, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);

            GameObject newBullet1 = Instantiate(bulletInUse, bulletPos2, transform.rotation);
            newBullet1.GetComponent<Rigidbody2D>().AddForce(direction2 * speedBullets.Value, ForceMode2D.Impulse);

            GameObject newBullet2 = Instantiate(bulletInUse, bulletPos3, transform.rotation);
            newBullet2.GetComponent<Rigidbody2D>().AddForce(direction3 * speedBullets.Value, ForceMode2D.Impulse);

            rb.AddForce(-direction * reculGlaceSHOTGUN.Value, ForceMode2D.Impulse);

            FindObjectOfType<T10_AudioManager>().Play("shotgun");
            yield return new WaitForSeconds(cadenceSHOTGUN.Value / cadenceGenerale.Value);
            
        }
        canFire = true;
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
            bulletInUse.GetComponent<T10_Bullet>().isGlace = true;

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

    IEnumerator ResetFireAfterNewSmiley()
    {
        StopCoroutine("Fire");
        yield return new WaitForSeconds(0.1f);
        canFire = true;
    }


    private void FaceMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - transform.position;
        direction = direction.normalized;
        transform.up = direction;
    }

}