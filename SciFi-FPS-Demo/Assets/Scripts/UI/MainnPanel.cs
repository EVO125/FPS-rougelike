using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainnPanel : MonoBehaviour
{
    [SerializeField]
    private Image enemyHp;
    [SerializeField]
    private Image playerHp;
    [SerializeField]
    private Text txt_Gold;
    [SerializeField]
    private GameObject hintPanel;
    [SerializeField]
    private Text txt_Bullet;
    [SerializeField]
    private Image currWeaponIcon;//当前武器的图标
    [SerializeField]
    private Text txt_enemyBornNum;//当前波数
    [SerializeField]
    private Text txt_KillEnemyNum;//击杀数量
    [SerializeField]
    private Text txt_BossBornNum;//boss还需要击杀多少
    [SerializeField]
    private Text txt_EnemyHasNum;//本轮还有多少怪
    private void Awake()
    {
        EventCenter.Instance.AddEventListener<float[]>("UpdateEnemyHp", UpdateEnemyHp);
        EventCenter.Instance.AddEventListener<float[]>("UpdatePlayerHp", UpdatePlayerHp);
        EventCenter.Instance.AddEventListener<int>("UpdateGoldNumEvent", UpdateGoldNumEvent);
        EventCenter.Instance.AddEventListener<string>("HintPanelTxt", HintPanelTxt);
        EventCenter.Instance.AddEventListener<int[]>("UpdateNumOfBullets", UpdateNumOfBullets);
        EventCenter.Instance.AddEventListener<Sprite>("UpdateWeaponIcon", UpdateWeaponIcon);
        EventCenter.Instance.AddEventListener<int[]>("UpdateCurrEnemyBornNum", UpdateCurrEnemyBornNum);
        EventCenter.Instance.AddEventListener<int[]>("UpdatePlayerKillNum", UpdatePlayerKillNum);
        EventCenter.Instance.AddEventListener<int[]>("UpdateBossInstNum", UpdateBossInstNum);
        EventCenter.Instance.AddEventListener<int[]>("UpdateEnemyHasNum", UpdateEnemyHasNum);
    }
    /// <summary>
    /// 更新当前波数
    /// </summary>
    private void UpdateCurrEnemyBornNum(int[] bornNum) 
    {
        int max = bornNum[0];
        int curr = bornNum[1];
        txt_enemyBornNum.text = $"怪物波数：{curr}/{max}";
    }
    /// <summary>
    /// 玩家击杀怪物数量
    /// </summary>
    private void UpdatePlayerKillNum(int[] nums) 
    {
        int max = nums[0];
        int curr = nums[1];
        txt_KillEnemyNum.text = $"击杀怪物数量：{curr}/{max}";
    }
    private void UpdateBossInstNum(int[] nums) 
    {
        int max = nums[0];
        int curr = nums[1];
        txt_BossBornNum.text = $"BOSS生成进度：{curr}/{max}";
    }
    /// <summary>
    /// 更新怪物血量
    /// </summary>
    private void UpdateEnemyHp(float[] hps) 
    {
        float maxHp = hps[0]; 
        float currHp = hps[1];
        enemyHp.fillAmount = currHp / maxHp;
    }

    /// <summary>
    /// 更新人物血量
    /// </summary>
    private void UpdatePlayerHp(float[] hps)
    {
        float maxHp = hps[0];
        float currHp = hps[1];
        playerHp.fillAmount = currHp / maxHp;
    }

    private void UpdateGoldNumEvent( int hasGold) 
    {
        txt_Gold.text = hasGold.ToString();
    }

    private void UpdateNumOfBullets(int[] bullets) 
    {
        int max = bullets[0];
        int curr = bullets[1];
        txt_Bullet.text = $"{curr}/{max}";
    }

    private void HintPanelTxt(string hint) 
    {
        hintPanel.SetActive(true);
        hintPanel.transform.Find("Text").GetComponent<Text>().text = hint;
        Invoke("DesHintPanel",1.0f);
    }
    private void DesHintPanel() 
    {
        hintPanel.SetActive(false);
    }

    //更新当前武器的icon
    private void UpdateWeaponIcon(Sprite sp) 
    {
        currWeaponIcon.gameObject.SetActive(true);
        currWeaponIcon.sprite = sp;
    }

    /// <summary>
    /// 刷新本轮的怪物数
    /// </summary>
    private void UpdateEnemyHasNum(int[] nums) 
    {
        int max = nums[0];
        int curr = nums[1];
        txt_EnemyHasNum.text = $"本轮还剩怪物：{curr}/{max}";
    }
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<float[]>("UpdateEnemyHp", UpdateEnemyHp);
        EventCenter.Instance.RemoveEventListener<float[]>("UpdatePlayerHp", UpdatePlayerHp);
        EventCenter.Instance.RemoveEventListener<int>("UpdateGoldNumEvent", UpdateGoldNumEvent);
        EventCenter.Instance.RemoveEventListener<string>("HintPanelTxt", HintPanelTxt);
        EventCenter.Instance.RemoveEventListener<int[]>("UpdateNumOfBullets", UpdateNumOfBullets);
        EventCenter.Instance.RemoveEventListener<Sprite>("UpdateWeaponIcon", UpdateWeaponIcon);
        EventCenter.Instance.RemoveEventListener<int[]>("UpdateCurrEnemyBornNum", UpdateCurrEnemyBornNum);
        EventCenter.Instance.RemoveEventListener<int[]>("UpdatePlayerKillNum", UpdatePlayerKillNum);
        EventCenter.Instance.RemoveEventListener<int[]>("UpdateBossInstNum", UpdateBossInstNum);
        EventCenter.Instance.RemoveEventListener<int[]>("UpdateEnemyHasNum", UpdateEnemyHasNum);
    }
}
