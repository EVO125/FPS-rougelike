using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip _pickupSound;

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
                    player.GetCoin();
                }
                AudioSource.PlayClipAtPoint(_pickupSound, transform.position, 0.75f);
            
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
