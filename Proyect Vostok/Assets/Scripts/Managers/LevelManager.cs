
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
    }

    public void LoadMainMenu()
    {
        currentLevelIndex = 1;
        LoadLevel(0);
        
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
