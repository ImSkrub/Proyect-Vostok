using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour,IState
{
    private PlayerController player;

    public IdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entered Idle State");
        //player.anim.SetBool("Idle", true); // Activar animación de Idle
    }

    public void Exit()
    {
        Debug.Log("Exited Idle State");
        //player.anim.SetBool("Idle", false); // Desactivar animación de Idle
    }

    public void UpdateState()
    {
        if (Mathf.Abs(player.moveInput.x) > 0.01f)
        {
            player.StateMachine.TransitionTo(player.StateMachine.runState);
        }

        if (Input.GetButtonDown("Jump") && player.IsGrounded())
        {
            player.StateMachine.TransitionTo(player.StateMachine.jumpState);
        }
    }
}
