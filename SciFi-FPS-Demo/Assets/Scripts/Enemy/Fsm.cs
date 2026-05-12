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
    Attack,
    Hit,
    Death,
}
[Serializable]
public class Parameter 
{
    public int health;
    public int currHealth;
    public int Def;//防御力
    public float idleTime;
    public Transform[] patrolPoints;
    public float chaseDic;//追赶距离
    public Transform target;
    public float patrolSpeed;
    public float chaseSpeed;
    public float attackDic;//攻击距离
    public float attackAngle;//攻击半径
    public int attack;//攻击力
    public int deadGold;//死亡以后所获得的金币
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
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this)); 
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
        if (currentstate is DeathState) return;//死亡状态以后就不能切换任何状态了
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

    public void ForceCrossFade(string name, float transitionDuration, int layer = 0, float normalizedTime = float.NegativeInfinity)
    {
        animator.Update(0);
        if (animator.GetNextAnimatorStateInfo(layer).fullPathHash == 0)
        {
            animator.CrossFade(name, transitionDuration, layer, normalizedTime);
        }
        else
        {
            animator.Play(animator.GetNextAnimatorStateInfo(layer).fullPathHash, layer);
            animator.Update(0);
            animator.CrossFade(name, transitionDuration, layer, normalizedTime);
        }
    }

    public void Damage(int attack) 
    {
        if (parameter.currHealth <= 0) return;
        int cha = attack - parameter.Def;
        if (cha <= 0) return;
        int _currHealth = parameter.currHealth - cha;
        parameter.currHealth = Mathf.Clamp(_currHealth, 0, parameter.health);
        if (parameter.currHealth <= 0) 
        {
            Transititionstate(StateType.Death);
            //添加怪物死亡数   玩家增加对应的金币
            EventCenter.Instance.EventTrigger("UpdateCurrDeadNum");
            EventCenter.Instance.EventTrigger<int>("PlayerKillEnemyGetGold", parameter.deadGold);
        }
        float[] hps = new float[2] { parameter.health, parameter.currHealth };
        EventCenter.Instance.EventTrigger<float[]>("UpdateEnemyHp", hps);
        Transititionstate(StateType.Hit);
    }
}
