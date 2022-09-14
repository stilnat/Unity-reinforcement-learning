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
        RewardVector reward = new RewardVector(new float[] { -1, 0 });
        MarkovDecisionProcess mdp = new MarkovDecisionProcess(systemDynamic, reward, 1);
        var res = mdp.FindOptimalPolicy(new float[systemDynamic.getStateNumber()]);
        Policy bestPolicy = res.Item1;
        MatrixND matrixND = new MatrixND(16, 4);


        //there's an issue with the test policy...
        matrixND.InitializeTo(0);
        matrixND.Set(1, 0, 3);
        matrixND.Set(1, 1, 3);
        matrixND.Set(1, 2, 3);
        matrixND.Set(1, 3, 3);
        matrixND.Set(1, 4, 0);
        matrixND.Set(1, 5, 3);
        matrixND.Set(1, 6, 3);
        matrixND.Set(1, 7, 2);
        matrixND.Set(1, 8, 0);
        matrixND.Set(1, 9, 3);
        matrixND.Set(1, 10, 2);
        matrixND.Set(1, 11, 2);
        matrixND.Set(1, 12, 1);
        matrixND.Set(1, 13, 1);
        matrixND.Set(1, 14, 1);
        matrixND.Set(1, 15, 3);

        Policy testPolicy = new Policy(matrixND);

        // Change this line to check if the policy returned is the correct one.
        Assert.IsTrue(bestPolicy == testPolicy);
    }
}
