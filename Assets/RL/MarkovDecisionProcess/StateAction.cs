using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StateAction
{

    private State _s;
    private Action _a;

    public StateAction(State s, Action a)
    {
        _s = s; _a = a;
    }
}
