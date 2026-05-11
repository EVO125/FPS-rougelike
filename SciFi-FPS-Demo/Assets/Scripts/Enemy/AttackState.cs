using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Fsm manager;
    private Parameter parameter;
    private AnimatorStateInfo info;
    private bool isAttackPlayer;//是否已经检测打击到玩家了
    private Player player;
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
        player = GameObject.FindObjectOfType<Player>();
    }

    public void OnExit()
    {
        manager.nav.isStopped = false;
        manager.nav.velocity = Vector3.zero;
        isAttackPlayer = false;
    }

    public void OnUpdate()
    {
        info = manager.animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 0.95f) 
        {
            manager.Transititionstate(StateType.Idle);
        }

        //攻击范围是否打击到人物
        if (isAttackPlayer) return;
        SectorAttack(player.transform, parameter.attackAngle, parameter.attackDic);
    }

    /// <summary>
    /// 扇形攻击逻辑判断
    /// </summary>
    /// <param name="hero">英雄坐标</param>
    /// <param name="attackAngle">攻击角度</param>
    /// <param name="attackRadius">攻击半径</param>
    void SectorAttack(Transform hero, float attackAngle, float attackRadius)
    {
        Vector3 tmpVec = hero.position - manager.transform.position;
        float cosValue = Vector3.Dot(tmpVec.normalized, manager.transform.forward);
        float realAngle = Mathf.Acos(cosValue) * Mathf.Rad2Deg;
        if (realAngle <= attackAngle / 2 && tmpVec.magnitude <= attackRadius)
        {
            //攻击
            player.Damage(parameter.attack);
            isAttackPlayer = true;
        }
    }
}
