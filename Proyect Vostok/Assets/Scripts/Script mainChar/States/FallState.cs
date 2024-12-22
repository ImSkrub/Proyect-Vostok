using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : MonoBehaviour,IState
{
    private PlayerController player;

    public FallState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entered Fall State");
        player.anim.SetBool("Fall", true); // Start fall animation
    }

    public void Exit()
    {
        Debug.Log("Exited Fall State");
        player.anim.SetBool("Fall", false); // Stop fall animation
    }

    public void UpdateState()
    {
        // Allow horizontal movement while falling
        player.Run();

        // Transition to idle if grounded
        if (player.IsGrounded())
        {
            player.StateMachine.TransitionTo(player.StateMachine.idleState);
        }

        // Wall slide logic
        if (player.IsWalled() && !player.IsGrounded())
        {
            player.StateMachine.TransitionTo(player.StateMachine.wallJumpState);
        }
    }
}
