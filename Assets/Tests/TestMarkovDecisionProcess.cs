using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestMarkovDecisionProcess
{
    // TODO : Verify if the policy returned is the correct one. There might be multiple correct optimal policies.
    [Test]
    public void TestCreationMarkovDecisionProcessRandomPolicy()
    {
        SystemDynamic systemDynamic = new SystemDynamic(TestHelpers.CreateSixteenStateGridWorldDynamicsListSingleDynamics());
        Reward reward = new Reward(new float[] { -1, 0 });
        MarkovDecisionProcess mdp = new MarkovDecisionProcess(systemDynamic, reward, 1);
        mdp.FindOptimalPolicy(new float[systemDynamic.getStateNumber()]);


        // Change this line to check if the policy returned is the correct one.
        Assert.IsTrue(false);
    }
}
