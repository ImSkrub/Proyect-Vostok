using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Stack<PlayerMemento> savedStates = new Stack<PlayerMemento>();
    public PlayerLife playerLife;
    public Player player;

    [SerializeField] Transform spawnpoint;

    private bool isRestoring =false;
    private void Start()
    {
        spawnpoint = this.transform;
        playerLife = FindObjectOfType<PlayerLife>();
        player = FindObjectOfType<Player>();
        playerLife.OnDeath += _Checkpoint;
        player.onRestart += _Checkpoint;
    }

    public void _Checkpoint()
    {
        if (isRestoring) return;
        isRestoring = true;
        if (HasSavedStates())
        {
            PlayerMemento lastSavedState =savedStates.Pop();
            playerLife.RestoreState(lastSavedState);
            player.startPos = spawnpoint.position;
            Debug.Log("Estado Restaurado");
        }
        else
        {
            Debug.Log("No saved states available to restore.");
        }
        isRestoring = false;
    }


    public bool HasSavedStates()
    {
        return savedStates.Count > 0;

    }

    public void ResetStates()
    {
        savedStates.Clear();
        Debug.Log("Saved states have been Reset.");
    }

}



