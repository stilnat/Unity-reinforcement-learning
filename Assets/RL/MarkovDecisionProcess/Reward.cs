using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward
{
    private float _value;

    public float Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public Reward(float reward)
    {
        _value = reward;
    }

}
