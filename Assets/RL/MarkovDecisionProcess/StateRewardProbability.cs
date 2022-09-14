using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StateRewardProbability
{
    private State _s;
    private Reward _r;
    private float _p;
    public State S { get { return _s; } }
    public Reward R { get { return _r; } }
    public float P { get { return _p; } }

    public StateRewardProbability(State s, Reward r, float p)
    {
        _s = s; _r = r; _p = p;
    }
}
