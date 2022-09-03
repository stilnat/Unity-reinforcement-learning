using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward 
{
    public float[] _rewardVector;

    //TODO : check if there's duplicate reward. Every value should be unique.
    public Reward(float[] rewardVector)
    {
        _rewardVector = rewardVector;
    }
}
