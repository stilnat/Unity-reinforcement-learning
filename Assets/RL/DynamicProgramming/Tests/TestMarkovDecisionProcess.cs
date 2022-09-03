using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMarkovDecisionProcess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestCreationMarkovDecisionProcessRandomPolicy(0);
    }

    // TODO : Verify if the policy returned is the correct one. There might be multiple correct optimal policies.
    public bool TestCreationMarkovDecisionProcessRandomPolicy(int verbose = 0)
    {
        SystemDynamic systemDynamic = new SystemDynamic(TestHelpers.CreateSixteenStateGridWorldDynamicsListSingleDynamics());
        Reward reward = new Reward(new float[] { -1, 0 });
        MarkovDecisionProcess mdp = new MarkovDecisionProcess(systemDynamic, reward, 1);
        mdp.FindOptimalPolicy(new float[systemDynamic.getStateNumber()]);

        if(verbose == 1)
        {
            mdp._policy.Print();
        }
        
        return false; // Change this line to check if the policy returned is the correct one.
    }



}
