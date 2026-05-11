using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField]
    private ShopData shopData;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject prefabs;

    [SerializeField]
    private Button btn_Close;

    private void Awake()
    {
        btn_Close.onClick.AddListener(()=> { Close(); });
        EventCenter.Instance.AddEventListener("OpenShopPanel", Open);
        int num = shopData.gunInfos.Count;
        for (int i = 0; i < num; i++)
        {
            int index = i;
            GameObject _go = Instantiate(prefabs, content);
            _go.SetActive(true);
            ShopItem item = _go.GetComponent<ShopItem>();
            item.Init(shopData.gunInfos[index]);
        }
        Close();
    }

    private void Open()
    {
        if (gameObject.activeInHierarchy) return;
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
    }
    private void Close() 
    {
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("OpenShopPanel", Open);
    }
}
