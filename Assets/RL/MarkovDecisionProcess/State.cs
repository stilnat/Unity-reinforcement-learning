using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MatrixND
{

    private bool _isTerminal;
    private string _name;

    public bool IsTerminal
    {
       get{ return _isTerminal; }
    }

    public string Name
    {
        get { return _name; }
    }

    public State(bool terminalState = false, params float[] values) : base(values.Length)
    {
        int i = 0;
        foreach (float value in values)
        {
            _array[i] = value;
            i++;
        }
        _isTerminal = terminalState;
    }

    public State(string name, bool terminalState = false, params float[] values) : base(values.Length)
    {
        int i = 0;
        _name = name;
        foreach (float value in values)
        {
            _array[i] = value;
            i++;
        }
        _isTerminal = terminalState;
    }

    public State(float value) : base(1)
    {
        _array[0] = value;
    }

    public State(MatrixND stateMatrix) : base(stateMatrix)
    {

    }

    public override string ToString()
    {
        if (_name == null)
        {
            return base.ToString() + "final = " + _isTerminal;
        }
        else
        {
            return _name + ": " + base.ToString() + "final = " + _isTerminal;
        }

    }

}
