using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class player : MonoBehaviour
{
    [Header("Variables")]
    public PlayerData Data;

    /*
     *Variables you have to access player data:
     *runMaxSpeed,runAccelAmount,runDeccelAmount,accelInAir,deccelInAir,jumpHangTimeThreshold.
     *jumpHangAccelerationMult,jumpHangMaxSpeedMult,doConserveMomentum,
    */

    [SerializeField] GameObject gameManager;
    private GameManager gameManagerInstance = GameManager.Instance;

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
    private float wallSlidingSpeed = 5f;
    //wallJump
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);


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

    [Header("Dash")]
    [SerializeField] private float dashingVelocity = 24f;
    [SerializeField] private float dashingTime = 0.3f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    private bool isDashEnabled = false;
    public bool _isDashEnabled => isDashEnabled;
    private Vector3 starPos;

    //[Header("DashColor")]
    //[Tooltip("Material to switch during dash")]
    //[SerializeField] private Material flashMaterial;
    //private SpriteRenderer spriteRenderer;
    //private Material originalMaterial;

    [Header("JetPack")]
   // public GameObject jetPack;
    [SerializeField] private float jetPackForce = 15f;
    public float jetPackFuel;
    private bool jetPackOn = false;


    //evento plataformas
    public event EventHandler OnJump;

    private Vector2 moveInput;
    public float LastPressedJumpTime { get; private set; }



    //last position
    private Vector2 firstPosition;
    private Vector2 lastPosition;

    //Listas que guardan el movimiento del jugador.
    public List<CopyDataModel> listCopyDataModels = new List<CopyDataModel>();
  
    /*COPIA
    En vez de lista, usar Queue, despues tener una variable para limitar a la cola, despues guardo por posiciones, hace de pos1 a pos2.
    Puedo ponerle una cantidad maxima de posiciones a guardar, a modo de que se vaya eliminando las anteriores posiciones.
    Pregunto diferencias si tengo que ajustar o no. Para manejar las animaciones
    Crear un script de modelo(Como el PlayerData), que tenga la posicion, accion y lo que quiere hacer. 
    Y hago una Queue de ese modelo
     */

    //Start del juego.
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        //jetPack.SetActive(false);
    }

    private void Start()
    {
        IsFacingRight = true;
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //originalMaterial = spriteRenderer.material;
        starPos = transform.position;
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
        var jetPackInput = Input.GetButton("Jump");
        if (jetPackOn)
        {
            if (jetPackInput && !IsGrounded())
            {
                RB.AddForce(Vector2.up * jetPackForce);
                
            }
            StartCoroutine(stopJetPack());
        }

        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;

        }

        //Copia
        if (Input.GetKeyDown(KeyCode.R))
        {
            //gameManager.TryGetComponent<GameManager>(out GameManager component);
            GameManager.Instance.UpdateListOfPositions(listCopyDataModels);
            GameManager.Instance._Reset();
            transform.position = starPos;
            listCopyDataModels.Clear();
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
            RB.velocity = dashingDir.normalized * dashingVelocity;
            return;
        }
        if (IsGrounded())
        {
            canDash = true;
        }
        #endregion

        WallSlide();
        WallJump();




        if (!IsWallJumping)
        {
            Turn();
        }

    }

    private void FixedUpdate()
    {
        CopyDataModel copyDataModel = new CopyDataModel(transform.position, "default");
        if (!IsWallJumping)
        {
            Run();
        }
        listCopyDataModels.Add(copyDataModel);
        //listPosition.Add(transform.position);

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
        if ((IsJumping || IsWallJumping) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
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

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            IsWallJumping = true;
            RB.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
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

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && moveInput.x != 0f)
        {
            IsWallSliding = true;
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Clamp(RB.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            IsWallSliding = false;
        }
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

    //public void Flash()
    //{
    //    if(flashRoutine != null)
    //    {
    //        StopCoroutine(flashRoutine);
    //    }
    //    flashRoutine = StartCouroutine(FlashRoutine);
    //}

    //private IEnumerator FlashRoutine()
    //{
    //    spriteRenderer.material = flashMaterial;
    //    yield return WaitForSecond;
    //}

    public void activateJetPack()
    {
       // jetPack.SetActive(true);
        jetPackOn = true;
    }

    private IEnumerator stopJetPack()
    {
        yield return new WaitForSeconds(jetPackFuel);
        // jetPack.SetActive(false);
        jetPackOn = false;
        //Zona de animaciones.
        
    }

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

        if (IsFacingRight && moveInput.x < 0f || !IsFacingRight && moveInput.x > 0f)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    #endregion

}
