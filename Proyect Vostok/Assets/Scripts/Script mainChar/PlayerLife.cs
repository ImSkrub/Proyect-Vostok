using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    public float CurrentHealth => currentHealth;
    public float maxHealth = 100; //Maximo de vida
    public float damageCooldown = 1f; //Daño
    public float currentHealth; //Vida que llevas en el juego
    private SpriteRenderer spriteRenderer; //Renderizado barra
    private float currentTime;
    public event Action OnDeath; //Muerte del jugador como evento.
    private Animator anim;
   
    private int deathCount=0;
    public Transform respawn;

    //Color al recibir daño.
    public Color damageColor = Color.red;
    private Color originalColor;


    //Stat de vida en Canvas
    public Image lifebar;

    private Checkpoint playerCheckpoint;
    public bool isDead =false;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        anim= GetComponent<Animator>();
    }

    private void Update()
    {
        lifebar.fillAmount = currentHealth / maxHealth;

        currentTime += Time.deltaTime;

        //Daño
        if (currentHealth <= 0)
        {
            Die();
        }
        if(deathCount >=1)
        {
            GameManager.Instance.LoseGame();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            currentHealth = maxHealth;
        }
    }

    //Recibie daño
    public void GetDamage(int value)
    {
        currentHealth -= value; //currentHealth = currentHealth - value; 
        spriteRenderer.color = damageColor;
        Invoke("RestoreColor", 0.5f);
    }

    private void RestoreColor()
    {
        // Restaurar el color original del sprite
        spriteRenderer.color = originalColor;
    }

  
   //Muere.
    public void Die()
    {
        if (isDead) return;
        if(playerCheckpoint != null && playerCheckpoint.HasSavedStates())
        {
            Debug.Log(playerCheckpoint.HasSavedStates());
            playerCheckpoint._Checkpoint(); // Restore the last saved state
            Debug.Log("Player restored from checkpoint.");
        }
        else
        {
            Debug.Log("No saved states available. Player is dead.");
            isDead = true;
            // anim.SetBool("IsDead", isDead);
            // Invoke("InvokeEvent", 2.5f);
            InvokeEvent();
            //modificar tiempo del invoke en relacion a la duracion de la anim muerte
        }
        deathCount++;
        isDead = true;
        
    }

    private void InvokeEvent()
    {
        OnDeath?.Invoke();
    }


    public PlayerMemento SaveState(Vector3 position)
    {
        return new PlayerMemento(position, maxHealth);
    }

    public void RestoreState(PlayerMemento state)
    {
        if (state != null)
        {
            gameObject.SetActive(true);
            transform.position = state.position;
            currentHealth = state.health;
            isDead = false; // Aseg�rate de que el jugador est� vivo despu�s de restaurar
            Debug.Log("Player state restored.");
        }
        else
        {
            Debug.LogError("Cannot restore state: state is null.");
        }
    }

}
