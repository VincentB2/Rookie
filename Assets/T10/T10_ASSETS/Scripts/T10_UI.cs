using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
class T10_UI : MonoBehaviour
{
    public bool isGameMenued, isGamePaused;
    public float timeScale;
    GameObject MenuLayer, InGameLayer, PauseLayer;
    Button PlayButton, PauseButton, ResumeButton, MenuButton, MainMenuButton, QuitButton;
    Button RetryButtonEnd, MainMenuButtonEnd, QuitButtonEnd;
    GameObject ScoreText, MenuLayerEnd;
    T10_PlayerFight Player;
    public GameObject playerGO;
    void Awake()
    {
        // Layers
        MenuLayer = GameObject.Find("MenuLayer");
        InGameLayer = GameObject.Find("InGameLayer");
        PauseLayer = GameObject.Find("PauseLayer");
        // Buttons
        PlayButton = GameObject.Find("PlayButton").GetComponent<Button>();
        PauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
        ResumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        MenuButton = GameObject.Find("MenuButton").GetComponent<Button>();
        MainMenuButton = GameObject.Find("MainMenuButton").GetComponent<Button>();
        QuitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        // EndMenu
        MenuLayerEnd = GameObject.Find("MenuEnd");
        ScoreText = GameObject.Find("Score");
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<T10_PlayerFight>();
        RetryButtonEnd = GameObject.Find("RetryButton").GetComponent<Button>();
        MainMenuButtonEnd = GameObject.Find("MainMenuButtonEnd").GetComponent<Button>();
        QuitButtonEnd = GameObject.Find("QuitButtonEnd").GetComponent<Button>();
        // EndListeners
        RetryButtonEnd.onClick.AddListener(MenuGame);
        MainMenuButtonEnd.onClick.AddListener(MainMenuGame);
        QuitButtonEnd.onClick.AddListener(QuitGame);
        // Listeners
        PlayButton.onClick.AddListener(PlayGame);
        PauseButton.onClick.AddListener(PauseResumeGame);
        ResumeButton.onClick.AddListener(PauseResumeGame);
        MenuButton.onClick.AddListener(MenuGame);
        MainMenuButton.onClick.AddListener(MainMenuGame);
        QuitButton.onClick.AddListener(QuitGame);
    }
    void Update()
    {
        timeScale = Time.timeScale;
        Time.timeScale = isGameMenued ? 0 : isGamePaused ? 0 : Player.isEndMenued ? 0 : 1;
        MenuLayer.SetActive(isGameMenued);
        InGameLayer.SetActive(!isGameMenued && !isGamePaused && !Player.isEndMenued);
        PauseLayer.SetActive(!isGameMenued && isGamePaused && !Player.isEndMenued);
        MenuLayerEnd.SetActive(!isGameMenued && !isGamePaused && Player.isEndMenued);
        ScoreText.GetComponent<TextMeshProUGUI>().text = "Your score : " + Player.timerScore;

        if (Input.GetKeyDown(KeyCode.Escape)) PauseResumeGame();
    }
    void PauseResumeGame() { isGamePaused ^= true; }
    void PlayGame() { isGameMenued ^= true; isGamePaused = false; playerGO.GetComponent<T10_MovementPlayer>().enabled = true; }
    void MenuGame() { SceneManager.LoadScene("T10_SCENE"); PlayerPrefs.SetInt("ScoreTeam10", 0); }
    void MainMenuGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SalleArcade");
    }
    void QuitGame() { Application.Quit(); }
}