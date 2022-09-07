using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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





}
