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
    Vector3 thisPos;
    Vector3 targetPos;
    float angle;
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
            thisPos = transform.position;
            targetPos = target.position;
            targetPos.x = targetPos.x - thisPos.x;
            targetPos.y = targetPos.y - thisPos.y;
            angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * 4);
        }
    }
}