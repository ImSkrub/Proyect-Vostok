
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private int currentLevelIndex = 1;
    public int currentLevel => currentLevelIndex;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        AudioManager.instance.PlayMusic("Music1");

    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        LoadLevel(currentLevelIndex);
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
        
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        currentLevelIndex = levelIndex;
        AudioManager.instance.PlayMusic("Music2");
    }

    public void LoadMainMenu()
    {
        currentLevelIndex = 1;
        LoadLevel(0);
        AudioManager.instance.PlayMusic("Music1");
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }
}
