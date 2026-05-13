using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private Button btn_ReStart;
    [SerializeField]
    private Text txt_Des;

    private void Awake()
    {
        btn_ReStart.onClick.AddListener(()=> { Tool.ChangeScene("SFGUI"); Time.timeScale = 1.0f; });
        EventCenter.Instance.AddEventListener("PlayerDeadEvent", PlayerDeadEvent);
        EventCenter.Instance.AddEventListener("GameVictory", GameVictory);
        gameObject.SetActive(false);
    }
    private void PlayerDeadEvent() 
    {
        Des("游戏失败玩家死亡!!!");
    }
    private void GameVictory() 
    {
        Des("游戏胜利!!!");
    }
    private void Des(string des) 
    {
        gameObject.SetActive(true);
        txt_Des.text = des;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("PlayerDeadEvent", PlayerDeadEvent);
        EventCenter.Instance.RemoveEventListener("GameVictory", GameVictory);
    }
}
