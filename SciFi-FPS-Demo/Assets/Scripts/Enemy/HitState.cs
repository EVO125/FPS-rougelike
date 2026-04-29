using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : IState
{
    private Fsm manager;
    private Parameter parameter;

    public HitState(Fsm manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        manager.animator.Play("Damage");
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
