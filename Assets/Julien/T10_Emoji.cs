using UnityEngine;
class T10_Emoji : MonoBehaviour
{
    public enum Type
    {
        Joy,
        Rage,
        ColdFace,
        HeartEyes,
        SlightSmile,
        Scream,
        SmilingImp
    }
    public SpriteRenderer playerCap;
    public Type emojiType;
    public Sprite[] emojiSprite;
    // Look At
    GameObject player;
    Transform target;
    Vector3 thisPos;
    Vector3 targetPos;
    float angle;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (player)
            target = player.GetComponent<Transform>();
        playerCap = GameObject.Find("/Player/smileyCap").GetComponent<SpriteRenderer>();
    }
    void Update()
    {

        if (target)
        {
            thisPos = transform.position;
            targetPos = target.position;
            targetPos.x = targetPos.x - thisPos.x;
            targetPos.y = targetPos.y - thisPos.y;
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * 4);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")) 
        {
            FindObjectOfType<T10_AudioManager>().Play("smiley");
            if (emojiType == Type.Joy)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.JOY;
                playerCap.sprite = emojiSprite[3];
            }
            else if (emojiType == Type.SlightSmile)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.SLIGHTSMILE;
                playerCap.sprite = emojiSprite[5];
            }
            else if (emojiType == Type.Rage)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.RAGE;
                playerCap.sprite = emojiSprite[4];
            }
            else if (emojiType == Type.Scream)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.SCREAM;
                playerCap.sprite = emojiSprite[1];
            }
            else if (emojiType == Type.ColdFace)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.COLDFACE;
                playerCap.sprite = emojiSprite[0];
            }
            else if (emojiType == Type.HeartEyes)
            {
                if (player.GetComponent<T10_PlayerFight>().playerHP < player.GetComponent<T10_PlayerFight>().playerHPValue && player.GetComponent<T10_PlayerFight>().playerHP != 0)
                {
                    player.GetComponent<T10_PlayerFight>().playerHP += 1;
                    Debug.Log(player.GetComponent<T10_PlayerFight>().playerHP);
                }
                playerCap.sprite = emojiSprite[2];
            }

            Destroy(gameObject);
        }

        

    }
}