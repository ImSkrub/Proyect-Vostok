using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class player : MonoBehaviour
{
    public PlayerData Data;
   
    /*
     *Variables you have to access player data:
     *runMaxSpeed,runAccelAmount,runDeccelAmount,accelInAir,deccelInAir,jumpHangTimeThreshold.
     *jumpHangAccelerationMult,jumpHangMaxSpeedMult,doConserveMomentum,
    */

    [SerializeField] GameObject gameManager;
    private GameManager gameManagerInstance;

    #region VARIABLES
    [SerializeField] private Rigidbody2D RB;
    private TrailRenderer trailRenderer;

    //Jump
    public bool IsFacingRight = true;
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    //wallSlide
    public bool IsWallSliding;
    //wallJump
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    [SerializeField]private Vector2 wallJumpingPower = new Vector2(8f, 16f);


    [Header("Checks")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.03f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    #endregion

    //Salto
    public float jumpForce = 15;
    private float currentTime;

    /*
    Funcion de dash para mas adelante, todavia no se va a usar.
    
    [Header("Dash")]
    [SerializeField] private float dashingVelocity = 24f;
    [SerializeField] private float dashingTime = 0.3f;
    private float dashingCooldown = 1f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    */

    //evento plataformas
    public event EventHandler OnJump;

    //Vectores para guardar posiciones e input.

    //first position
    private Vector2 moveInput;
    public float LastPressedJumpTime { get; private set; }

    //last position
    private Vector2 lastVector;

    //Listas que guardan el movimiento del jugador.
    public List<Vector3> playerPositions = new List<Vector3>();
    public List<List<Vector3>> listPlayerPositions = new List<List<Vector3>>();

    //Start del juego.
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        lastVector = RB.velocity;
        gameManager.TryGetComponent<GameManager>(out gameManagerInstance);
        moveInput= gameObject.transform.position;

    }

    private void Start()
    {
        IsFacingRight = true;
    }

    //Update del juego.
    private void Update()
    {
     
        ///////Movimiento y teclas//////.     
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        
       

        //Saltar
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            RB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            OnJump?.Invoke(this, EventArgs.Empty);
        }

        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;

        }

        /*
        if (CanJump() && LastPressedJumpTime > 0)
        {
            IsJumping = true;
            IsWallJumping = false;
            Jump();
        }*/
        /*
        #region DASH
        var dashInput = Input.GetButtonDown("Dash");
        
        //dash
        
        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            trailRenderer.emitting = true;
            dashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if(dashingDir== Vector2.zero)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }
            StartCoroutine(StopDashing());
        }

        //para animacion      animator.SetBool("isDashing",isDashing);

        if (isDashing)
        {
            RB.velocity = dashingDir.normalized * dashingVelocity;
            return;
        }
        if (IsGrounded())
        {
            canDash = true;
        }
        #endregion*/
       
        WallSlide();
        WallJump();

        if (!IsWallJumping)
        {
            Turn();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //listPlayerPositions.Add(playerPositions);
            gameManagerInstance.UpdateListOfPositions(playerPositions);

            //Life guarda el respawnpoint para reiniciar las posiciones, como lo relaciono?

            playerPositions.Clear();

            

            gameManagerInstance.instantiateListOfObjects();

        }
    }

    private void FixedUpdate()
    {
        if (!IsWallJumping)
        {
            Run();
        }
     }

    #region RUN METHOD
    private void Run()
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = moveInput.x * Data.runMaxSpeed;
       
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
        if ((IsJumping || IsWallJumping ) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }
    #endregion

    #region Jump methods

    //trampolin
    public void ForcedJump()
    {
      RB.AddForce(Vector2.up * 600);
    }

    public bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;   
    }

    private void WallJump()
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

        if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            IsWallJumping = true;
            RB.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection)
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

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && moveInput.x != 0f)
        {
            IsWallSliding = true;
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Clamp(RB.velocity.y, -5, float.MaxValue));
        }
        else
        {
            IsWallSliding = false;
        }        
    }
    #endregion
    /*
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false;
    }
    */
    #region GENERAL METHODS
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void Turn()
    {
     
        if(IsFacingRight && moveInput.x < 0f|| !IsFacingRight && moveInput.x > 0f)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }
      
    #endregion

}
