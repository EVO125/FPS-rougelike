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

    private int currDeadNum;//死亡的怪物数量

    private int thisRoundKillNum;//本轮击杀的怪物

    [SerializeField]
    private GameObject bossPrefabs;
    [SerializeField]
    private Transform bossBron;

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

        //初始化波数
        CherkInfo info = playerInfo.cherkPlayerInfos[Tool.currCherk].cherk;
        int max = info.round;
        int[] array = new int[2] { max, currEnmeyBornNum + 1 };
        EventCenter.Instance.EventTrigger<int[]>("UpdateCurrEnemyBornNum", array);

        int _max = 0;
        for (int i = 0; i < info.enemys.Length; i++)
        {
            _max += info.enemys[i];
        }
        int[] _array = new int[2] { _max, currDeadNum };
        EventCenter.Instance.EventTrigger<int[]>("UpdatePlayerKillNum", _array);

        int[] __array = new int[2] { info.victory, currDeadNum };
        EventCenter.Instance.EventTrigger<int[]>("UpdateBossInstNum", __array);

        int[] ___array = new int[2] {info.enemys[currEnmeyBornNum], info.enemys[currEnmeyBornNum] - thisRoundKillNum };
        EventCenter.Instance.EventTrigger<int[]>("UpdateEnemyHasNum", ___array);
    }

    public void UpdateEnmey() 
    {
        thisRoundKillNum = 0;
        EventCenter.Instance.EventTrigger<string>("HintPanelTxt", "Monster Refresh!!!");
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
        thisRoundKillNum += 1;
        CherkInfo info = playerInfo.cherkPlayerInfos[Tool.currCherk].cherk;
        if (thisRoundKillNum >= info.enemys[currEnmeyBornNum]) 
        {
            if (currEnmeyBornNum++ < info.round)
            {
                currEnmeyBornNum++;
                currEnmeyBornNum = Mathf.Clamp(currEnmeyBornNum, 0, info.round - 1);
                //重新刷怪
                UpdateEnmey();
            }
            else 
            {
                //没有打boss   把小怪清完了
            }
        }
        if (currDeadNum == info.victory) 
        {
            //生成boss  打完过关
            GameObject _boss = Instantiate(bossPrefabs, enemyContent);
            Fsm boss = _boss.GetComponent<Fsm>();
            boss.gameObject.SetActive(true);
            boss.transform.position = bossBron.position;
            boss.parameter.patrolPoints = RangePatrolPoint();
        }
        int max = 0;
        for (int i = 0; i < info.enemys.Length; i++)
        {
            max += info.enemys[i];
        }
        int[] array = new int[2] { max , currDeadNum }; 
        EventCenter.Instance.EventTrigger<int[]>("UpdatePlayerKillNum", array);
        int[] _array = new int[2] { info.victory, currDeadNum };
        EventCenter.Instance.EventTrigger<int[]>("UpdateBossInstNum", _array);
        Debug.Log($"currEnmeyBornNum__{currEnmeyBornNum}");
        Debug.Log($"thisRoundKillNum__{thisRoundKillNum}");
        int[] ___array = new int[2] { info.enemys[currEnmeyBornNum], info.enemys[currEnmeyBornNum] - thisRoundKillNum };
        EventCenter.Instance.EventTrigger<int[]>("UpdateEnemyHasNum", ___array);

        int[] __array = new int[2] { info.round, currEnmeyBornNum + 1 };
        EventCenter.Instance.EventTrigger<int[]>("UpdateCurrEnemyBornNum", __array);

        EventCenter.Instance.EventTrigger<int>("PlayerGetBuff", currDeadNum);
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("UpdateCurrDeadNum", UpdateCurrDeadNum);
    }
}
