using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 每个关卡的游戏管理器
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> enemyBrons = new List<Transform>();
    [SerializeField]
    List<Fsm> enemys = new List<Fsm>();
    [SerializeField]
    List<Transform> patrolPoints = new List<Transform>();
    [SerializeField]
    private PlayerInitData playerInfo;
    int currEnmeyBornNum = 0;//当前生成怪物波数
    [SerializeField]
    private Transform enemyContent;//怪物生成的父物体

    public int currDeadNum;//当前波数死亡的怪物数量

    private void Awake()
    {
        EventCenter.Instance.AddEventListener("UpdateCurrDeadNum", UpdateCurrDeadNum);
    }
    private void Start()
    {
        InitGame();
    }
    public void InitGame() 
    {
        //刷新怪物
        UpdateEnmey();
    }

    public void UpdateEnmey() 
    {
        CherkInfo info = playerInfo.cherkPlayerInfos[Tool.currCherk].cherk;
        if (currEnmeyBornNum >= info.enemys.Length) return;
        int num = info.enemys[currEnmeyBornNum];
        for (int i = 0; i < num; i++)
        {
            int enemyID = Random.Range(0, enemys.Count);
            int bron = Random.Range(0, enemyBrons.Count);
            GameObject _enemy = Instantiate(enemys[enemyID].gameObject, enemyContent);
            Fsm enemy = _enemy.GetComponent<Fsm>();
            enemy.gameObject.SetActive(true);
            enemy.transform.position = enemyBrons[bron].position;
            enemy.parameter.patrolPoints = RangePatrolPoint();
        }
    }

    /// <summary>
    /// 随机三个巡逻点
    /// </summary>
    public Transform[] RangePatrolPoint() 
    {
        int index = 0;
        List<int> indexs = new List<int>();
        while (index < 3) 
        {
            int range = Random.Range(0, patrolPoints.Count);
            if (indexs.Contains(range)) continue;
            indexs.Add(range);
            index++;
        }
        Transform[] points = new Transform[3] 
        {
            patrolPoints[indexs[0]], patrolPoints[indexs[1]], patrolPoints[indexs[2]]
        };
        return points;
    }
    private void UpdateCurrDeadNum() 
    {
        currDeadNum += 1;
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("UpdateCurrDeadNum", UpdateCurrDeadNum);
    }
}
