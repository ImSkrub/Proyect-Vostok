using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [Header("PowerUp")]
    [SerializeField] public List<GameObject> activePowerUps = new List<GameObject>();
    [SerializeField] public List<GameObject> powerUpDisabled = new List<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        FindPowerUpsInScene();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void NotifyPowerUpUsed(GameObject powerUp)
    {
        if (activePowerUps.Contains(powerUp))
        {
            activePowerUps.Remove(powerUp);
            powerUpDisabled.Add(powerUp);
        }
    }

    public void ReactivatePowerUps()
    {
        foreach (var powerUp in powerUpDisabled)
        {
            powerUp.SetActive(true);
            activePowerUps.Add(powerUp);
        }
        powerUpDisabled.Clear();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        activePowerUps.Clear();
        powerUpDisabled.Clear();
        FindPowerUpsInScene();
    }

    private void FindPowerUpsInScene()
    {
        CollisionItems[] powerUpItems = FindObjectsOfType<CollisionItems>();

        foreach (CollisionItems item in powerUpItems)
        {
            if (!activePowerUps.Contains(item.gameObject))
            {
                activePowerUps.Add(item.gameObject);
            }
        }

        Debug.Log($"Se encontraron {activePowerUps.Count} power-ups en la escena.");
    }
}