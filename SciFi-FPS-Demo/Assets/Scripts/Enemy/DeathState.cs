using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState
{
    private Fsm manager;
    private Parameter parameter;
    private AnimatorStateInfo info;
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
        info = manager.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f)
        {
            GameObject.Destroy(manager.gameObject);
        }
    }
}
