using System.Collections;
using UnityEngine;
class Emoji : MonoBehaviour
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
    public Type emojiType;
    public Sprite emojiSprite;
    // Look At
    GameObject player;
    Transform target;
    Vector3 targetPos;
    Vector3 thisPos;
    float angle;
    // Effect Bools
    bool bJoy, bRage, bColdFace, bHeartEyes, bSlightSmile;
    void Awake()
    {
        if (emojiSprite)
            GetComponent<SpriteRenderer>().sprite = emojiSprite;
        player = GameObject.FindWithTag("Player");
        if (player)
            target = player.GetComponent<Transform>();
    }
    void Update()
    {
        if (target)
        {
            targetPos = target.position;
            thisPos = transform.position;
            targetPos.x = targetPos.x - thisPos.x;
            targetPos.y = targetPos.y - thisPos.y;
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        switch (emojiType)
        {
            case Type.Joy:
                bJoy = true;
                break;
            case Type.Rage:
                bRage = true;
                break;
            case Type.ColdFace:
                bColdFace = true;
                break;
            case Type.HeartEyes:
                bHeartEyes = true;
                break;
            case Type.SlightSmile:
                bSlightSmile = true;
                break;
        }
    }
}