using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class TestTD0PolicyEvaluation
{
    [Test]
    public void TestEvaluationTD0OnSimpleDynamics()
    {
        MCSystemDynamic systemDynamic = TestHelper.GenerateSimpleDynamicWithLoop();

        List<State> states = systemDynamic.getAllStates();

        Dictionary<State, float> initialisation = new Dictionary<State, float>();
        foreach (State state in states)
        {
            initialisation.Add(state, 0);
        }
        State firstState = states.Find(x => x.Name == "a");
        initialisation[firstState] = 12f;

        MCPolicy policy = systemDynamic.GenerateRandomPolicy();

        var res = TD0PolicyEvaluation.TD0Evaluate(initialisation, firstState, systemDynamic, policy, 1, 0.005f, 200000);
    }

    [Test]
    public void TestEvaluationTD0OnSimpleDynamics2()
    {

        MCSystemDynamic systemDynamic = TestHelper.GenerateSimpleDynamic();


        List<State> states = systemDynamic.getAllStates();

        Dictionary<State, float> initialisation = new Dictionary<State, float>();
        foreach (State state in states)
        {
            initialisation.Add(state, 0);
        }

        MCPolicy policy = systemDynamic.GenerateRandomPolicy();
        State stateA = states.Find(x => x.Name == "a");
        State stateB = states.Find(x => x.Name == "b");
        State stateC = states.Find(x => x.Name == "c");

        var res = TD0PolicyEvaluation.TD0Evaluate(initialisation, stateA, systemDynamic, policy, 1, 0.001f, 100000);
        Assert.IsTrue(res[stateA]._stateValue <= -2.4 && res[stateA]._stateValue >= -2.6, "state 'a' value is precisely -2.475, the result should be between -2.4 and -2.6. " +
            "value found is " + res[stateA]._stateValue );
        Assert.IsTrue(res[stateB]._stateValue <= -5.4 && res[stateB]._stateValue >= -5.6, "state 'b' value is precisely -5.5, the result should be between -5.4 and -5.6." + 
            "value found is " + res[stateB]._stateValue);
        Assert.IsTrue(res[stateC]._stateValue <= -1.9 && res[stateC]._stateValue >= -2.1, "state 'c' value is precisely -2, the result should be between -1.9 and -2.1." +
            "value found is " + res[stateC]._stateValue);
    }
}
