using System;

public class StateAction
{

    private State _s;
    private Action _a;

    public State S {  get { return _s; } }
    public Action A { get { return _a; } }

    public StateAction(State s, Action a)
    {
        _s = s; _a = a;
    }

    public static bool operator ==(StateAction m1, StateAction m2)
    {
        return m1.Equals(m2);
    }

    public static bool operator !=(StateAction m1, StateAction m2)
    {
        return !(m1.Equals(m2));
    }

    public override bool Equals(object obj)
    {
        if (!(obj is StateAction))
        {
            return false;
        }

        StateAction stateAction = (StateAction)obj;

        return stateAction.A == _a && stateAction.S == _s ? true : false;
    }

    public override int GetHashCode()
    {
        return A.GetHashCode() + S.GetHashCode();
    }


}
