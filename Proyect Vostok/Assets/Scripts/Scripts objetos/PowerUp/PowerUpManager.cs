using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }
    public void DeactivatePowerUp()
    {
        foreach (var powerUp in powerUp)
        {
            powerUp.SetActive(false);
            GameManager.Instance.powerUpDisabled.Add(powerUp);
        }
    }
}
