using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Stack<PlayerMemento> savedStates = new Stack<PlayerMemento>();
    public PlayerLife playerLife;
    public Player player;

    private bool isRestoring =false;
    private void Start()
    {
        playerLife = FindObjectOfType<PlayerLife>();
        player = FindObjectOfType<Player>();
        playerLife.OnDeath += _Checkpoint;
    }
    public void SaveState()
    {
        PlayerMemento memento = new PlayerMemento(player.transform.position, playerLife.currentHealth);
        savedStates.Push(memento);
        player.listCopyDataModels.Clear();
        Debug.Log("Estado guardado en el checkpoint.");
    }

    public void _Checkpoint()
    {
        if (isRestoring) return;
        isRestoring = true;

        if (HasSavedStates())
        {
            PlayerMemento lastSavedState = savedStates.Pop();
            playerLife.RestoreState(lastSavedState);
            player.transform.position = lastSavedState.position; // Restaurar la posici�n del checkpoint
            Debug.Log("Estado restaurado desde el checkpoint.");
        }
        else
        {
            Debug.Log("No hay estados guardados para restaurar.");
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



