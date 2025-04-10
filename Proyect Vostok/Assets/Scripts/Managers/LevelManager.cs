
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private string[] nonPlayableScenes = { "Menu", "Lose", "Win", "LevelSelect"};

    private int lastPlayableLevelIndex;
    public int currentLevel => lastPlayableLevelIndex;

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
            return;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        if (!IsNonPlayableScene(sceneName))
        {
            lastPlayableLevelIndex = scene.buildIndex;
            Debug.Log($"Escena jugable cargada: {scene.name}, Índice guardado: {lastPlayableLevelIndex}");
        }
        else
        {
            Debug.Log($"Escena no jugable: {scene.name}, se mantiene el índice: {lastPlayableLevelIndex}");
        }
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.ReactivatePowerUps();
        }
        else
        {
            Debug.LogWarning("PowerUpManager.Instance es nulo. Asegúrate de que haya un PowerUpManager en la escena.");
        }

        if (lastPlayableLevelIndex != 8)
        {
            // AudioManager.instance.PlayMusic("Music2");
        }
    }

    private bool IsNonPlayableScene(string sceneName)
    {
        foreach (var nonPlayable in nonPlayableScenes)
        {
            if (sceneName == nonPlayable)
                return true;
        }
        return false;
    }

    public void LoadNextLevel()
    {
        LoadLevel(lastPlayableLevelIndex + 1);
    }

    public void RestartLevel()
    {
        Debug.Log($"Me resetee en el nivel {lastPlayableLevelIndex}");
        LoadLevel(lastPlayableLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        if (Application.CanStreamedLevelBeLoaded(levelIndex))
        {
            string sceneName = SceneUtility.GetScenePathByBuildIndex(levelIndex);
            string trimmedSceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);

            if (!IsNonPlayableScene(trimmedSceneName))
            {
                // Guardamos antes de cargar si es jugable
                lastPlayableLevelIndex = levelIndex;
                Debug.Log($"[LevelManager] Guardando índice jugable ANTES de cargar: {lastPlayableLevelIndex}");
            }
            else
            {
                Debug.Log($"[LevelManager] Cargando escena NO jugable (índice guardado sigue en: {lastPlayableLevelIndex})");
            }

            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogWarning($"No se puede cargar la escena con índice {levelIndex}. ¿Está incluida en el Build Settings?");
        }
    }

    public void LoadMainMenu()
    {
        LoadLevel(0);
        // AudioManager.instance.PlayMusic("Music1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public int GetCurrentLevelIndex()
    {
        return lastPlayableLevelIndex;
    }
}
