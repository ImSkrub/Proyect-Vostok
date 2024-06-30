using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] int newCopyNumber = 5;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelManager.instance.LoadNextLevel();
            GameManager.Instance.ResetList();
            GameManager.Instance.CopyNumberChange(newCopyNumber);
            PowerUpManager.Instance.powerUp.Clear();
        }
    }
}
