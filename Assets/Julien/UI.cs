using System.Collections;
using UnityEngine;
using UnityEngine.UI;
class UI : MonoBehaviour
{
    public bool isGamePaused;
    GameObject MenuLayer, InGameLayer, PauseLayer;
    Button PauseButton, ResumeButton;
    void Awake()
    {
        MenuLayer = GameObject.Find("MenuLayer");
        InGameLayer = GameObject.Find("InGameLayer");
        PauseLayer = GameObject.Find("PauseLayer");
        PauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
        ResumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        PauseButton.onClick.AddListener(PauseGame);
        ResumeButton.onClick.AddListener(PauseGame);
    }
    void Update()
    {
        Time.timeScale = isGamePaused ? 0 : 1;
        InGameLayer.SetActive(!isGamePaused);
        PauseLayer.SetActive(isGamePaused);
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }
    void PauseGame()
    {
        isGamePaused ^= true;
    }
}