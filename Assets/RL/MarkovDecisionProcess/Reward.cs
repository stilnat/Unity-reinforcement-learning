using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Rewards used as 
/// </summary>
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

    public static Reward operator +(Reward r1, Reward r2)
    {
        Reward r3 = new Reward(0);
        r3.Value = r1.Value + r2.Value;
        return r3;
    }





}
