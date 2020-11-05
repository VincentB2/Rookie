using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class T10_PlayerFight : MonoBehaviour
{
    public int playerHPValue = 3;
    public float playerHP = 0;
    public float timerGlobalValue = 60.0f;
    public float timerGlobal;
    float timerScore;
    public GameObject textTimer;
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
        timerGlobal -= Time.deltaTime;
        timerScore = (timerGlobalValue - timerGlobal) * 1000;
        textTimer.GetComponent<TextMeshProUGUI>().text = "Time : " + timerGlobal;
        // Julien
        livesText.GetComponent<TextMeshProUGUI>().text = "Lives : " + playerHP;
        PlayerPrefs.SetFloat("ScoreTeam10", timerScore);
        if (playerHP <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void TakeDamage(float damage)
    {
        playerHP -= damage;
    }
}