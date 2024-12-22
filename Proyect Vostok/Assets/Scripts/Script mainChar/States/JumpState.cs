using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : MonoBehaviour,IState
{
    private PlayerController player;
    public JumpState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        Debug.Log("Entered Jump State");
        player.anim.SetBool("Jump", true); // Start jump animation
        player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse); // Apply jump force
    }
    public void Exit()
    {
        Debug.Log("Saliste de Jump");
        player.anim.SetBool("Jump", false); 
    }
    public void UpdateState()
    {
        player.Run();
        // Check if the player is falling
        if (player.rb.velocity.y < 0)
        {
            // Transition to falling or grounded state
            if (player.IsGrounded())
            {
                player.StateMachine.TransitionTo(player.StateMachine.idleState); // Transition to idle if grounded
            }
            else if (player.IsWalled())
            {
                player.StateMachine.TransitionTo(player.StateMachine.wallJumpState); // Transition to wall slide if on wall
            }
        }

        // Handle wall jump logic
        if (Input.GetButtonDown("Jump") && player.IsWalled())
        {
            player.StateMachine.TransitionTo(player.StateMachine.wallJumpState); // Transition to WallJumpState
        }

    }
}
