using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    private PlayerController controller;
    private PlayerAnimator animHandler;
    private PlayerLife health;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private PowerUpManager powerUpManager;
   
    [Header("Jetpack")]
    [SerializeField] private float jetpackForce = 15f;
    [SerializeField] private ParticleSystem jetpackParticle;
    public float jetpackFuel = 1;
    public bool jetpackOn = false;
    
    [Header("Copia")]
    public List<CopyDataModel> listCopyDataModels = new List<CopyDataModel>();
    public Vector2 startPos;
    
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        animHandler = GetComponent<PlayerAnimator>();
        health = GetComponent<PlayerLife>();
        gameManager = FindObjectOfType<GameManager>();
        powerUpManager = FindObjectOfType<PowerUpManager>();

    }
    private void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        startPos = transform.position;
        jetpackParticle.Stop();

    }
    private void Update()
    {
        
        //Copia
        if (Input.GetKeyDown(KeyCode.R))
        {
            HandleCopy();
        }
    }

    private void FixedUpdate()
    {
        CopyDataModel copyDataModel = new CopyDataModel(transform.position, "default");
        listCopyDataModels.Add(copyDataModel);
        HandleJetPack();
    }
    
    public void HandleCopy()
    {
        GameManager.Instance.UpdateQueueOfPositions(listCopyDataModels);
        GameManager.Instance._Reset();

        // Restaurar la posición desde el último checkpoint
        Checkpoint checkpoint = FindObjectOfType<Checkpoint>();
        if (checkpoint != null && checkpoint.HasSavedStates())
        {
            PlayerMemento lastSavedState = checkpoint.savedStates.Peek(); // Obtener el último estado sin eliminarlo
            transform.position = lastSavedState.position;
            Debug.Log("Posición restaurada desde el checkpoint.");
        }
        else
        {
            transform.position = startPos; // Volver a la posición inicial si no hay checkpoints
            Debug.Log("No hay checkpoints, reiniciando a la posición inicial.");
        }

        listCopyDataModels.Clear();

        if (powerUpManager != null)
        {
            powerUpManager.ReactivatePowerUps();
        }
        else
        {
            Debug.LogWarning("PowerUpManager no encontrado en la escena.");
        }

        jetpackParticle.Stop();
        jetpackOn = false;
    }

    #region JETPACK
    public void HandleJetPack()
    {
        var jetPackInput = Input.GetButton("Jump");
        if (jetpackOn)
        {
            if (jetPackInput && jetpackFuel > 0)
            {
                rb.AddForce(Vector2.up * jetpackForce,ForceMode2D.Impulse);
                jetpackParticle.Play();
                jetpackFuel -= Time.deltaTime; // Decrease fuel over time while the button is held
            }
            else
            {
                jetpackParticle.Stop(); // Stop particles if the button is released
            }
            // If fuel runs out, stop the jetpack
            if (jetpackFuel <= 0)
            {
                jetpackOn = false; // Disable the jetpack
                jetpackParticle.Stop(); // Stop particles
            }
        }

    }
    public void activateJetPack()
    {
        jetpackOn = true;
        jetpackParticle.Play(); // Start particles when activating the jetpack
    }

    #endregion
    //Aca va todo lo que no tenga que ver con el movimiento, copia, jetpack y colisiones.

}
