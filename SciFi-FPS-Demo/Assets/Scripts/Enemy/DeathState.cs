using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState
{
    private Fsm manager;
    private Parameter parameter;
    public DeathState(Fsm manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        manager.animator.Play("Death");
        manager.nav.isStopped = true;
        manager.nav.velocity = Vector3.zero;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
