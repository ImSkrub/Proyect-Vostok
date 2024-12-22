using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    public PlayerData Data;
    private StateMachine stateMachine;
    public StateMachine StateMachine => stateMachine;
    private GameObject gameManager;
    private GameManager gameManagerInstance = GameManager.Instance;

    #region VARIABLES
    [SerializeField] public Rigidbody2D rb;
    private TrailRenderer trailRenderer;

    //Jump
    public bool IsFacingRight = true;
    public bool IsJumping { get; private set; }
    public bool IsWallJumping;
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    //wallSlide
    public bool IsWallSliding;
    private float wallSlidingSpeed = 5f;
    //wallJump
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [Space(3)]
    [Header("Checks")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.03f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    #endregion

    //Salto
    public float jumpForce = 15;
    private float currentTime;
    [Space(3)]
    [Header("Dash")]
    [SerializeField] private float dashingVelocity = 24f;
    [SerializeField] private float dashingTime = 0.3f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    private bool isDashEnabled = false;
    public bool _isDashEnabled => isDashEnabled;
    private Vector3 startPos;

    public Vector3 StartPos { get { return startPos; } set {  startPos = value; } }

    
    [Header("JetPack")]
   // public GameObject jetPack;
    [SerializeField] private float jetPackForce = 15f;
    public float jetPackFuel;
    public bool jetPackOn = false;
    [SerializeField] private ParticleSystem jetPackParticle;
    
    public Vector2 moveInput;
    public float LastPressedJumpTime { get; private set; }

    //last position
    private Vector2 firstPosition;
    private Vector2 lastPosition;
    [Space(3)]
    [Header("Copias")]
    public List<CopyDataModel> listCopyDataModels = new List<CopyDataModel>();

    public Animator anim;

    //Start del juego.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
        stateMachine = new StateMachine();
        stateMachine.InitializeStates(this);
        stateMachine.Initialize(stateMachine.idleState);
    }

    private void Start()
    {
        //SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
        startPos = transform.position;
        jetPackParticle.Stop();
    }

    //Update del juego.
    private void Update()
    {
        
        //Copia
        if (Input.GetKeyDown(KeyCode.R))
        {
            _Reset();
          
        }

        #region DASH
        //dash

        var dashInput = Input.GetButtonDown("Dash");
        if (isDashEnabled)
        {
            if (dashInput && canDash)
            {
                isDashing = true;
                canDash = false;
                trailRenderer.emitting = true;
                dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (dashingDir == Vector2.zero)
                {
                    dashingDir = new Vector2(transform.localScale.x, 0);
                }
                StartCoroutine(StopDashing());
            }
        }

        //para animacion      animator.SetBool("isDashing",isDashing);

        if (isDashing)
        {
            rb.velocity = dashingDir.normalized * dashingVelocity;
            return;
        }
        if (IsGrounded())
        {
            canDash = true;
        }
        #endregion

        //if (IsWalled() && !IsGrounded())
        //{
        //    IsWallSliding = true;
        //    WallSlide();
        //}
        //else
        //{
        //    IsWallSliding = false;
        //}

        //WallJump();

        //if (!IsWallJumping)
        //{
        //    Turn();
        //}




        //#region GRAVITY 
        //       else if (RB.velocity.y < 0)
        //{
        //    //Higher gravity if falling
        //    SetGravityScale(Data.gravityScale * Data.fallGravityMult);
        //    //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
        //    RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
        //}
        //else
        //{ 
        //    SetGravityScale(Data.gravityScale); 
        //}

        //#endregion
    }

    private void FixedUpdate()
    {
        ///////Movimiento y teclas//////.     
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
       
        stateMachine.UpdateState();

        CopyDataModel copyDataModel = new CopyDataModel(transform.position, "default");
                
        listCopyDataModels.Add(copyDataModel);
        HandleJetPack();
       
    }

    #region RUN METHOD
    public void Run()
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = moveInput.x * Data.runMaxSpeed;
        Debug.Log($"Running: velocity.x = {rb.velocity.x}, targetSpeed = {moveInput.x * Data.runMaxSpeed}");

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((IsJumping || IsWallJumping) && Mathf.Abs(rb.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (Data.doConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rb.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }
    #endregion

    #region Jump methods

    public void WallJump()
    {
        if (IsWallSliding)
        {
            IsWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            IsWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                IsFacingRight = !IsFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        IsWallJumping = false;
    }

    public void WallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        
    }
    #endregion

    #region DASH METHODS

    public void activateDash()
    {
        isDashEnabled = true;
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false;
        isDashEnabled = false;
    }


    #endregion

    #region JETPACK
    public void HandleJetPack()
    {
        var jetPackInput = Input.GetButton("Jump");
        if (jetPackOn)
        {
            if (jetPackInput && jetPackFuel > 0)
            {
                rb.AddForce(Vector2.up * jetPackForce);
                jetPackParticle.Play();
                jetPackFuel -= Time.deltaTime; // Decrease fuel over time while the button is held
            }
            else
            {
                jetPackParticle.Stop(); // Stop particles if the button is released
            }

            // If fuel runs out, stop the jetpack
            if (jetPackFuel <= 0)
            {
                jetPackOn = false; // Disable the jetpack
                jetPackParticle.Stop(); // Stop particles
            }
        }
        
    }
    public void activateJetPack()
    {
        jetPackOn = true;
        anim.SetBool("Jetpack", true);
        jetPackParticle.Play(); // Start particles when activating the jetpack
    }


    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    public void Turn()
    {

        if (IsFacingRight && moveInput.x < 0f || !IsFacingRight && moveInput.x > 0f)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    private void _Reset()
    {
        GameManager.Instance.UpdateQueueOfPositions(listCopyDataModels);
        GameManager.Instance._Reset();
        transform.position = startPos;
        listCopyDataModels.Clear();
        GameManager.Instance.ActivatePowerUp();
        jetPackParticle.Stop();
        jetPackOn = false;
        isDashEnabled = false;
    }

    #endregion

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheck.position, _wallCheckSize);        
    }
    #endregion
}
