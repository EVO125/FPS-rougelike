using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerBuffData", menuName = "Setting/Creat PlayerBuffData")]
public class PlayerBuffData : ScriptableObject
{
   public List<BuffInfo> buffs = new List<BuffInfo>();
}

/// <summary>
/// Buff信息
/// </summary>
[Serializable]
public class BuffInfo
{
    public int buffID;//buffid
    public int tagetNum;//击杀怪物数量
    public float value;//提升数值
    public KeyCode keyCode;//按键
    public string des;//描述
}
