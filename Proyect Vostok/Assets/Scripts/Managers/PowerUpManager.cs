using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;
    //Lista que guarda los powerUp.
    [Header("PowerUp")]
    [SerializeField] public List<GameObject> powerUp = new List<GameObject>();
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
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindPowerUpsInScene();
            
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void DeactivatePowerUp()
    {
        foreach (var _powerUp in powerUp)
        {
            if (_powerUp.activeSelf)
            {
                _powerUp.SetActive(false);
               powerUpDisabled.Add(_powerUp);
                if (_powerUp.TryGetComponent<CollisionItems>(out CollisionItems component))
                {
                    component.indexList = powerUp.Count - 1;
                }
            }
        }
    }
    public void ReactivatePowerUps()
    {
        foreach (var _powerUp in powerUp)
        {
            _powerUp.SetActive(true);
        }
        powerUpDisabled.Clear();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        powerUp.Clear();
        FindPowerUpsInScene();
    }
    private void FindPowerUpsInScene()
    {
        CollisionItems[] powerUpItems = FindObjectsOfType<CollisionItems>();

        foreach (CollisionItems item in powerUpItems)
        {
            if (!powerUp.Contains(item.gameObject))
            {
                powerUp.Add(item.gameObject);
            }
        }

        Debug.Log($"Se encontraron {powerUp.Count} power-ups en la escena.");
    }
}
