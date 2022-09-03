using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestPolicyEvaluation
{
    [Test]
    public void EvaluationOnFourStateGridWorld()
    {

        SystemDynamic systemDynamic = new SystemDynamic(TestHelpers.CreateFourStateGridWorldDynamicsMatrix());

        float[] rewardVector = new float[2];
        rewardVector[0] = -1;
        rewardVector[1] = 0;
        Reward reward = new Reward(rewardVector);

        float[] valueInitialisation = new float[4];
        valueInitialisation[0] = 0;
        valueInitialisation[1] = 0;
        valueInitialisation[2] = 0;
        valueInitialisation[3] = 0;

        float[,] policyMatrix = new float[4, 4];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (j == 0 || j == 3)
                    policyMatrix[i, j] = 0.5f;
                else
                    policyMatrix[i, j] = 0;
            }
        }

        Policy policy = new Policy(new MatrixND(policyMatrix));

        float[] newStateValue = PolicyEvaluation.Evaluate(valueInitialisation, systemDynamic, reward, 1, policy);

        if (MathHelpers.CloseTo(newStateValue[0], 0, 0.01f) && MathHelpers.CloseTo(newStateValue[1], -2, 0.01f) &&
            MathHelpers.CloseTo(newStateValue[2], -2, 0.01f) && MathHelpers.CloseTo(newStateValue[3], -3, 0.01f))
        {
            Assert.IsTrue(true);
        }
        else
            Assert.IsTrue(false);
    }

    [Test]
    public void EvaluationOnSixteenStateGridWorld()
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

        float[] newStateValue = PolicyEvaluation.Evaluate(valueInitialisation, systemDynamic, reward, 1, policy, 100);

        float[] expectedResult = new float[] { 0, -14, -20, -22, -14, -18, -20, -20, -20, -20, -18, -14, -22, -20, -14, 0 };

        for (int i = 0; i < 16; i++)
        {
            if (!MathHelpers.CloseTo(newStateValue[i], expectedResult[i], 0.01f))
            {
                Assert.IsTrue(false);
            }
        }

        Assert.IsTrue(true);

    }
}