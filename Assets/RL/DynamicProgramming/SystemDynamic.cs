using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// System dynamic is a class representing the model dynamics of a finite, discrete markov reward process. 
/// </summary>
public class SystemDynamic
{

    /// <summary>
    ///  A single dynamic is defined this way : 
    ///  s is the state we're in, 
    ///  a is the action we take, 
    ///  r is the reward we get, 
    ///  sp is the state we end up in,
    ///  p is the probability of getting into state sp with reward r, when taking action a in state s.
    /// </summary>
    public class SingleDynamic
    {

        public SingleDynamic(int s, int a, int r, int sp, float p)
        {
            _s = s;
            _a = a;
            _r = r;
            _sp = sp;
            _p = p;
        }

        private int _s;
        private int _a;
        private int _r;
        private int _sp;
        private float _p;

        public int S{get { return _s; }}
        public int A { get { return _a; } }
        public int R { get { return _r; } }
        public int SP { get { return _sp; } }
        public float P { get { return _p; } }
    }

    /// <summary>
    /// A matrix containing every single dynamic of the system.
    /// </summary>
    private MatrixND _dynamicMatrix;

    /// <summary>
    /// Set a singular dynamic in the system
    /// </summary>
    public void SetDynamic(int sp, int r, int s, int a, float p)
    {
        _dynamicMatrix.Set(p,sp,r,s,a);
        if (!VerifyDynamics(_dynamicMatrix))
        {
            throw new System.Exception("The dynamic matrix probabilities are not all summing up to 1");
        }
    }

    /// <summary>
    /// Get the probability of getting reward r and ending in state sp, by choosing action a in state s.
    /// </summary>
    public float GetDynamic(int sp, int r, int s, int a)
    {
        return _dynamicMatrix.Get(sp, r, s, a);
    }


    public SystemDynamic(MatrixND dynamicMatrix)
    {
        if (VerifyDynamics(dynamicMatrix))
        {
            _dynamicMatrix = dynamicMatrix;
        }
        else throw new System.Exception("The dynamic matrix probabilities are not all summing up to 1"); 
    }

    /// <summary>
    /// Verifies if the sum of every probabilities of getting into state sp and getting reward r by choosing action a in state s amounts to one.
    /// </summary>
    private bool VerifyDynamics(MatrixND dm)
    {
        float sum = 0;
        for (int s = 0; s < dm.GetLength(2); s++)
        {
            for (int a = 0; a < dm.GetLength(3); a++)
            {

                for (int sp = 0; sp < dm.GetLength(0); sp++)
                {
                    for (int r = 0; r < dm.GetLength(1); r++)
                    {
                        sum += dm.Get(sp, r, s, a);
                    }                   
                }

                if (sum != 1) return false;
                else sum = 0;
            }
        }
        return true;
    }

    /// <summary>
    /// Initialise the dynamic matrix to any value
    /// </summary>
    public void InitialiseTo(float value )
    {
        _dynamicMatrix.InitializeTo(value);
    }

    /// <summary>
    /// Creates a System Dynamic from the list of every single dynamics in the system.
    /// </summary>
    public SystemDynamic(List<SingleDynamic> singleDynamics)
    {
        int[] dims =DefineDimensions(singleDynamics);
        _dynamicMatrix = new MatrixND(dims[0], dims[1], dims[2], dims[3]);

        _dynamicMatrix.InitializeTo(0);

        foreach (SingleDynamic sd in singleDynamics)
        {
            _dynamicMatrix.Set(sd.P, sd.SP, sd.R, sd.S, sd.A); 
        }
    }

    /// <summary>
    /// This method supposes that every state, action, and reward are labelled in order 0,1,2...
    /// Then, finding the highest label for every parameter, and adding 1, gives us the dimension we're looking for.
    /// </summary>
    /// <param name="singleDynamics"> A list of every dynamics of the system.</param>
    /// <returns></returns>
    private int[] DefineDimensions(List<SingleDynamic> singleDynamics)
    {
        int dimS = 0;
        int dimR = 0;
        int dimA = 0;

        foreach(SingleDynamic sd in singleDynamics)
        {
            dimS = Mathf.Max(dimS, sd.S);
            dimR = Mathf.Max(dimR, sd.R);
            dimA = Mathf.Max(dimA, sd.A);
        }

        return new int[] { dimS + 1, dimR + 1, dimS + 1, dimA + 1 };

    }

    public static bool AreEqualDynamics(SystemDynamic sd1, SystemDynamic sd2)
    {
        return sd1._dynamicMatrix == sd2._dynamicMatrix ? true : false;
    }

    public int getActionNumber()
    {
        return _dynamicMatrix.GetLength(3);
    }

    public int getStateNumber()
    {
        return _dynamicMatrix.GetLength(0);
    }

    public int getRewardNumber()
    {
        return _dynamicMatrix.GetLength(1);
    }

}
