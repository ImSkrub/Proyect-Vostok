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
    private void CheckAnimationState()
    {
        //Agregar funciones para pasar a Run y idle
        if (startedJumping)
        {
            anim.SetTrigger("Jump");
            startedJumping = false;
            return;
        }
        if (justLanded)
        {
            anim.SetTrigger("Land");
            justLanded = false;
            return ;
        }
        anim.SetFloat("Vel Y", controller.rb.velocity.y);
    }
}
