using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class T10_MovementPlayer : MonoBehaviour
{
    //C-------------------------CAMERA SHAKE
    T10_CameraController camControl;
    [Header("CAMERA SHAKE")]
    public float shakeDurShotGun = 0.1f;
    public float shakeAmShotGun = 1f;
    public float shakeDurDefault = 0.1f;
    public float shakeAmDefault = 1f;
    public float shakeDurSMG = 0.1f;
    public float shakeAmSMG = 1f;
    // ------------------------------- FIRE
    private Vector2 direction;
    [Header("FIRE")]
    public GameObject Arrow;
    public enum Weapon { DEFAULT, MITRAILLETTE, SHOTGUN, SNIPER, GRENADE, COLD }
    public Weapon weapon;
    // --------------------GUNS
    private bool canFire = true;
    [Header("GUNS")]
    public FloatVariable cadenceGenerale;
    // --------DEFAULT
    [Header("DEFAULT")]
    public FloatVariable cadenceDEFAULT;
    public FloatVariable reculGlaceDEFAULT;
    public FloatVariable PuissReculTerreDEFAULT;
    public FloatVariable TimeReculTerreDEFAULT;
    // --------MITRAILLETTE
    [Header("RIFLE")]
    public FloatVariable cadenceMITRAILLETTE;
    public FloatVariable reculGlaceMITRAILLETTE;
    public FloatVariable PuissReculTerreMITRAILLETTE;
    public FloatVariable TimeReculTerreMITRAILLETTE;
    // --------SHOTGUN
    [Header("SHOTGUN")]
    public FloatVariable cadenceSHOTGUN;
    public FloatVariable reculGlaceSHOTGUN;
    public FloatVariable TimeReculTerreSHOTGUN;
    public FloatVariable PuissReculTerreSHOTGUN;
    // --------SNIPER
    [Header("SNIPER")]
    public FloatVariable cadenceSNIPER;
    public FloatVariable reculGlaceSNIPER;
    public FloatVariable PuissReculTerreSNIPER;
    public FloatVariable TimeReculTerreSNIPER;
    public LineRenderer lineRenderer;
    private bool isSniper;
    Vector2 lineVector;
    public LayerMask ignoreLayers;
    // --------GRENADE
    [Header("GRENADE")]
    public FloatVariable cadenceGRENADE;
    public FloatVariable reculGlaceGRENADE;
    public FloatVariable PuissReculTerreGRENADE;
    public FloatVariable TimeReculTerreGRENADE;
    // ---------------RECUL
    private Vector3 actualPos;
    private Vector3 targetPos;
    private bool isRecul = false;
    private float reculPower = 1;
    float timeSave;
    // ---------------------BULLETS
    [Header("BULLETS")]
    public FloatVariable speedBullets;
    private enum BULLETS { DEFAULT, GLACE, SNIPER, GRENADE }
    BULLETS bullets;
    public GameObject bulletInUse;
    // ------------------------------- MOVE
    private bool canMove = true;
    [Header("MOVE")]
    public FloatVariable Speed;
    private float speed;
    private Rigidbody2D rb;
    private bool canDash = true;
    public FloatVariable dashDelay;
    public GameObject shield;
    public GameObject shieldConsumedUI;
    public GameObject shieldUI;
    // ------------------------------- SOL
    [HideInInspector]
    public bool isGlace = false;
    // ------------------------------- COLLIDER
    private Collider2D col;
    [Header("RAYCAST IGNORE")]
    public LayerMask player;
    // ------------------------------- SMILEY
    public enum SMILEY { JOY, RAGE, COLDFACE, SLIGHTSMILE, SCREAM, SMILINGIMP, MAD }
    [Header("SMILEY")]
    public SMILEY smiley;
    SMILEY lastSmiley;
    public FloatVariable SpeedIncrease;
    public FloatVariable cooldownSmiley;
    public Sprite[] emojiSprite;
    public SpriteRenderer playerCap;
    // Julien
    T10_UI ui;
    private void Awake()
    {
        lineRenderer.enabled = false;
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetWidth(0.02f, 0.02f);
        lineRenderer.SetColors(Color.red, Color.red);
        playerCap = GameObject.Find("/Player/smileyCap").GetComponent<SpriteRenderer>();
        // Julien
        ui = GameObject.Find("UI").GetComponent<T10_UI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        camControl = GameObject.Find("/Camera").GetComponent<T10_CameraController>();
        smiley = SMILEY.SLIGHTSMILE;
        lastSmiley = smiley;
        WhichSmiley(smiley);
        speed = Speed.Value;
        shieldConsumedUI.SetActive(true);
        shieldUI.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        // Julien
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            gameObject.GetComponent<Animator>().Play("player_walk");
        else
            gameObject.GetComponent<Animator>().Play("player_idle");
        if (smiley != lastSmiley)
        {
            if (lastSmiley == SMILEY.SCREAM)
            {
                speed /= SpeedIncrease.Value;
            }
            if (lastSmiley == SMILEY.SMILINGIMP)
            {
                lineRenderer.enabled = false;
                isSniper = false;
            }
            WhichSmiley(smiley);
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
            if (Input.GetKeyDown("space") && canDash)
            {
                if (rb.velocity.magnitude != 0)
                {
                    Debug.Log("dash");
                    StartCoroutine("Dash");
                }
            }
            // Curseur();
            FaceMouse();
            if (isSniper)
            {
                SetLineRenderer();
            }
            if (!ui.isGameMenued && !ui.isGamePaused && Input.GetMouseButton(0) && canFire)
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
                    targetPos = transform.position - ((new Vector3(direction.x, direction.y) * reculPower) + (dir - vit));
                    camControl.ShakeCamera(shakeDurShotGun, shakeAmShotGun);
                    StartCoroutine(Recul(targetPos, TimeReculTerreSHOTGUN.Value, PuissReculTerreSHOTGUN.Value));
                }
                else if (weapon == Weapon.SNIPER)
                {
                    targetPos = transform.position - ((new Vector3(direction.x, direction.y) * reculPower) + (dir - vit));
                    //camControl.ShakeCamera(shakeDurShotGun, shakeAmShotGun);
                    StartCoroutine(Recul(targetPos, TimeReculTerreSNIPER.Value, PuissReculTerreSNIPER.Value));
                }
                else if (weapon == Weapon.GRENADE)
                {
                    targetPos = transform.position - ((new Vector3(direction.x, direction.y) * reculPower) + (dir - vit));
                    //camControl.ShakeCamera(shakeDurShotGun, shakeAmShotGun);
                    StartCoroutine(Recul(targetPos, TimeReculTerreGRENADE.Value, PuissReculTerreGRENADE.Value));
                }
            }
        }
        if (isGlace)
        {
            //Curseur();
            FaceMouse();
            if (isSniper)
            {
                SetLineRenderer();
            }
            if (!ui.isGameMenued && !ui.isGamePaused && Input.GetMouseButton(0) && canFire)
            {
                canFire = false;
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
                rb.velocity = new Vector2(x, y) * speed;
            }
            if (isGlace)
            {
                float x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
                float y = Input.GetAxis("Vertical") * Time.fixedDeltaTime;
                rb.AddForce(new Vector2(x, y) * speed);
                rb.AddForce(-rb.velocity * 2);
            }
        }
    }
    //void Curseur()
    //{
    //    float canvasScale = canvas.scaleFactor;
    //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    direction = mousePos - transform.position;
    //    direction = direction.normalized;
    //    Vector2 screenPos = Camera.main.WorldToScreenPoint(Vector3.right * transform.localScale.x / 2.0f) - Camera.main.WorldToScreenPoint(Vector2.zero);
    //    direction *= ((screenPos.x / Camera.main.orthographicSize / 5) + (500 / Camera.main.orthographicSize) / 5) * canvasScale ;
    //    Vector2 cellScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
    //    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    Arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    Arrow.GetComponent<RectTransform>().localPosition = ((cellScreenPosition + direction) / canvasScale);
    //}
    // ---------------------------------------------------------------- NOT GLACE
    #region notGlace
    IEnumerator Dash()
    {
        canDash = false;
        shield.SetActive(true);
        shieldUI.SetActive(false);
        Physics2D.IgnoreLayerCollision(9, 10, true);
        FindObjectOfType<T10_AudioManager>().Play("shield");
        speed *= 3;
        yield return new WaitForSeconds(0.3f);
        speed /= 3;
        shield.GetComponent<Animator>().SetTrigger("end");
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(9, 10, false);
        shield.SetActive(false);

        yield return new WaitForSeconds(dashDelay.Value);
        shieldUI.SetActive(true);

        canDash = true;
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
        if (weapon == Weapon.COLD)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);
            Vector2 cellScreenPosition = transform.position;
            Vector2 bulletPos = cellScreenPosition + direction1;
            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);
            FindObjectOfType<T10_AudioManager>().Play("coldgun");
            yield return new WaitForSeconds(cadenceDEFAULT.Value * 0.7f / cadenceGenerale.Value);
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
            yield return new WaitForSeconds(cadenceSHOTGUN.Value / cadenceGenerale.Value);
        }
        else if (weapon == Weapon.SNIPER)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);
            Vector2 cellScreenPosition = transform.position;
            Vector2 bulletPos = cellScreenPosition + direction1;
            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value * 1.5f, ForceMode2D.Impulse);
            FindObjectOfType<T10_AudioManager>().Play("sniper");
            yield return new WaitForSeconds(cadenceSNIPER.Value / cadenceGenerale.Value);
        }
        else if (weapon == Weapon.GRENADE)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);
            Vector2 cellScreenPosition = transform.position;
            Vector2 bulletPos = cellScreenPosition + direction1;
            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value * 0.4f, ForceMode2D.Impulse);
            FindObjectOfType<T10_AudioManager>().Play("GrenadeLauncher");
            yield return new WaitForSeconds(cadenceGRENADE.Value / cadenceGenerale.Value);
        }
        canFire = true;
    }
    public IEnumerator Recul(Vector2 targetPos, float timeRecul, float puissanceRecul)
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
        else if (weapon == Weapon.COLD)
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
            FindObjectOfType<T10_AudioManager>().Play("coldgun");
            yield return new WaitForSeconds(cadenceDEFAULT.Value *0.7f/ cadenceGenerale.Value);
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
        else if (weapon == Weapon.SNIPER)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);
            Vector2 cellScreenPosition = transform.position;
            Vector2 bulletPos = cellScreenPosition + direction1;
            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value, ForceMode2D.Impulse);
            rb.AddForce(-direction * reculGlaceSNIPER.Value, ForceMode2D.Impulse);
            FindObjectOfType<T10_AudioManager>().Play("sniper");
            yield return new WaitForSeconds(cadenceSNIPER.Value / cadenceGenerale.Value);
        }
        else if (weapon == Weapon.GRENADE)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction1 = mousePos - transform.position;
            direction1 = direction.normalized;
            direction1 *= transform.localScale.x / (2 * transform.localScale.x);
            Vector2 cellScreenPosition = transform.position;
            Vector2 bulletPos = cellScreenPosition + direction1;
            GameObject newBullet = Instantiate(bulletInUse, bulletPos, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(direction1 * speedBullets.Value * 0.4f, ForceMode2D.Impulse);
            rb.AddForce(-direction * reculGlaceGRENADE.Value, ForceMode2D.Impulse);
            FindObjectOfType<T10_AudioManager>().Play("GrenadeLauncher");
            yield return new WaitForSeconds(cadenceGRENADE.Value / cadenceGenerale.Value);
        }
        canFire = true;
    }
    private void WhichSmiley(SMILEY smiley)
    {
        StopCoroutine("Fire");
        StopCoroutine("FireGlace");
        canFire = false;
        StopCoroutine("CooldownSmiley");
        if (smiley == SMILEY.SLIGHTSMILE)
        {
            weapon = Weapon.DEFAULT;
            bullets = BULLETS.DEFAULT;
            bulletInUse.GetComponent<T10_Bullet>().bulletType = T10_Bullet.BULLETS.DEFAULT;
            playerCap.sprite = emojiSprite[0];
        }
        else if (smiley == SMILEY.JOY)
        {
            weapon = Weapon.MITRAILLETTE;
            bullets = BULLETS.DEFAULT;
            bulletInUse.GetComponent<T10_Bullet>().bulletType = T10_Bullet.BULLETS.DEFAULT;
            playerCap.sprite = emojiSprite[1];
        }
        else if (smiley == SMILEY.RAGE)
        {
            weapon = Weapon.SHOTGUN;
            bullets = BULLETS.DEFAULT;
            bulletInUse.GetComponent<T10_Bullet>().bulletType = T10_Bullet.BULLETS.SHOTGUN;
            playerCap.sprite = emojiSprite[2];
        }
        else if (smiley == SMILEY.COLDFACE)
        {
            weapon = Weapon.COLD;
            bullets = BULLETS.GLACE;
            bulletInUse.GetComponent<T10_Bullet>().bulletType = T10_Bullet.BULLETS.GLACE;
            playerCap.sprite = emojiSprite[3];
        }
        else if (smiley == SMILEY.SCREAM)
        {
            weapon = Weapon.DEFAULT;
            speed *= SpeedIncrease.Value;
            bullets = BULLETS.DEFAULT;
            bulletInUse.GetComponent<T10_Bullet>().bulletType = T10_Bullet.BULLETS.DEFAULT;
            playerCap.sprite = emojiSprite[4];
        }
        else if (smiley == SMILEY.SMILINGIMP)
        {
            weapon = Weapon.SNIPER;
            bullets = BULLETS.SNIPER;
            bulletInUse.GetComponent<T10_Bullet>().bulletType = T10_Bullet.BULLETS.SNIPER;
            lineRenderer.enabled = true;
            isSniper = true;
            playerCap.sprite = emojiSprite[5];
        }
        else if (smiley == SMILEY.MAD)
        {
            weapon = Weapon.GRENADE;
            bullets = BULLETS.GRENADE;
            bulletInUse.GetComponent<T10_Bullet>().bulletType = T10_Bullet.BULLETS.GRENADE;
            playerCap.sprite = emojiSprite[6];
        }
        StartCoroutine("CooldownSmiley");
        canFire = true;
    }
    private void FaceMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - transform.position;
        direction = direction.normalized;
        transform.up = direction;
    }
    private void SetLineRenderer()
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineVector = MousePos - new Vector2(Arrow.transform.position.x, Arrow.transform.position.y);
        lineVector = lineVector.normalized;
        Ray ray = new Ray(Arrow.transform.position, lineVector * 30);
        RaycastHit2D hit = Physics2D.Raycast(Arrow.transform.position, lineVector, Mathf.Infinity, ~ignoreLayers);
        // Debug.DrawRay(Arrow.transform.position, lineVector * 30, Color.black);
        Vector2 pos2;
        if (hit)
        {
            pos2 = hit.point;
        }
        else
        {
            pos2 = ray.GetPoint(25);;
        }
        lineRenderer.SetPosition(0, Arrow.transform.position);
        lineRenderer.SetPosition(1, pos2);
    }
    IEnumerator CooldownSmiley()
    {
        yield return new WaitForSeconds(cooldownSmiley.Value);
        smiley = SMILEY.SLIGHTSMILE;
    }
}