using System.Collections;
using System;
using System.Collections.Generic;
using NUnit.Framework;

public class TestQLearning
{
    [Test]
    public void TestEvaluationQLearningOnRandomWalk()
    {

        int n = 11;
        MCSystemDynamic systemDynamic = TestHelper.GenerateRandomWalk(n);

        List<State> states = systemDynamic.getAllStates();
        List<StateAction> stateActions = systemDynamic.getAllStatesActions();


        Dictionary<StateAction, float> initialisation = new Dictionary<StateAction, float>();
        foreach (StateAction stateAction in stateActions)
        {
            initialisation.Add(stateAction, 10);
        }

        MCPolicy policy = new MCPolicy();
        State initialState = states.Find(x => x.Name == (n / 2).ToString());
        float discount = 1;
        float learningRate = 0.9f;
        float epsilon = 0.2f;
        float nbIterations = 15;

        var res = QLearning.QLearningPolicy(initialisation, initialState, systemDynamic, policy, discount, learningRate, epsilon, nbIterations);
        MCPolicy policyResult = res.Item1;
        MCPolicy policyTargetMiddleGoRight = TargetPolicyforRandomWalk(true, systemDynamic, n);
        MCPolicy policyTargetMiddleGoLeft = TargetPolicyforRandomWalk(false, systemDynamic, n);
        Assert.IsTrue((policyResult.Equals(policyTargetMiddleGoRight) || policyResult.Equals(policyTargetMiddleGoLeft)),policyResult.PolicyAsString());
    }

    /// <summary>
    /// Generate an optimal greedy policy for the random walk.
    /// </summary>
    /// <param name="middleGoRight">Decides if the state in the middle should have action right or left.</param>
    /// <param name="systemDynamic"> The dynamic of the system.</param>
    /// <param name="n">The number of states in the random walk.</param>
    private MCPolicy TargetPolicyforRandomWalk(bool middleGoRight, MCSystemDynamic systemDynamic, int n)
    {
        MCPolicy policyTarget = new MCPolicy();
        List<State> states = systemDynamic.getAllStates();
        var actionLeft = new Action("left", 0);
        var actionRight = new Action("right", 1);
        foreach (State state in states)
        {
            if (!state.IsTerminal)
            {
                if (int.Parse(state.Name) < n / 2) // If state is on the left of the middle state, go left in priority.
                {
                    List<ActionProbability> ap = new List<ActionProbability>();
                    ap.Add(new ActionProbability(actionLeft, 1f));
                    policyTarget.AddPolicyForState(state, ap);
                }

                if (int.Parse(state.Name) > n / 2) // If state is on the right of the middle state, go right in priority.
                {
                    List<ActionProbability> ap = new List<ActionProbability>();
                    ap.Add(new ActionProbability(actionRight, 1f));
                    policyTarget.AddPolicyForState(state, ap);
                }

                if (int.Parse(state.Name) == n / 2) // If it's the middle state, choose according to parameter middleGoRight.
                {
                    List<ActionProbability> ap = new List<ActionProbability>();
                    if (middleGoRight)
                    {
                        ap.Add(new ActionProbability(actionRight, 1f));
                    }
                    else
                    {
                        ap.Add(new ActionProbability(actionLeft, 1f));
                    }

                    policyTarget.AddPolicyForState(state, ap);
                }
            }
        }

        return policyTarget;
    }
}
