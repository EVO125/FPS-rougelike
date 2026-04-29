using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Fsm manager;
    private Parameter parameter;
    private AnimatorStateInfo info;
    public AttackState(Fsm manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        manager.animator.Play("Attack");
        manager.nav.isStopped = true;
        manager.nav.velocity = Vector3.zero;
    }

    public void OnExit()
    {
        manager.nav.isStopped = false;
        manager.nav.velocity = Vector3.zero;
    }

    public void OnUpdate()
    {
        info = manager.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f) 
        {
            manager.Transititionstate(StateType.Idle);
        }
    }
}
