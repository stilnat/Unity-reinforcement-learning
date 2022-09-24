using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestTDLambdaPolicyEvaluation
{
    
    [Test]
    public void TestEvaluationTDLambdaOnRandomWalk()
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

        var res = TDLambdaPolicyEvaluation.TDLambdaEvaluate(initialisation, initialState, systemDynamic, policy, 1f, 0.9f, 0.005f, 20000);

    }



    // The return of TDLambdaEvaluate is stochastic, it's then expected that the test may fail occasionnally
    [Test]
    public void TestEvaluationTDLambdaOnSimpleDynamics()
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

        var res = TDLambdaPolicyEvaluation.TDLambdaEvaluate(initialisation, stateA, systemDynamic, policy, 1f, 0.7f, 0.002f,20000);
        Assert.IsTrue(res[stateA]._stateValue <= -8.5 && res[stateA]._stateValue >= -8.8, "state 'a' value is precisely -8.6, the result should be between -8.5 and -8.8. " +
           "value found is " + res[stateA]._stateValue + " with trace decay = 0.7 ");
        Assert.IsTrue(res[stateB]._stateValue <= -7.5 && res[stateB]._stateValue >= -7.8, "state 'b' value is precisely -7.6, the result should be between -7.5 and -7.8. " +
   "value found is " + res[stateB]._stateValue + " with trace decay = 0.7 ");

        // lambda = 0.2
        var res2 = TDLambdaPolicyEvaluation.TDLambdaEvaluate(initialisation, stateA, systemDynamic, policy, 1f, 0.2f, 0.002f, 20000);

        Assert.IsTrue(res[stateA]._stateValue <= -8.5 && res[stateA]._stateValue >= -8.8, "state 'a' value is precisely -8.6, the result should be between -8.5 and -8.8. " +
           "value found is " + res[stateA]._stateValue + " with trace decay = 0.2 ");
        Assert.IsTrue(res[stateB]._stateValue <= -7.5 && res[stateB]._stateValue >= -7.8, "state 'b' value is precisely -7.6, the result should be between -7.5 and -7.8. " +
   "value found is " + res[stateB]._stateValue + " with trace decay = 0.2 ");


    }
}
