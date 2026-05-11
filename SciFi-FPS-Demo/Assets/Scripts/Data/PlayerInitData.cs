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
    public int cherk;//关卡序号
    public int initGold;//初始金币
    public int initHp;//初始血量
    public int needFindObject;//需要搜集的物品数量
    public Sprite sprite;//需要搜集的物品图标
}
