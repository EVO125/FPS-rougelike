using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShop : MonoBehaviour
{
    [SerializeField] private AudioSource _youWin;

    private void Start()
    {
        _youWin = GetComponent<AudioSource>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                //Get the Player script/component from the collided object
                Player player = other.GetComponent<Player>();
                if(player != null)
                {
                    player.BuyRifle();
                    _youWin.Play();
                }
            }
        }
    }
}
