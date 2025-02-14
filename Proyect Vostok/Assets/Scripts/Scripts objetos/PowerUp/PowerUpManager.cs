using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;
    //Lista que guarda los powerUp.
    [SerializeField] public List<GameObject> powerUp = new List<GameObject>();

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    public void DeactivatePowerUp()
    {
        foreach (var _powerUp in powerUp)
        {
            _powerUp.SetActive(false);
            GameManager.Instance.powerUpDisabled.Add(_powerUp);
            if(_powerUp.TryGetComponent<CollisionItems>(out CollisionItems component))
            {
                component.indexList = powerUp.Count - 1;
            }
        }
    }
}
