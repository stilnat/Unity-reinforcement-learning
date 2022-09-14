using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ActionProbability
{
    private Action _a;
    private float _p;

    public Action A { get { return _a; } }
    public float P { get { return _p; } }

    public ActionProbability(Action a, float p)
    {
        _a = a; _p = p;
    }
}
