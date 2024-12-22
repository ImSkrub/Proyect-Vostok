using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : MonoBehaviour, IState
{
    private PlayerController player;
    public WallJumpState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        Debug.Log("Entro en grab");
        player.IsWallJumping = true;
    }
    public void Exit()
    {
        Debug.Log("Salgo grab");
    }
    public void UpdateState()
    {
        
        if (player.IsWalled() && !player.IsGrounded())
        {
            player.IsWallSliding = true;
            player.WallSlide();
        }
        else
        {
            player.IsWallSliding = false;
        }
        // If the player is no longer sliding, transition to idle or run state
        if (player.IsGrounded())
        {
            player.StateMachine.TransitionTo(player.StateMachine.idleState); // Transition to IdleState if grounded
        }
        else
        {
            player.StateMachine.TransitionTo(player.StateMachine.jumpState); // Transition to JumpState if in the air
        }
        
    }
}
