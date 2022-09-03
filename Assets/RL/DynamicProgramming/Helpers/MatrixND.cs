using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixND
{
    private float[] _array;
    private int[] _dimensions;
    // an array starting with 1 and containing dn, then dn*dn-1, then dn*...*d2
    private int[] _dimensionsProduct;

    public MatrixND(params int[] dimensions)
    {
        //TODO : check if all elements of dimensions are positive.
        _dimensions = dimensions;

        _dimensionsProduct = new int[dimensions.Length];
        _dimensionsProduct[0] = 1;
        for(int i=1; i<dimensions.Length; i++) // works
        {
            _dimensionsProduct[i] = _dimensionsProduct[i-1] * dimensions[dimensions.Length-i];
        }

        int allDimensionsProduct = 1;
        for (int i = 0; i < dimensions.Length; i++) // works
        {
            allDimensionsProduct *= dimensions[i];
        }

        _array = new float[allDimensionsProduct];
    }

    public float Get(params int[] indexes)
    {
        if(indexes.Length != _dimensions.Length)
        {
            throw new System.Exception("This matrix needs " + _dimensions.Length + " parameters");
        }
        if (!IndexesAreInDimensions(indexes))
        {
            throw new System.Exception("Index out of the Matrix bounds");
        }

        int index = 0;

        for(int i=0; i < indexes.Length; i++)
        {
            index += indexes[indexes.Length-i-1] * _dimensionsProduct[i];
        }

        return _array[index];
    }

    public void InitializeTo(float value)
    {
        for(int i=0; i< _array.Length; i++)
        {
            _array[i] = value;
        }
    }

    public void Set(float value, params int[] indexes)
    {
        if (indexes.Length != _dimensions.Length)
        {
            throw new System.Exception("This matrix needs " + _dimensions.Length + " parameters");
        }
        if (!IndexesAreInDimensions(indexes))
        {
            throw new System.Exception("Index out of the Matrix bounds");
        }

        int index = 0;

        for (int i = 0; i < indexes.Length; i++)
        {
            index += indexes[indexes.Length - i - 1] * _dimensionsProduct[i];
        }

        _array[index] = value;
    }

    public bool IndexesAreInDimensions(params int[] indexes)
    {
        int dimension = 0;
        foreach(int index in indexes)
        {
            if (!IndexIsInDimensions(index, _dimensions[dimension]))
            {
                return false;
            }
            dimension++;
        }
        return true;
    }

    public bool IndexIsInDimensions(int index, int dimension)
    {
        return (index < dimension && index >= 0) ? true : false;
    }

    public string ToVectorString()
    {
        string str = "";
        foreach(float element in _array)
        {
            str += element.ToString() + " ";
        }

        return str;
    }

    public static bool operator == (MatrixND m1, MatrixND m2)
    {
        return m1.Equals(m2);
    }

    public static bool operator !=(MatrixND m1, MatrixND m2)
    {
        return !(m1.Equals(m2));
    }

    public override bool Equals(object obj)
    {
        if(!(obj is MatrixND))
        {
            return false;
        }

        MatrixND m = (MatrixND)obj;

        if(m._dimensions.Length != _dimensions.Length)
        {
            return false;
        }

        for(int i=0; i< m._dimensions.Length; i++)
        {
            if(m._dimensions[i] != _dimensions[i])
            {
                return false;
            }
        }

        for (int i = 0; i < m._array.Length; i++)
        {
            if (m._array[i] != _array[i] )
            {
                return false;
            }
        }

        return true;
    }

}
