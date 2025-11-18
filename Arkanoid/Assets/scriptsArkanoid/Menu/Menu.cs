using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _levelsPanel;

    [Header("Levels")]
    [SerializeField] private LevelsSettings[] _levelsSettings;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        _settingsPanel.SetActive(true);
        _levelsPanel.SetActive(false);
    }

    public void ShowLevels()
    {
        _settingsPanel.SetActive(false);
        _levelsPanel.SetActive(true);
    }

    public void LoadLevel(int levelIndex)
    {
        StaticActions.CurrentLevelSettings = _levelsSettings[levelIndex];
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}