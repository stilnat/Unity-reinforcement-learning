using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MatrixND
{

    private bool _isTerminal;

    public State(bool terminalState = false,params float[] values) : base(values.Length)
    {
        int i = 0;
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

}
