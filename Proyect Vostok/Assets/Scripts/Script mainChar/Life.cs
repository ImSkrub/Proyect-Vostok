using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public float CurrentHealth => currentHealth;
    public float maxHealth = 100; //Maximo de vida
    public float damageCooldown = 1f; //Daño
    public float currentHealth; //Vida que llevas en el juego
    private SpriteRenderer spriteRenderer; //Renderizado barra
    private float currentTime;
    public event Action OnDeath; //Muerte del jugador como evento.
    //public DeathCounter deathCounter;
    private int deathCount=0;
    public Transform respawn;

    //Color al recibir daño.
    public Color damageColor = Color.red;
    private Color originalColor;


    //Stat de vida en Canvas
    public Image lifebar;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

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
        gameObject.transform.position = respawn.position;
        currentHealth = maxHealth;
        deathCount++;
        OnDeath?.Invoke();
    }

    
}
