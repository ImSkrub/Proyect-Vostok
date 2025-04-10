using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    private PlayerController controller;
    private PlayerView playerView;
    private PlayerLife health;
    public PlayerLife Health
    {
        get { return health; } 
        private set { health = value; }
    }
    private GameManager gameManager;
    private Rigidbody2D rb;
    private PowerUpManager powerUpManager;
    private PlayerRaycast playerRaycast;
   
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
        playerView = GetComponent<PlayerView>();
        health = GetComponent<PlayerLife>();
        gameManager = FindObjectOfType<GameManager>();
        powerUpManager = FindObjectOfType<PowerUpManager>();
        rb = GetComponent<Rigidbody2D>();
        playerRaycast = GetComponent<PlayerRaycast>();
    }
    private void Start()
    {
        startPos = transform.position;
        jetpackParticle.Stop();

    }
    private void Update()
    {
        //Copia
        if (Input.GetKeyDown(KeyCode.R))
        {
            HandleCopy();
            playerRaycast.ResetTimer();
        }
        //Copia
        if (Input.GetKeyDown(KeyCode.T))
        {
            HardReset();
            playerRaycast.ResetTimer();
        }
    }

    private void FixedUpdate()
    {
        CopyDataModel copyDataModel = new CopyDataModel(transform.position, "default");
        listCopyDataModels.Add(copyDataModel);
        HandleJetPack();
    }

    public void HardReset()
    {
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


        GameManager.Instance.ClearListOfPositions();
        GameManager.Instance.ClearCopiaPlayerList();
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
        var jetPackInput = Input.GetButtonDown("Jump");
        if (jetpackOn)
        {
            
            if (jetPackInput && jetpackFuel > 0)
            {
               
                rb.AddForce(Vector2.up * jetpackForce,ForceMode2D.Impulse);
                jetpackParticle.Play();
                jetpackFuel -= Time.deltaTime;  
            }
            else
            {
                jetpackParticle.Stop();     
            }
            
            if (jetpackFuel <= 0)
            {
                Debug.Log("El jugador se saco el jetpack");
                jetpackOn = false; // Disable the jetpack
                jetpackParticle.Stop(); // Stop particles
                playerView.SetJetpackState(false);
                
            }
        }
       
    }
    public void activateJetPack()
    {
        Debug.Log("El jugador se puso el jetpack");
        jetpackOn = true;
        jetpackParticle.Play(); // Start particles when activating the jetpack
        playerView.SetJetpackState(true);
    }

    public void AddJetpackFuel(int value)
    {
        jetpackFuel += value;
        activateJetPack();
    }
    #endregion

}
