using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing a Discrete finite Markov decision process with a deterministic policy.
/// The mdp contains a finite number of states, a finite number of actions and a finite number of rewards.
/// The dynamics can be stochastics.
/// </summary>
public class MarkovDecisionProcess
{
    // TODO : Should these attributes be private ?
    public SystemDynamic _systemDynamic;
    public Reward _reward;
    public Policy _policy;

    private float _discount;

    public float Discount { 
        get { return _discount; }
        set 
        {
            if (value >= 0 && value <= 1)
            {
                _discount = value;
            }
            else throw new System.Exception("The discount value should be between 0 and 1 included.");
        }
    }

    /// <summary>
    /// Initialise the MDP with every elements defined by the user explicitly.
    /// </summary>
    public MarkovDecisionProcess(SystemDynamic systemDynamic, Reward reward, Policy policy, float discount)
    {
        _systemDynamic = systemDynamic;
        _reward = reward;
        _policy = policy;
        _discount = discount;
    }

    /// <summary>
    /// Initialise the MDP with a random policy. 
    /// </summary>
    public MarkovDecisionProcess(SystemDynamic systemDynamic, Reward reward, float discount)
    {
        _systemDynamic = systemDynamic;
        _reward = reward;
        _discount = discount;
        MakeRandomPolicy(systemDynamic);
    }

    /// <summary>
    /// Create a policy such that for any state s, look up all possible actions a and equiprobably choose one. 
    /// </summary>
    private void MakeRandomPolicy(SystemDynamic systemDynamic)
    {
        float[,] randomPolicyMatrix = new float[systemDynamic.getStateNumber(), systemDynamic.getActionNumber()];

        int[] sums = new int[systemDynamic.getStateNumber()];

        // Check if there's a link between s and sp by any action.
        for(int sp=0; sp<systemDynamic.getStateNumber(); sp++)
        {
            for (int r = 0; r < systemDynamic.getRewardNumber(); r++)
            {
                for (int s = 0; s < systemDynamic.getStateNumber(); s++)
                {
                    for (int a = 0; a < systemDynamic.getActionNumber(); a++)
                    {
                        if (systemDynamic.GetDynamic(sp,r,s,a) != 0)
                        {
                            randomPolicyMatrix[s, a] = 1;
                        }
                    }
                }
            }
        }

        // Count the number of link between sp and s
        for(int s=0; s < systemDynamic.getStateNumber(); s++)
        {
            sums[s] = 0;
            for (int a = 0; a < systemDynamic.getActionNumber(); a++)
            {
                sums[s] += (int) randomPolicyMatrix[s, a];
            }
        }

        // If more than one action is possible from state s, divide equally the probability to choose every possible action from state s.
        for (int s = 0; s < systemDynamic.getStateNumber(); s++)
        {
            for (int a = 0; a < systemDynamic.getActionNumber(); a++)
            {
                if (sums[s] > 1)
                {
                    randomPolicyMatrix[s, a] = 1 / (float)sums[s];
                }
            }
        }
        _policy = new Policy(randomPolicyMatrix);
    }


    /// <summary>
    /// Find the Optimal policy of this Markov decision process. 
    /// </summary>
    /// <param name="valueInitialisation"> a float[] vector containing arbitrary value for initialising the state values. Terminal states must have a 
    /// initial state value worth 0.</param>
    /// <returns> Returns an optimal policy, as well as the optimal state values.</returns>
    public (Policy, float[]) FindOptimalPolicy(float[] valueInitialisation)
    {
        return PolicyIteration.Iterate(valueInitialisation, _systemDynamic, _reward, _discount, _policy);
    }
}
