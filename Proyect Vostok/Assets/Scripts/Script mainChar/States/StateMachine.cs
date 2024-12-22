using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState currentState;
    public IState CurrentState => currentState;

    public IdleState idleState;
    public RunState runState;
    public JumpState jumpState;
    public WallJumpState wallJumpState;
    public FallState fallState;

    public void InitializeStates(PlayerController player)
    {
        idleState = new IdleState(player);
        runState = new RunState(player);
        jumpState = new JumpState(player);
        wallJumpState = new WallJumpState(player);
        fallState = new FallState(player);
    }

    public void Initialize(IState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void TransitionTo(IState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

    public void UpdateState()
    {
        currentState.UpdateState();
    }
}
