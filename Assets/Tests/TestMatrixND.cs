using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestMatrixND
{

    [Test]
    public void TestNDMatrixAreEqual()
    {
        MatrixND matrixND = new MatrixND(2, 2, 2, 2, 2);
        MatrixND matrixND2 = new MatrixND(2, 2, 2, 2, 2);
        int n = 0;
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
                            matrixND.Set(n, i, j, k, l, m);
                            matrixND2.Set(n, i, j, k, l, m);
                            n++;
                        }
                    }
                }
            }
        }

        // being defined in the same way, they should be equal.
        Assert.IsTrue(matrixND == matrixND2);

        // changing one element of the matrix should make them not equal.
        matrixND.Set(0, 1, 0, 1, 0, 0);
        Assert.IsTrue(matrixND != matrixND2);
    }
}
