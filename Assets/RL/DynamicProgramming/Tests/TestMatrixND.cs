using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMatrixND : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MatrixND matrixND = new MatrixND(2,2,2,2,2);
        MatrixND matrixND2 = new MatrixND(2, 2, 2, 2, 2);
        int n = 0;
        for(int i=0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        for (int m = 0; m < 2; m++)
                        {
                            matrixND.Set(n, i, j, k, l, m);
                            matrixND2.Set(n, i, j, k, l, m);
                            n++;
                        }
                    }
                }
            }
        }

        float res;
        n = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        for (int m = 0; m < 2; m++)
                        {
                            res =matrixND.Get(i, j, k, l, m);
                            n++;
                        }
                    }
                }
            }
        }

        Debug.Log(matrixND.ToVectorString());

        if(matrixND == matrixND2)
        {
            Debug.Log("same matrices");
        }
        else
        {
            Debug.Log("not the same matrices");
        }

        
    }

}
