using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpawnpoint : MonoBehaviour
{
    [SerializeField] private Checkpoint checkpoint;
    [SerializeField] private bool wasTriggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !wasTriggered)
        {
            checkpoint.SaveState();
            GameManager.Instance.ClearListOfPositions();
            GameManager.Instance.ClearCopiaPlayerList();
            wasTriggered = true;
            Debug.Log("Checkpoint activado.");
        }
    }
}
