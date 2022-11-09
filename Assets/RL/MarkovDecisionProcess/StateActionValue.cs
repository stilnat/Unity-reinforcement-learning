using System;

[System.Serializable]
public class StateActionValue : IComparable
{
    public StateActionValue(float stateActionValue, int numberOfVisit)
    {
        _stateActionValue = stateActionValue;
        _numberOfVisit = numberOfVisit;
        _eligibilityTrace = 0;
    }

    public StateActionValue(float stateActionValue)
    {
        _stateActionValue = stateActionValue;
        _numberOfVisit = 0;
        _eligibilityTrace = 0;
    }

    public float _eligibilityTrace;
    public float _stateActionValue;
    public int _numberOfVisit;

    public int CompareTo(object o)
    {
        var stateActionvalueObject = o as StateActionValue;
        if (_stateActionValue < stateActionvalueObject._stateActionValue) return -1;
        else if (_stateActionValue > stateActionvalueObject._stateActionValue) return 1;
        else return 0;
    }

    public override string ToString()
    {
        return _stateActionValue.ToString();
    }
}
