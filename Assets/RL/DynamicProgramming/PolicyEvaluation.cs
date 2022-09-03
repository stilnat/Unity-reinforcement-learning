using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PolicyEvaluation
{
    /// <summary>
    /// Evaluates a policy using the simple algorithm of Sutton & Barto p97.
    /// A fixed number of iterations is used here.
    /// </summary>
    /// <param name="initialisation"></param>
    /// <returns>A float[] vector of the state values for the given policy.</returns>

    public static float[] Evaluate(float[] valueInitialisation, SystemDynamic systemDynamic, Reward reward,
        float discount, Policy policy, float nbIterations = 50, int verbose = 0)
    {
       float[] newStateValues = new float[valueInitialisation.Length];
       float[] oldStateValues = valueInitialisation;

       float stateValue;

        for(int k = 0; k<nbIterations; k++)
        {
            if (verbose >= 1)
            {
                Debug.Log(oldStateValues[0]);
                Debug.Log(oldStateValues[1]);
                Debug.Log(oldStateValues[2]);
                Debug.Log(oldStateValues[3]);
                Debug.Log("***************************************************");
            }

            for (int s = 0; s < valueInitialisation.Length; s++)
            {
                stateValue = 0;
                for (int a = 0; a < systemDynamic.getActionNumber(); a++)
                {
                    for (int sp = 0; sp < valueInitialisation.Length; sp++)
                    {
                        for (int r = 0; r < reward._rewardVector.Length; r++)
                        {
                            
                            stateValue += policy.GetStateAction(s,a) * systemDynamic.GetDynamic(sp,r,s,a) * (reward._rewardVector[r] + discount * newStateValues[sp]);
                        }
                    }
                }
                newStateValues[s] = stateValue;
            }
            oldStateValues = newStateValues;
        }
        return newStateValues;
    }
}
