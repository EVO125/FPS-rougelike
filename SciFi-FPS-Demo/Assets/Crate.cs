using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private GameObject _destroyedCrate;
   public void DestroyCrate()
    {
        Instantiate(_destroyedCrate, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
