using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject respawn;
    private bool isReseted = false;
    [SerializeField] GameObject player;

     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            respawn.transform.position = collision.transform.position;
            player.GetComponent<player>().StartPos = respawn.transform.position;
            if (!isReseted)
            {
                GameManager.Instance.ResetList();
                player.GetComponent<player>().listCopyDataModels.Clear();
                isReseted =true;
            }
        }
    }
}
