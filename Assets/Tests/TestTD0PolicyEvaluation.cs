using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class TestTD0PolicyEvaluation
{
    [Test]
    public void TestEvaluationTD0OnSimpleDynamics()
    {
        State a = new State("a", false, 0);
        State b = new State("b", false, 1);
        State c = new State("c", false, 2);
        State d = new State("d", false, 3);
        State e = new State("e", true, 4);

        Reward ac = new Reward(2);
        Reward cd = new Reward(-10);
        Reward de = new Reward(-2);
        Reward ad = new Reward(-3);
        Reward ab1 = new Reward(-5);
        Reward ab2 = new Reward(-1);
        Reward ba = new Reward(1);

        Action AToC = new Action("AToC",0);
        Action AToBOrD = new Action("AToBOrD",1);
        Action CToD = new Action("CToD",2);
        Action DToE = new Action("DToE",3);
        Action BToA = new Action("BToA",4);

        MCSystemDynamic systemDynamic = new MCSystemDynamic();
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToC, ac, c, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(c, CToD, cd, d, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(d, DToE, de, e, 1));

        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ad, d, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab1, b, 0.1f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab2, b, 0.4f));

        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(b, BToA, ba, a, 1));

        List<State> states = systemDynamic.getAllStates();

        Dictionary<State, float> initialisation = new Dictionary<State, float>();
        foreach (State state in states)
        {
            initialisation.Add(state, 0);
        }
        initialisation[a] = 12f;

        MCPolicy policy = new MCPolicy();
        foreach (State state in states)
        {
            if (!state.IsTerminal)
            {
                HashSet<Action> actions = systemDynamic.GetActionsForState(state);
                List<ActionProbability> actionProbabilities = new List<ActionProbability>();
                float nbActions = actions.Count;
                foreach (Action action in actions)
                {
                    ActionProbability actionProbability = new ActionProbability(action, 1f / nbActions);
                    actionProbabilities.Add(actionProbability);
                }
                policy.AddPolicyForState(state, actionProbabilities);
            }   
        }

        var res = TD0PolicyEvaluation.TD0Evaluate(initialisation, a, systemDynamic, policy, 1, 0.005f, 200000);
        Debug.Log("end of test");
    }

    [Test]
    public void TestEvaluationTD0OnSimpleDynamics2()
    {
        State a = new State("a", false, 0);
        State b = new State("b", false, 1);
        State c = new State("c", false, 2);
        State d = new State("e", true, 4);


        Action AToC = new Action("AToC", 0);
        Action AToBOrD = new Action("AToBOrD", 1);
        Action BToC = new Action("BToC", 2);
        Action CToD= new Action("CToD", 3);

        Reward ac = new Reward(2);
        Reward ad = new Reward(-3);
        Reward ab1 = new Reward(-5);
        Reward ab2 = new Reward(-1);
        Reward bc1 = new Reward(-1);
        Reward bc2 = new Reward(-6);
        Reward cd = new Reward(-2);



        MCSystemDynamic systemDynamic = new MCSystemDynamic();
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToC, ac, c, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab1, b, 0.05f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab2, b, 0.45f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ad, d, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(b, BToC, bc1, c, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(b, BToC, bc2, c, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(c, CToD, cd, d, 1));


        List<State> states = systemDynamic.getAllStates();

        Dictionary<State, float> initialisation = new Dictionary<State, float>();
        foreach (State state in states)
        {
            initialisation.Add(state, 0);
        }

        MCPolicy policy = new MCPolicy();
        foreach (State state in states)
        {
            if (!state.IsTerminal)
            {
                HashSet<Action> actions = systemDynamic.GetActionsForState(state);
                List<ActionProbability> actionProbabilities = new List<ActionProbability>();
                float nbActions = actions.Count;
                foreach (Action action in actions)
                {
                    ActionProbability actionProbability = new ActionProbability(action, 1f / nbActions);
                    actionProbabilities.Add(actionProbability);
                }
                policy.AddPolicyForState(state, actionProbabilities);
            }
        }

        var res = TD0PolicyEvaluation.TD0Evaluate(initialisation, a, systemDynamic, policy, 1, 0.001f, 100000);
        Assert.IsTrue(res[a]._stateValue <= -2.4 && res[a]._stateValue >= -2.6, "state 'a' value is precisely -2.475, the result should be between -2.4 and -2.6. " +
            "value found is " + res[a]._stateValue );
        Assert.IsTrue(res[b]._stateValue <= -5.4 && res[b]._stateValue >= -5.6, "state 'b' value is precisely -5.5, the result should be between -5.4 and -5.6." + 
            "value found is " + res[b]._stateValue);
        Assert.IsTrue(res[c]._stateValue <= -1.9 && res[c]._stateValue >= -2.1, "state 'c' value is precisely -2, the result should be between -1.9 and -2.1." +
            "value found is " + res[c]._stateValue);
    }
}
