using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Policy
{
    /// <summary>
    /// Dimension one is for states, dimension two is for actions.
    /// </summary>
    private MatrixND _policyMatrix;

    /// <summary>
    /// The policy pi, choosing action a knowing state s is a probability distribution, therefore the sum of pi(a|s) for all action a, must equal one.
    /// </summary>
    /// <param name="p">a two dimensional array representing the policy matrix, i.e. every probability pi(a|s) for every action a and state s. </param>
    /// <returns></returns>
    private bool VerifyPolicy(MatrixND p)
    {
        float sum = 0;
        for (int s = 0; s < p.GetLength(0); s++)
        {
            for (int a = 0; a < p.GetLength(1); a++)
            {

                sum += p.Get(s,a);
            }
            if (sum != 1) return false;
            else sum = 0;
        }
        return true;
    }

    /// <summary>
    /// Set a single probability, action knowing state is s pi(a|s).
    /// </summary>
    /// <param name="state"> The identifier of the state. </param>
    /// <param name="action"> The identifier of the action. </param>
    /// <param name="prob"> The probability, must be between 0 and 1. </param>
    public void SetStateAction(int state, int action, float prob)
    {
        if(prob > 1 || prob < 0)
        {
            throw new System.Exception("Probabilities must be between 0 and 1 included.");
        }
        _policyMatrix.Set(prob, state, action);
    }

    public float GetStateAction(int state, int action)
    {
        return _policyMatrix.Get(state, action);
    }

    public Policy(MatrixND policyMatrix)
    {
        if (VerifyPolicy(policyMatrix))
            _policyMatrix = policyMatrix;
        else throw new System.Exception("Policy matrix not well defined.");
    }

    public static bool operator ==(Policy p1, Policy p2)
    {
        return p1.Equals(p2);
    }

    public static bool operator !=(Policy p1, Policy p2)
    {
        return !p1.Equals(p2);
    }

    /// <summary>
    /// Check if every element of the policy matrices are the same.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (!(obj is Policy)) return false;

        Policy p2 = (Policy)obj;

        return _policyMatrix == p2._policyMatrix;
    }

    /// <summary>
    /// Show this policy in the log console.
    /// </summary>
    public void Print()
    {
        string line = "";

        for (int i = 0; i < _policyMatrix.GetLength(0); i++)
        {
            line = "s" + i + "  ";
            for (int j = 0; j < _policyMatrix.GetLength(1); j++)
            {
                line += _policyMatrix.Get(i,j) + " ";
            }
            Debug.Log(line);
        }
    }




}
