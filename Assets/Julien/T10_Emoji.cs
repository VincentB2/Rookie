﻿using UnityEngine;
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
        Debug.Log(emojiType);
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
            if (emojiType == Type.Joy)
            {
                player.GetComponent<MovementPlayer>().weapon = MovementPlayer.Weapon.MITRAILLETTE;
            }
            else if (emojiType == Type.SlightSmile)
            {

            }
            else if (emojiType == Type.Rage)
            {
                player.GetComponent<MovementPlayer>().weapon = MovementPlayer.Weapon.SHOTGUN;
            }
            else if (emojiType == Type.SmilingImp)
            {

            }
            else if (emojiType == Type.Scream)
            {

            }
            else if (emojiType == Type.ColdFace)
            {

            }else if (emojiType == Type.HeartEyes)
            {
                if (player.GetComponent<T10_PlayerFight>().playerHP < player.GetComponent<T10_PlayerFight>().playerHPValue && player.GetComponent<T10_PlayerFight>().playerHP != 0)
                {
                    player.GetComponent<T10_PlayerFight>().playerHP += 1;
                    Debug.Log(player.GetComponent<T10_PlayerFight>().playerHP);
                }
            }

            Destroy(gameObject);
        }

        

    }
}