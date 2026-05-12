using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerInitData", menuName = "Setting/Creat PlayerInitData")]
public class PlayerInitData : ScriptableObject
{
    public List<CherkPlayerInfo> cherkPlayerInfos = new List<CherkPlayerInfo>();
}

/// <summary>
/// 关卡人物信息
/// </summary>
[Serializable]
public class CherkPlayerInfo
{
    public CherkInfo cherk;//关卡
    public int initGold;//初始金币
    public int initHp;//初始血量
    public int needFindObject;//需要搜集的物品数量
    public Sprite sprite;//需要搜集的物品图标
}
/// <summary>
/// 关卡信息
/// </summary>
[Serializable]
public class CherkInfo 
{
    public int index;
    public int round;//怪物刷新波数
    public int[] enemys;//每次刷新的数量
    public int victory;//胜利条件  击杀多少怪物
}
