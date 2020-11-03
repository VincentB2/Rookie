using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
class T10_UI : MonoBehaviour
{
    public bool isGameMenued, isGamePaused;
    public float timeScale;
    GameObject MenuLayer, InGameLayer, PauseLayer;
    Button PlayButton, PauseButton, ResumeButton, MenuButton, MainMenuButton, QuitButton;
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
        // Listeners
        PlayButton.onClick.AddListener(MenuGame);
        PauseButton.onClick.AddListener(PauseResumeGame);
        ResumeButton.onClick.AddListener(PauseResumeGame);
        MenuButton.onClick.AddListener(MenuGame);
        MainMenuButton.onClick.AddListener(MainMenuGame);
        QuitButton.onClick.AddListener(QuitGame);
    }
    void Update()
    {
        timeScale = Time.timeScale;
        Time.timeScale = isGameMenued ? 0 : isGamePaused ? 0 : 1;
        MenuLayer.SetActive(isGameMenued);
        InGameLayer.SetActive(!isGameMenued && !isGamePaused);
        PauseLayer.SetActive(!isGameMenued && isGamePaused);
        if (Input.GetKeyDown(KeyCode.Escape)) PauseResumeGame();
    }
    void PauseResumeGame() { isGamePaused ^= true; }
    void MenuGame() { isGameMenued ^= true; PauseResumeGame(); }
    void MainMenuGame()
    {
        // SceneManager.LoadScene( /* main menu scene needed */ );
    }
    void QuitGame() { Application.Quit(); }
}