using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    private Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController Jugador = collision.collider.GetComponent<PlayerController>();
        if (Jugador != null)
        {
            Jugador.ForcedJump();
            Animator.SetBool("PlayerToched",true);
        }
    }
   
    private void Finish()
    {
        Animator.SetBool("PlayerToched", false);
    }
}
