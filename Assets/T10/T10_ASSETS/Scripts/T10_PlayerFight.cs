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
    float timerScore;
    public GameObject textTimer;
    // MENU
    public bool isEndMenued;
    public GameObject flagEnd;
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
        flagEnd = GameObject.Find("End");
    }
    // Update is called once per frame
    void Update()
    {
        timerGlobal += Time.deltaTime;
        timerScore = (timerGlobalValue - timerGlobal) * 1000;
        textTimer.GetComponent<TextMeshProUGUI>().text = "Time : " + string.Format("{0:0.00}", timerGlobal);
        // Julien
        livesText.GetComponent<TextMeshProUGUI>().text = "Lives : " + playerHP;
        PlayerPrefs.SetFloat("ScoreTeam10", timerScore);
        if (playerHP <= 0)
        {
            playerHP = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void TakeDamage(float damage)
    {
        playerHP -= damage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("End")) 
        {
            isEndMenued = true;
        }
    }
}