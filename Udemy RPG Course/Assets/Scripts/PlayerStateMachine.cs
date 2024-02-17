using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }   //Value public if want to get it, private to set it. Read only basically.

    public void Initialize(PlayerState _startState)    //Runs first state.
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)      //Exits state, changes state, enters new state. 
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }


}
