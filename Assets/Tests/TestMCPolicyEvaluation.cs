using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestMCPolicyEvaluation
{
    [Test]
    public void TestSingleTerminalState()
    {
        //to implement
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

    }


}
