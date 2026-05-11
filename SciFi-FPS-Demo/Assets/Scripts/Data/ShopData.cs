using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="ShopData",menuName ="Setting/Creat ShopData")]
public class ShopData : ScriptableObject
{
    public List<GunInfo> gunInfos = new List<GunInfo>();
}

[Serializable]
public class GunInfo 
{
    public int gunId;
    public Sprite icon;//图标
    public int price;//价格
    public int attack;//攻击了
    public int bulletNum;//子弹数量
    public int bearLoad;//负重
}
