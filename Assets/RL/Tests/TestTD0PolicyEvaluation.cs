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
        State stateA = states.Find(x => x.Name == "a");
        State stateB = states.Find(x => x.Name == "b");
        initialisation[stateA] = 12f;

        MCPolicy policy = systemDynamic.GenerateRandomPolicy();

        var res = TD0PolicyEvaluation.TD0Evaluate(initialisation, stateA, systemDynamic, policy, 1, 0.005f, 20000);
        Assert.IsTrue(res[stateA]._stateValue <= -8.5 && res[stateA]._stateValue >= -8.8, "state 'a' value is precisely -8.6, the result should be between -8.5 and -8.8. " +
           "value found is " + res[stateA]._stateValue);
        Assert.IsTrue(res[stateB]._stateValue <= -7.5 && res[stateB]._stateValue >= -7.8, "state 'b' value is precisely -7.6, the result should be between -7.5 and -7.8. " +
   "value found is " + res[stateB]._stateValue);

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

        var res = TD0PolicyEvaluation.TD0Evaluate(initialisation, stateA, systemDynamic, policy, 1, 0.002f, 200000);
        Assert.IsTrue(res[stateA]._stateValue <= -2.3 && res[stateA]._stateValue >= -2.7, "state 'a' value is precisely -2.475, the result should be between -2.3 and -2.7. " +
            "value found is " + res[stateA]._stateValue );
        Assert.IsTrue(res[stateB]._stateValue <= -5.3 && res[stateB]._stateValue >= -5.7, "state 'b' value is precisely -5.5, the result should be between -5.3 and -5.7." + 
            "value found is " + res[stateB]._stateValue);
        Assert.IsTrue(res[stateC]._stateValue <= -1.9 && res[stateC]._stateValue >= -2.1, "state 'c' value is precisely -2, the result should be between -1.9 and -2.1." +
            "value found is " + res[stateC]._stateValue);
    }
}
