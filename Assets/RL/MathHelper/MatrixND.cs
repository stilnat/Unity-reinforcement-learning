using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatrixND
{
    [SerializeField]
    protected float[] _array;

    [SerializeField]
    protected int[] _dimensions;
   
    // an array starting with 1 and containing dn, then dn*dn-1, then dn*...*d2
    [SerializeField]
    protected int[] _dimensionsProduct;

    private int _hashcode;

    private bool _hasHashCode;


    public MatrixND(MatrixND matrix)
    {
        //TODO : check if all elements of dimensions are positive.
        _dimensions = matrix._dimensions;
        _dimensionsProduct = matrix._dimensionsProduct;
        _array = new float[matrix._array.Length];
        int i = 0;
        foreach (float value in matrix._array)
        {
            _array[i] = matrix._array[i];
            i ++;
        }
        _hasHashCode = false;
    }

    // For vectors
    public MatrixND(int dimension)
    {
        //TODO : check if all elements of dimensions are positive.
        _dimensions = new int[1] { dimension };
        InitializeDimensionsProduct(dimension);
        InitializeArray(dimension);
        _hasHashCode = false;
    }

    public MatrixND(params int[] dimensions)
    {
        //TODO : check if all elements of dimensions are positive.
        _dimensions = dimensions;
        InitializeDimensionsProduct(dimensions);
        InitializeArray(dimensions);
        _hasHashCode = false;
    }

    public MatrixND(float[,] array)
    {
        _dimensions = new int[2] { array.GetLength(0), array.GetLength(1) };

        InitializeDimensionsProduct(_dimensions);
        InitializeArray(_dimensions);

        for(int i=0; i < _dimensions[0]; i++)
        {
            for (int j = 0; j < _dimensions[1]; j++)
            {
                Set(array[i, j], i, j);
            }
        }
        _hasHashCode = false;
    }

    public MatrixND(float[,,,] array)
    {
        _dimensions = new int[4] { array.GetLength(0), array.GetLength(1), array.GetLength(2), array.GetLength(3) };

        InitializeDimensionsProduct(_dimensions);
        InitializeArray(_dimensions);

        for (int i = 0; i < _dimensions[0]; i++)
        {
            for (int j = 0; j < _dimensions[1]; j++)
            {
                for (int k = 0; k < _dimensions[2]; k++)
                {
                    for (int l = 0; l < _dimensions[3]; l++)
                    {
                        Set(array[i, j,k,l], i, j, k, l);
                    }
                }
            }
        }
        _hasHashCode = false;
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

    public override int GetHashCode()
    {
        if (_hasHashCode) { return _hashcode; }
        float hashcode = 0;
        for(int i = 0; i < _array.Length; i++)
        {
            hashcode += _array[i];
        }
        for (int i = 0; i < _array.Length; i++)
        {
            hashcode += _array[i];
        }

        if (hashcode > 0 && hashcode < 1) { hashcode += 1; }
        if (hashcode < 0 && hashcode > -1) 
        {
            hashcode -= 1;
        }
        _hashcode = (int)(hashcode *= 32767);
        _hasHashCode = true;
        return _hashcode;

    }

    public int GetLength(int dimension)
    {
        return _dimensions[dimension];
    }

    protected void InitializeDimensionsProduct(params int[] dimensions)
    {
        _dimensionsProduct = new int[dimensions.Length];
        _dimensionsProduct[0] = 1;
        for (int i = 1; i < dimensions.Length; i++) // works
        {
            _dimensionsProduct[i] = _dimensionsProduct[i - 1] * dimensions[dimensions.Length - i];
        }
    }

    protected void InitializeArray(params int[] dimensions)
    {
        int allDimensionsProduct = 1;
        for (int i = 0; i < dimensions.Length; i++) // works
        {
            allDimensionsProduct *= dimensions[i];
        }

        _array = new float[allDimensionsProduct];
    }

    public override string ToString()
    {
        string final = "";
        final += "(";
        foreach(float f in _array)
        {
            final += f.ToString() + ", ";
        }
        final = final.Remove(final.Length - 2);
        final += ")";
        return final;
    }



}
