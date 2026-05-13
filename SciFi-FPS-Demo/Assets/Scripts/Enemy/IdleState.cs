using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private Fsm manager;
    private Parameter parameter;
    private float idleTime;
    private float timer;
    public IdleState(Fsm manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        idleTime = parameter.idleTime;
    }

    public void OnEnter()
    {
        manager.animator.Play("Idle");
        timer = 0.0f;
    }

    public void OnExit()
    {
        timer = 0.0f;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            //攻击间隔
            if (Vector3.Distance(manager.nav.transform.position, parameter.target.transform.position) <= parameter.attackDic)
            {
                Debug.LogError($"timer___{timer}");
                // 到达人物点  切换攻击
                manager.Transititionstate(StateType.Attack);
            }
            else
            {
                //切换巡逻
                manager.Transititionstate(StateType.Patrol);
            }
        }

        //测试代码
        if (Vector3.Distance(manager.nav.transform.position, manager.player.transform.position) <= parameter.chaseRudis)
        {
            parameter.target = manager.player.transform;
            manager.Transititionstate(StateType.Chase);
            
        }
    }
}
