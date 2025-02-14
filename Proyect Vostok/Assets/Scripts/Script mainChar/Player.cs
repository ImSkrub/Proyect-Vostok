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
   
    [Header("Jetpack")]
    [SerializeField] private float jetpackForce = 15f;
    [SerializeField] private ParticleSystem jetpackParticle;
    public float jetpackFuel = 1;
    public bool jetpackOn = false;
    
    [Header("Copia")]
    public List<CopyDataModel> listCopyDataModels = new List<CopyDataModel>();
    public Vector2 startPos;
    public event Action onRestart;
    
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        animHandler = GetComponent<PlayerAnimator>();
        health = GetComponent<PlayerLife>();
        gameManager = FindObjectOfType<GameManager>();
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
            GameManager.Instance.UpdateQueueOfPositions(listCopyDataModels);
            GameManager.Instance._Reset();
            transform.position = startPos;
            listCopyDataModels.Clear();
            onRestart?.Invoke();
          //  GameManager.Instance.ActivatePowerUp();
            jetpackParticle.Stop();
            jetpackOn = false;
        }
    }

    private void FixedUpdate()
    {
        CopyDataModel copyDataModel = new CopyDataModel(transform.position, "default");
        listCopyDataModels.Add(copyDataModel);
        HandleJetPack();
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
