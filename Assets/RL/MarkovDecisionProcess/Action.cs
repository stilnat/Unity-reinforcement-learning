using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An action in a Markov decision process, represented by a matrix.
/// </summary>
public class Action : MatrixND
{
    
    private string _name;

    public Action(params float[] values) : base(values.Length)
    {
        int i = 0;
        foreach(float value in values)
        {
            _array[i] = value;
            i++;
        }
    }

    public Action(string name, params float[] values) : base(values.Length)
    {
        int i = 0;
        foreach (float value in values)
        {
            _array[i] = value;
            i++;
        }
        _name = name;
    }

    //TODO : adds equality overloading to compare name too, not just MatrixND.





}
