using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text txt_Price;
    [SerializeField]
    private Text des;
    [SerializeField]
    private Button btn_Buy;
    private GunInfo info;
    private void Awake()
    {
        btn_Buy.onClick.AddListener(()=> { EventCenter.Instance.EventTrigger<GunInfo>("BuyGunEvent",info); });
    }
    public void Init(GunInfo _info) 
    {
        info = _info;
        icon.sprite = info.icon;
        txt_Price.text = info.price.ToString();
        des.text = $"attack-{info.attack},bulletNum-{info.bulletNum},bearLoad-{info.bearLoad}";
    }
}
