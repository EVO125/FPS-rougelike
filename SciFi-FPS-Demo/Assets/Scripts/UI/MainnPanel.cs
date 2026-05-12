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
    private void Awake()
    {
        EventCenter.Instance.AddEventListener<float[]>("UpdateEnemyHp", UpdateEnemyHp);
        EventCenter.Instance.AddEventListener<float[]>("UpdatePlayerHp", UpdatePlayerHp);
        EventCenter.Instance.AddEventListener<int>("UpdateGoldNumEvent", UpdateGoldNumEvent);
        EventCenter.Instance.AddEventListener("BuySuccessful", BuySuccessful);
        EventCenter.Instance.AddEventListener("BuyFailed", BuyFailed);
        EventCenter.Instance.AddEventListener<int[]>("UpdateNumOfBullets", UpdateNumOfBullets);
        EventCenter.Instance.AddEventListener<Sprite>("UpdateWeaponIcon", UpdateWeaponIcon);
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

    private void BuySuccessful() 
    {
        hintPanel.SetActive(true);
        hintPanel.transform.Find("Text").GetComponent<Text>().text = "You bought a Rifle !";
        Invoke("DesHintPanel",1.0f);
    }
    private void BuyFailed() 
    {
        hintPanel.SetActive(true);
        hintPanel.transform.Find("Text").GetComponent<Text>().text = "You have no coins !";
        Invoke("DesHintPanel", 1.0f);
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
    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<float[]>("UpdateEnemyHp", UpdateEnemyHp);
        EventCenter.Instance.RemoveEventListener<float[]>("UpdatePlayerHp", UpdatePlayerHp);
        EventCenter.Instance.RemoveEventListener<int>("UpdateGoldNumEvent", UpdateGoldNumEvent);
        EventCenter.Instance.RemoveEventListener("BuySuccessful", BuySuccessful);
        EventCenter.Instance.RemoveEventListener("BuyFailed", BuyFailed);
        EventCenter.Instance.RemoveEventListener<int[]>("UpdateNumOfBullets", UpdateNumOfBullets);
        EventCenter.Instance.RemoveEventListener<Sprite>("UpdateWeaponIcon", UpdateWeaponIcon);
    }
}
