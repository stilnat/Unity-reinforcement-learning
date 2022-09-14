using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class TestMCSystemDynamics
{
    [Test]
    public void TestEvaluationTD0OnSimpleDynamics()
    {

        MCSystemDynamic systemDynamic = TestHelper.GenerateSimpleDynamicWithLoop();

        List<State> states = systemDynamic.getAllStates();
        State stateA = states.Find(x => x.Name == "a");
        MCPolicy policy = systemDynamic.GenerateRandomPolicy();

        for(int i =0; i < 100000; i++)
        {
           systemDynamic.GenerateTrajectory(stateA, policy);

        }
    }

}
