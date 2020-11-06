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
        SmilingImp,
        Mad
    }
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle + 90)), Time.deltaTime * 4);
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
            }
            else if (emojiType == Type.SlightSmile)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.SLIGHTSMILE;
            }
            else if (emojiType == Type.Rage)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.RAGE;
            }
            else if (emojiType == Type.Scream)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.SCREAM;
            }
            else if (emojiType == Type.ColdFace)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.COLDFACE;
            }
            else if (emojiType == Type.HeartEyes)
            {
                if (player.GetComponent<T10_PlayerFight>().playerHP < player.GetComponent<T10_PlayerFight>().playerHPValue && player.GetComponent<T10_PlayerFight>().playerHP != 0)
                {
                    player.GetComponent<T10_PlayerFight>().playerHP = 10;
                }
            }
            else if (emojiType == Type.SmilingImp)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.SMILINGIMP;
            }
            else if (emojiType == Type.Mad)
            {
                player.GetComponent<T10_MovementPlayer>().smiley = T10_MovementPlayer.SMILEY.MAD;
            }
            Destroy(gameObject);
        }
    }
}