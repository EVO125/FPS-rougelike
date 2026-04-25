using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    Patrol,
    Chase,
    React,
    Attack
}
[Serializable]
public class Parameter 
{
    public int health;
    public float movespeed;
    public float chasespeed;
    public float idleTime;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;
}
public class Fsm : MonoBehaviour
{
    public Animator animator;
    private IState currentstate;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    public Parameter parameter;

    private void Start()
    {
        //初始往状态机里新建两个字典
        states.Add(StateType.Idle, new IdleState(this));
        //states.Add(StateType.Patrol, new PatrolState(this));
        Transititionstate(StateType.Idle);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        currentstate.OnUpdate();
    }

    private void Transititionstate(StateType type) 
    {
        if (currentstate!=null) currentstate.OnExit();
        currentstate = states[type];
        currentstate.OnEnter();
    }
    public void FlipTo(Transform target) 
    {
        if (target != null) 
        {
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.position.x) 
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
