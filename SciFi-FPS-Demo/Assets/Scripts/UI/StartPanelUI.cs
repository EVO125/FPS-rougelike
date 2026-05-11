using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 开始游戏面板
/// </summary>
public class StartPanelUI : MonoBehaviour
{
    [SerializeField]
    private Button btn_Start;
    [SerializeField]
    private Button btn_Quit;

    private void Awake()
    {
        btn_Start.onClick.AddListener(()=> { Tool.ChangeScene("Demo1"); });
        btn_Quit.onClick.AddListener(() => { Application.Quit(); });
    }
}
