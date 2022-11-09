using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartPoleController : MonoBehaviour
{
    ArticulationBody _Rigidbody;
    public float _Thrust = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _Rigidbody = GetComponent<ArticulationBody>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey("up"))
        {
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            _Rigidbody.AddForce(transform.forward *_Thrust);
        }

        if (Input.GetKey("down"))
        {
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            _Rigidbody.AddForce(-transform.forward * _Thrust);
        }

        if (Input.GetKey("left"))
        {
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            _Rigidbody.AddForce(-transform.right * _Thrust);
        }

        if (Input.GetKey("right"))
        {
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            _Rigidbody.AddForce(transform.right * _Thrust);
        }
    }
}
