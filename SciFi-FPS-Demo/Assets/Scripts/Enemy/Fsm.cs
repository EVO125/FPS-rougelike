using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public float idleTime;
    public Transform[] patrolPoints;
    public float chaseDic;//追赶距离
    public Transform target;
    public float patrolSpeed;
    public float chaseSpeed;
}
public class Fsm : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    private IState currentstate;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    public Parameter parameter;
    [HideInInspector]
    public NavMeshAgent nav;
    [HideInInspector]
    public Player player;//玩家   测试代码
    private void Awake()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        //初始往状态机里新建两个字典
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        Transititionstate(StateType.Idle);

        // 测试代码
        player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        currentstate.OnUpdate();
    }

    public void Transititionstate(StateType type) 
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
