using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class T10_PlayerFight : MonoBehaviour
{
    public int playerHPValue = 3;
    public float playerHP = 0;
    float timerGlobalValue = 0;
    public float timerGlobal;
    public float timerScore;
    public GameObject textTimer;
    // MENU
    public bool isEndMenued;
    public Animator camera;
    // Julien
    GameObject livesText;
    // Start is called before the first frame update
    void Awake()
    {
        playerHP = playerHPValue;
        timerGlobal = timerGlobalValue;
        textTimer = GameObject.Find("/UI/InGameLayer/timer");
        // Julien
        livesText = GameObject.Find("lives");
    }
    // Update is called once per frame
    void Update()
    {
        timerGlobal += Time.deltaTime;
        textTimer.GetComponent<TextMeshProUGUI>().text = "Time : " + string.Format("{0:0.00}", timerGlobal);
        // Julien
        livesText.GetComponent<TextMeshProUGUI>().text = "Lives : " + playerHP;
        if (playerHP <= 0)
        {
            playerHP = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void TakeDamage(float damage)
    {
        playerHP -= damage;
        camera.SetTrigger("playerHit");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("End")) 
        {

            timerScore = 5000 - timerGlobal * 20;
            Debug.Log(timerScore);
            PlayerPrefs.SetFloat("ScoreTeam10", timerScore);
            isEndMenued = true;
        }
    }
}