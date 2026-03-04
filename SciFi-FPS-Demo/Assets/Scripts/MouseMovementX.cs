using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovementX : MonoBehaviour
{
    [SerializeField] private float _sensitivityX = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //yes, by moving mouse right and left, ull look right and left, which is the Y-axis in 3D game
        float LookX = Input.GetAxis("Mouse X");

        //We will use localEulerAngles, not Quaternion Rotations

        Vector3 directionOfRotation = transform.localEulerAngles;

        directionOfRotation.y += LookX * _sensitivityX;
        transform.localEulerAngles = directionOfRotation;
    }
}
