using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestMCPolicyEvaluation
{

    [Test]
    public void TestEvaluationMCOnRandomWalk()
    {
        int n = 11;
        MCSystemDynamic systemDynamic = TestHelper.GenerateRandomWalk(n);

        List<State> states = systemDynamic.getAllStates();

        Dictionary<State, float> initialisation = new Dictionary<State, float>();
        foreach (State state in states)
        {
            initialisation.Add(state, 0);
        }

        MCPolicy policy = systemDynamic.GenerateRandomPolicy();
        State initialState = states.Find(x => x.Name == (n / 2).ToString());

        var res = MCPolicyEvaluation.FirstVisitMCEvaluate(initialisation, initialState, systemDynamic, policy, 1, 10000);


    }

    // simple dynamics, choosing an action randomly.
    [Test]
    public void TestEvaluationOnSimpleDynamics()
    {
        MCSystemDynamic systemDynamic = TestHelper.GenerateSimpleDynamicWithLoop();

        List<State> states = systemDynamic.getAllStates();

        Dictionary<State, float> initialisation = new Dictionary<State, float>();
        foreach(State state in states)
        {
            initialisation.Add(state, 0);
        }
        State stateA = states.Find(x => x.Name == "a");

        MCPolicy policy = systemDynamic.GenerateRandomPolicy();
        var res = MCPolicyEvaluation.FirstVisitMCEvaluate(initialisation, stateA, systemDynamic, policy, 1, 50000, 0);

        Assert.IsTrue(res[stateA]._stateValue <= -8.50 && res[stateA]._stateValue >= -8.80, "state 'a' value is precisely -8.6, the result should be between -8.5 and -8.8. " +
   "value found is " + res[stateA]._stateValue);
    }


}
