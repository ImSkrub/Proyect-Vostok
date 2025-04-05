using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

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

    private Checkpoint playerCheckpoint;
    public bool isDead = false;
    private Player player;

    [Header("Death Animation")]
    public string deathAnimationName = "death"; 
    public float deathAnimationDuration = 1.5f;

    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
        playerView = GetComponent<PlayerView>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        originalColor = spriteRenderer.color;

        // Detectar duración de la animación de muerte
        var clip = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name == deathAnimationName);

        if (clip != null)
        {
            deathAnimationDuration = clip.length;
            
        }
        else
        {
            Debug.LogWarning($"No se encontró el clip de animación '{deathAnimationName}'. Usando valor por defecto.");
        }
    }

    private void Update()
    {
       
        currentTime += Time.deltaTime;

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
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

    private IEnumerator HandleDeath()
    {
        if (isDead) yield break;

        if (playerCheckpoint != null && playerCheckpoint.HasSavedStates())
        {
            playerCheckpoint._Checkpoint();
            Debug.Log("Player restored from checkpoint.");
        }
        else
        {
            Debug.Log("No saved states available. Player is dead.");
            isDead = true;
            playerView.TriggerDeathAnimation();
            yield return new WaitForSeconds(deathAnimationDuration);
            InvokeEvent();
        }
    }

    private void InvokeEvent()
    {
        OnDeath?.Invoke();
        deathCount++;
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
