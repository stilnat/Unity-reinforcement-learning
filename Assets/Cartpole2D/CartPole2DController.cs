using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartPole2DController : MonoBehaviour
{
    ArticulationBody _Rigidbody;
    public float _Thrust = 3000f;
    public int _nFrame;
    public int _updateCount;


    // Start is called before the first frame update
    void Start()
    {
        _Rigidbody = GetComponent<ArticulationBody>();
        _updateCount = 0;
    }

    void FixedUpdate()
    {
        if (_updateCount % _nFrame == 0)
        {
            if (Input.GetKey("up"))
            {
                _Rigidbody.AddForce(transform.forward * _Thrust);
            }

            if (Input.GetKey("down"))
            {
                //Apply a force to this Rigidbody in direction of this GameObjects up axis
                if (_updateCount % _nFrame == 0) _Rigidbody.AddForce(-transform.forward * _Thrust);
            }
        }
        _updateCount += 1;
    }
}
