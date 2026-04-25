using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private Fsm manager;
    private Parameter parameter;
    private float timer;

    public IdleState(Fsm manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        manager.animator.Play("Idle");
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
