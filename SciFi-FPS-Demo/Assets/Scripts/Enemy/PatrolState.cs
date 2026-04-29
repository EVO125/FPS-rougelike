using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private Fsm manager;
    private Parameter parameter;
    private int currIndexPatrolIndex;

    public PatrolState(Fsm manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        manager.nav.speed = parameter.patrolSpeed;
        manager.animator.Play("Walk");
    }

    public void OnExit()
    {
        currIndexPatrolIndex++;
        if (currIndexPatrolIndex >= parameter.patrolPoints.Length) 
        {
            currIndexPatrolIndex = 0;
        }
    }

    public void OnUpdate()
    {
        manager.FlipTo(parameter.patrolPoints[currIndexPatrolIndex]);
        manager.nav.SetDestination(parameter.patrolPoints[currIndexPatrolIndex].transform.position);

        if (Vector3.Distance(manager.nav.transform.position, parameter.patrolPoints[currIndexPatrolIndex].transform.position) <= manager.nav.stoppingDistance)
        {
            // 到达目标点
            manager.Transititionstate(StateType.Idle);
        }

        //测试代码
        if (Vector3.Distance(manager.nav.transform.position, manager.player.transform.position) <= 5.0f) 
        {
            parameter.target = manager.player.transform;
            manager.Transititionstate(StateType.Chase);
        }
    }
}
