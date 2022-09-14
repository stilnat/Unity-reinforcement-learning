using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class TestMCSystemDynamics
{
    [Test]
    public void TestGenerateTrajectory()
    {

        MCSystemDynamic systemDynamic = TestHelper.GenerateSimpleDynamicWithLoop();

        List<State> states = systemDynamic.getAllStates();
        State stateA = states.Find(x => x.Name == "a");
        State stateB = states.Find(x => x.Name == "b");
        State stateC = states.Find(x => x.Name == "c");
        State stateD = states.Find(x => x.Name == "d");
        State stateE = states.Find(x => x.Name == "e");
        MCPolicy policy = systemDynamic.GenerateRandomPolicy();

        int nbIteration = 10000;

        int countA = 0;
        int countB = 0;
        int countC = 0;
        int countD = 0;
        int countE = 0;

        for (int i =0; i < nbIteration; i++)
        {
           Trajectory trajectory = systemDynamic.GenerateTrajectory(stateA, policy);
           Dictionary<State,int> countStateCrossed = trajectory.CountStatesCrossed();

            if (countStateCrossed.ContainsKey(stateA)) countA += countStateCrossed[stateA];
            if (countStateCrossed.ContainsKey(stateB)) countB += countStateCrossed[stateB];
            if (countStateCrossed.ContainsKey(stateC)) countC += countStateCrossed[stateC];
            if (countStateCrossed.ContainsKey(stateD)) countD += countStateCrossed[stateD];
            if (countStateCrossed.ContainsKey(stateE)) countE += countStateCrossed[stateE];
        }

        float averageA = (float)countA / (float)nbIteration;
        float averageB = (float)countB / (float)nbIteration;
        float averageC = (float)countC / (float)nbIteration;
        float averageD = (float)countD / (float)nbIteration;
        float averageE = (float)countE / (float)nbIteration;

        Assert.IsTrue(averageC < 0.51 && averageC > 0.49, "state C should appear in half of the trajectories, it appeared in " + averageC + " trajectories.");

    }

}
