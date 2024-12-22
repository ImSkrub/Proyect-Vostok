using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MonoBehaviour,IState
{
    private PlayerController player;
    public RunState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        Debug.Log("Entered Run State");
        player.anim.SetBool("Run", true); // Start running animation
    }

    public void Exit()
    {
        Debug.Log("Exited Run State");
        player.anim.SetBool("Run", false); // Stop running animation
    }
    public void UpdateState()
    {
        // Handle movement
        player.Run();
        player.Turn();


        if (Input.GetButtonDown("Jump") && player.IsGrounded())
        {
            Debug.Log("Jump input detected and player is grounded.");
            player.StateMachine.TransitionTo(player.StateMachine.jumpState);
        }

        // Check for idle condition
        if (player.moveInput.x == 0)
        {
            player.StateMachine.TransitionTo(player.StateMachine.idleState);
        }


    }
}
