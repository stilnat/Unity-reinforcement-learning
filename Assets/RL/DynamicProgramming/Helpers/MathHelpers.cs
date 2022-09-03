using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelpers
{
    public static bool CloseTo(float x, float y, float epsilon)
    {
        return Mathf.Abs(x - y) <= epsilon ? true : false;
    }


    public static void InitialiseToZero(float[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = 0;
            }
        }
    }

}
