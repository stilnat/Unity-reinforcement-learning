using System.Collections;
using System;

public class StateValue : IComparable
{
    public StateValue(float stateValue)
    {
        _stateValue = stateValue;
        _numberOfVisit = 0;
        _eligibilityTrace = 0;
    }

    public StateValue(float stateValue, int numberOfVisit)
    {
        _stateValue = stateValue;
        _numberOfVisit = numberOfVisit;
        _eligibilityTrace = 0;
    }

    public int CompareTo(object o)
    {
        var statevalueObject = o as StateValue;
        if (_stateValue < statevalueObject._stateValue) return -1;
        else if (_stateValue > statevalueObject._stateValue) return 1;
        else return 0;
    }

    public float _eligibilityTrace;
    public float _stateValue;
    public int _numberOfVisit;


}


