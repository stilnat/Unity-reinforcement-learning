using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class TestMCSystemDynamics
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

        Action AToC = new Action("AToC", 0);
        Action AToBOrD = new Action("AToBOrD", 1);
        Action CToD = new Action("CToD", 2);
        Action DToE = new Action("DToE", 3);
        Action BToA = new Action("BToA", 4);

        MCSystemDynamic systemDynamic = new MCSystemDynamic();
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToC, ac, c, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(c, CToD, cd, d, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(d, DToE, de, e, 1));

        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ad, d, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab1, b, 0.1f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab2, b, 0.4f));

        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(b, BToA, ba, a, 1));

        List<State> states = systemDynamic.getAllStates();

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

        for(int i =0; i < 100000; i++)
        {
           systemDynamic.GenerateTrajectory(a, policy);

        }



    }

}
