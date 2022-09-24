using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestSarsaLambda
{
    [Test]
    public void TestEvaluationSarsaLambdaOnRandomWalk()
    {
        int n = 11;
        MCSystemDynamic systemDynamic = TestHelper.GenerateRandomWalk(n);

        List<StateAction> stateActions = systemDynamic.getAllStatesActions();
        List<State> states = systemDynamic.getAllStates();

        Dictionary<StateAction, float> initialisation = new Dictionary<StateAction, float>();
        foreach (StateAction stateAction in stateActions)
        {
            initialisation.Add(stateAction, 0);
        }

        MCPolicy policy = new MCPolicy();
        State initialState = states.Find(x => x.Name == (n / 2).ToString());
        float discount = 1;
        float eligibilityDecay = 0.9f;
        float learningRate = 0.2f;
        float epsilon = 0.05f;
        float nbIterations = 100;

        var res = SarsaLambda.SarsaLambdaEvaluate(initialisation, initialState, systemDynamic, policy, discount, eligibilityDecay, learningRate, epsilon, nbIterations);
        MCPolicy policyResult = res.Item1;

    }
}
