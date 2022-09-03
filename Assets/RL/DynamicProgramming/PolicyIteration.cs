using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyIteration
{
    /// <summary>
    /// Implements the algorithm of Sutton and Barto page 80. Works for finite discrete MDP.
    /// </summary>
    /// <param name="valueInitialisation"> Initialisation of the state value, can be anything, only zero for terminal states.</param>
    /// <param name="systemDynamic">The dynamics of the MDP. </param>
    /// <param name="reward"> All the achievable rewards of the MDP.</param>
    /// <param name="discount"> The discount factor of the MDP. </param>
    /// <param name="policy"> The initial policy, can be anything. </param>
    /// <param name="nbIterations">The number of iterations for the state value evaluation. </param>
    /// <param name="verbose">If verbose more than zero, debug.log some info.</param>
    /// <returns> An optimal deterministic policy for the given MDP, it's not necessarily the unique one. Returns as well as the optimal state-value.</returns>
    public static (Policy, float[]) Iterate(float[] valueInitialisation, SystemDynamic systemDynamic, Reward reward,
    float discount, Policy policy, float nbIterations = 50, int verbose = 0)
    {
       
        float actionValue;
        float maxActionValue;
        int bestAction = 0;

        bool isStable = false;

        int nbPolicyIteration = 0;

        float[] evaluation = { };

        // Iterate while the policy did not stabilize.
        while (!isStable)
        {
           
            if(verbose >= 1)
            {
                
                Debug.Log("policy iteration " + nbPolicyIteration);
                policy.Print();
            }
            nbPolicyIteration += 1;

            evaluation = PolicyEvaluation.Evaluate(valueInitialisation, systemDynamic, reward, discount, policy, nbIterations);
            isStable = true;

            // For each state s, determine the best action to take.
            for (int s = 0; s < valueInitialisation.Length; s++)
            {
                maxActionValue = float.MinValue;

                for (int a = 0; a < systemDynamic.getActionNumber(); a++)
                {
                    actionValue = 0;

                    for (int sp = 0; sp < valueInitialisation.Length; sp++)
                    {
                        for (int r = 0; r < reward._rewardVector.Length; r++)
                        {
                            actionValue += systemDynamic.GetDynamic(sp, r, s, a) * (reward._rewardVector[r] + discount * evaluation[sp]);
                        }
                    }

                    if (actionValue >= maxActionValue)
                    {
                        bestAction = a;
                        maxActionValue = actionValue;
                    }
                }

                if (verbose >= 1)
                {
                    Debug.Log("best action in state " + s + " is action " + bestAction);
                }

                // Check if the best actions were already selected.
                for (int i = 0; i < systemDynamic.getActionNumber(); i++)
                {
                    if (i == bestAction)
                    {
                        // If an action change in the new policy, then the policy is not stable yet.
                        if (policy.GetStateAction(s,i) != 1)
                        {
                            isStable = false;
                        }
                        // Set the policy to choose action bestAction with probability 1. 
                        policy.SetStateAction(s, i, 1);
                    }
                    else policy.SetStateAction(s, i, 0);
                }

               

            }
        }

        return (policy, evaluation);

        
    }
}
