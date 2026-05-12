using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : IState
{
    private Fsm manager;
    private Parameter parameter;
    private AnimatorStateInfo info;
    public HitState(Fsm manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        manager.ForceCrossFade("Damage",0.0f);
        manager.nav.isStopped = true;
        manager.nav.velocity = Vector3.zero;
        //manager.Damage(20);//受伤测试代码
    }

    public void OnExit()
    {
        manager.nav.isStopped = false;
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
