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
        State a = new State("a",false, 0);
        State b = new State("b",false, 1);
        State c = new State("c",false, 2);
        State d = new State("d",false, 3);
        State e = new State("e",true, 4);

        Reward ac = new Reward(2);
        Reward cd = new Reward(-10);
        Reward de = new Reward(-2);
        Reward ad = new Reward(-3);
        Reward ab1 = new Reward(-5);
        Reward ab2 = new Reward(-1);
        Reward ba = new Reward(1);

        Action AToC = new Action(0);
        Action AToBOrD = new Action(1);
        Action CToD = new Action(2);
        Action DToE = new Action(3);
        Action BToA = new Action(4);

        MCSystemDynamic systemDynamic = new MCSystemDynamic();
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToC, ac, c, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(c, CToD, cd, d, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(d, DToE, de, e, 1));

        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ad, d, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab1, b, 0.1f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab2, b, 0.4f));

        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(b, BToA, ba, a, 1));

        State[] states = systemDynamic.getAllStates();

        Dictionary<State, float> initialisation = new Dictionary<State, float>();
        foreach(State state in states)
        {
            initialisation.Add(state, 0);
        }

        MCPolicy policy = new MCPolicy();
        foreach (State state in states)
        {
            List<Action> actions =  systemDynamic.GetActionsForState(state);
            List<ActionProbability> actionProbabilities = new List<ActionProbability>();
            float nbActions = actions.Count;
            foreach(Action action in actions)
            {
                ActionProbability actionProbability = new ActionProbability(action, 1f / nbActions);
                actionProbabilities.Add(actionProbability);
            }
            policy.AddPolicyForState(state, actionProbabilities);
        }



         var res = MCPolicyEvaluation.FirstVisitMCEvaluate(initialisation, a, systemDynamic, policy, 1, 500000, 0);
         Debug.Log("end of test");


    }


}
