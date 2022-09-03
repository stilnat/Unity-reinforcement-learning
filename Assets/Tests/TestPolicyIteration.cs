using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestPolicyIteration
{

    [Test]
    public void TestPolicyIterationOnSixteenStatesGridWorld()
    {
        SystemDynamic systemDynamic = new SystemDynamic(TestHelpers.CreateSixteenStateGridWorldDynamicsListSingleDynamics());

        float[] rewardVector = new float[2];
        rewardVector[0] = -1;
        rewardVector[1] = 0;
        Reward reward = new Reward(rewardVector);

        float[] valueInitialisation = new float[16];
        for (int i = 0; i < 16; i++)
        {
            valueInitialisation[i] = 0;
        }

        float[,] policyMatrix = new float[16, 4];

        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                policyMatrix[i, j] = 0.25f;
            }
        }

        Policy policy = new Policy(new MatrixND(policyMatrix));

        var res = PolicyIteration.Iterate(valueInitialisation, systemDynamic, reward, 1, policy, 100, 0);

        policy = res.Item1;
        float[] optimalEvaluation = res.Item2;


        float[,] m = new float[16, 4];
        MathHelpers.InitialiseToZero(m);

        m[0, 3] = 1;
        m[1, 3] = 1;
        m[2, 3] = 1;
        m[3, 3] = 1;

        m[4, 0] = 1;
        m[5, 3] = 1;
        m[6, 3] = 1;
        m[7, 2] = 1;

        m[8, 0] = 1;
        m[9, 3] = 1;
        m[10, 2] = 1;
        m[11, 2] = 1;

        m[12, 1] = 1;
        m[13, 1] = 1;
        m[14, 1] = 1;
        m[15, 3] = 1;

        Policy test = new Policy(new MatrixND(m));

        Assert.IsTrue(test == policy);
    }
}
