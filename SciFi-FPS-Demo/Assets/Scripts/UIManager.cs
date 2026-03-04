using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] public Text numOfBullets;
    [SerializeField] private GameObject _coinImage;
    [SerializeField] private Text hasCoinOrNot;
    // Start is called before the first frame update
    void Start()
    {
        numOfBullets.text = "60";
    }

    public void ShowCoinImage()
    {
        _coinImage.SetActive(true);
    }
    public void HideCoinImage()
    {
        _coinImage.SetActive(false);
    }
    public void UpdateNumOfBullets(int bullets)
    {
        numOfBullets.text = bullets.ToString();
    }


    public IEnumerator BuySuccessful()
    {
        hasCoinOrNot.text = "You bought a Rifle !";
        yield return new WaitForSeconds(2f);
        hasCoinOrNot.text = "";
    }

    public IEnumerator BuyFailed()
    {
        hasCoinOrNot.text = "You have no coins !";
        yield return new WaitForSeconds(2f);
        hasCoinOrNot.text = "";
    }
}
