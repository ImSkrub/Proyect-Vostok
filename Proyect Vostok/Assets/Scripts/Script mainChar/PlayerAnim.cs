using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private PlayerController controller;
    private Animator anim;

    public bool startedJumping {  get; private set; }
    public bool justLanded { get; private set; }

    public bool jetpackActive = false;


    public float currentVelY;


    private void Start()
    {
        controller= GetComponent<PlayerController>();
        anim = GetComponent<Animator>();

    }
    public void TriggerJumpAnimation()
    {
        anim.SetTrigger("Jump");
    }

    public void TriggerLandAnimation()
    {
        anim.SetTrigger("Land");
    }

    public void UpdateMovementAnimation(float moveSpeed, float velocityY)
    {
        anim.SetBool("IsRunning", moveSpeed > 0.1f); // Run animation
        anim.SetFloat("VelY", velocityY); // Vertical velocity for jump/fall
    }
}
