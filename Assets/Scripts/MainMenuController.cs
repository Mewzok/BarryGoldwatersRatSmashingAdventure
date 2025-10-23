using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject difficultyPanel;

    [Header("Buttons")]
    public Button electionButton;
    public Button endlessButton;
    public Button backButton;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    private string selectedMode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);

        electionButton.onClick.AddListener(() => ShowDifficulty("Election"));
        endlessButton.onClick.AddListener(() => ShowDifficulty("Endless"));
        backButton.onClick.AddListener(ShowMainMenu);

        easyButton.onClick.AddListener(() => LoadGame("Easy"));
        mediumButton.onClick.AddListener(() => LoadGame("Medium"));
        hardButton.onClick.AddListener(() => LoadGame("Hard"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowDifficulty(string mode) {
        selectedMode = mode;
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    void ShowMainMenu() {
        mainMenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
    }

    void LoadGame(string difficulty) {
        GameSettings.mode = selectedMode;
        GameSettings.difficulty = difficulty;

        SceneManager.LoadScene("RatScene");
    }
}
