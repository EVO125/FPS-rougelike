using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovementY : MonoBehaviour
{
    // First thing to know is, we put the MainCamera inside the MouseMovementY object that uses this script
    // since we move where we are looking
    // Because otherwise, once we look up and move forward, we will start moving up(towards the sky) instead of to the front (on ground)

    [SerializeField] private float _sensitivityY = 2.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //yes, by moving mouse up and down, ull look up and down, which is the X-axis in 3D game
        float LookY = Input.GetAxis("Mouse Y");


        //We will use localEulerAngles, not Quaternion Rotations
        Vector3 directionOfRotation = transform.localEulerAngles;

        //it is inverted that is why we use subtraction

        directionOfRotation.x -= LookY * _sensitivityY;

        transform.localEulerAngles = directionOfRotation;
    }
}
