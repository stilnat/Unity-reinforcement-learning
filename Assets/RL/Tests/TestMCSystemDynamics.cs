using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class TestMCSystemDynamics
{
    [Test]
    public void TestTerminalStateWithDynamics()
    {
        MCSystemDynamic systemDynamic = new MCSystemDynamic();
        State s = new State(true, 0);
        Action a= new Action(0);
        Reward r = new Reward(0);
        try
        {
            systemDynamic.AddDynamic(new SingleActionStateDynamic(s, a, r, s, 1));
            Assert.IsTrue(false, "Impossible to define a terminal state with actions"); // only executed if no exceptions caught ...
        } catch(SystemDynamicsException e)
        {
            if (e.TerminalStateHasActions)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false, "Impossible to define a terminal state with actions");
            }
        }

        
        
    }


    [Test]
    public void TestGenerateTrajectory()
    {
        //The test is correct(debug testing), there's really an issue with the way trajectory are generated.
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

        Assert.IsTrue(averageC < 0.67 && averageC > 0.65, "state C should appear with frequency between 0.65 and 0.67, it appeared in " + averageC + " trajectories.");

    }

}
