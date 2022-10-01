/// <summary>
/// An action in a Markov decision process, represented by a matrix.
/// </summary>
/// 
using UnityEngine;

[System.Serializable]
public class Action : MatrixND
{
    [SerializeField]
    private string _name;

    public string Name { get { return _name; } }

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

    public override string ToString()
    {
        if(_name == null)
        {
            return base.ToString();
        }
        else
        {
            return _name;
        }
        
    }

    //TODO : adds equality overloading to compare name too, not just MatrixND.





}
