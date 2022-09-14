using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleActionStateDynamic
{

    public SingleActionStateDynamic(State s, Action a, Reward r, State sp, float p)
    {
        _s = s;
        _a = a;
        _r = r;
        _sp = sp;
        _p = p;
    }

    private State _s;
    private Action _a;
    private Reward _r;
    private State _sp;
    private float _p;

    public State S { get { return _s; } }
    public Action A { get { return _a; } }
    public Reward R { get { return _r; } }
    public State SP { get { return _sp; } }
    public float P { get { return _p; } }


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        SingleActionStateDynamic dyn = (SingleActionStateDynamic)obj;

        return _s == dyn._s && _a == dyn._a && _r == dyn._r && _sp == dyn._sp && _p == dyn._p ? true : false;

    }
}
