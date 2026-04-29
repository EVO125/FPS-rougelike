using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private Fsm manager;
    private Parameter parameter;

    private float timer;
    public ChaseState(Fsm manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        manager.nav.speed = parameter.chaseSpeed;
        manager.animator.Play("Chase");
    }

    public void OnExit()
    {
        manager.nav.speed = parameter.patrolSpeed;
        timer = 0.0f;
    }

    public void OnUpdate()
    {
        manager.FlipTo(parameter.target);
        if (parameter.target != null)
        {
            manager.nav.SetDestination(parameter.target.transform.position);
            //大于追赶点  就回去巡逻
            if (Vector3.Distance(parameter.target.position, manager.nav.transform.position) >= parameter.chaseDic)
            {
                manager.Transititionstate(StateType.Patrol);
            }
            if (Vector3.Distance(manager.nav.transform.position, parameter.target.transform.position) <= parameter.attackDic)
            {
                // 到达人物点  切换攻击
                manager.Transititionstate(StateType.Attack);
            }
        }
        else 
        {
            manager.Transititionstate(StateType.Idle);
        }
    }
}
