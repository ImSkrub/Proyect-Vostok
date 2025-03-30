using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [Header("Atributos")]
    private PlayerView playerView;
    public float CurrentHealth => currentHealth;
    public float maxHealth = 100;
    public float damageCooldown = 1f;
    public float currentHealth;
    private SpriteRenderer spriteRenderer;
    private float currentTime;
    public event Action OnDeath;

    private int deathCount = 0;
    public Transform respawn;

    public Color damageColor = Color.red;
    private Color originalColor;

    public Image lifebar;

    private Checkpoint playerCheckpoint;
    public bool isDead = false;
    private Player player;
    [Space]
    [Header("Death timer")]
    public float deathAnimationDuration = 2.5f; // Duración de la animación de muerte

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
        playerView = GetComponent<PlayerView>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        lifebar.fillAmount = currentHealth / maxHealth;
        currentTime += Time.deltaTime;

        if (currentHealth <= 0)
        {
            HandleDeath();
        }

        if (deathCount >= 1)
        {
            GameManager.Instance.LoseGame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentHealth = maxHealth;
        }
    }

    public void AddHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    public void GetDamage(int value)
    {
        currentHealth -= value;
        spriteRenderer.color = damageColor;
        Invoke("RestoreColor", 0.5f);
    }

    private void RestoreColor()
    {
        spriteRenderer.color = originalColor;
    }

    private void HandleDeath()
    {
        if (isDead) return;

        if (playerCheckpoint != null && playerCheckpoint.HasSavedStates())
        {
            playerCheckpoint._Checkpoint();
            Debug.Log("Player restored from checkpoint.");
        }
        else
        {
            Debug.Log("No saved states available. Player is dead.");
            isDead = true;
            deathCount++;
            playerView.TriggerDeathAnimation();
            Invoke("InvokeEvent", deathAnimationDuration); // Esperar antes de ejecutar el evento
        }
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
            player.startPos = state.position;
            currentHealth = state.health;
            isDead = false;
            Debug.Log("Player state restored.");
        }
        else
        {
            Debug.LogError("Cannot restore state: state is null.");
        }
    }
}
