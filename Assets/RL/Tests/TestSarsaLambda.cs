using System;
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
        float eligibilityDecay = 0.9f;
        float learningRate = 0.1f;
        float epsilon = 0.25f;
        float nbIterations = 50;

        var res = SarsaLambda.SarsaLambdaEvaluate(initialisation, initialState, systemDynamic, policy, discount, eligibilityDecay, learningRate, epsilon, nbIterations);

        MCPolicy policyResult = res.Item1;
        policyResult.Name = "policyResult";
        MCPolicy policyTargetMiddleGoRight = TargetPolicyforRandomWalk(true, systemDynamic, n, epsilon);
        policyTargetMiddleGoRight.Name = "policyTargetMiddleGoRight";
        MCPolicy policyTargetMiddleGoLeft = TargetPolicyforRandomWalk(false, systemDynamic, n, epsilon);
        policyTargetMiddleGoLeft.Name = "policyTargetMiddleGoLeft";

        if (!policyTargetMiddleGoLeft.Equals(policyResult) && !policyTargetMiddleGoRight.Equals(policyResult))
        {
                Assert.IsTrue(false, policyTargetMiddleGoRight.FindDifferencesWithToString(policyResult) + "\n" + policyTargetMiddleGoLeft.FindDifferencesWithToString(policyResult));
        }
    }

    /// <summary>
    /// Generate an optimal policy for the random walk.
    /// </summary>
    /// <param name="middleGoRight">Decides if the state in the middle should have action right or left.</param>
    /// <param name="systemDynamic"> The dynamic of the system.</param>
    /// <param name="n">The number of states in the random walk.</param>
    /// <param name="epsilon">The epsilon of the epsilon-greedy algorithm.</param>
    /// <returns></returns>
    private MCPolicy TargetPolicyforRandomWalk(bool middleGoRight, MCSystemDynamic systemDynamic, int n, float epsilon)
    {
        MCPolicy policyTarget = new MCPolicy();
        List<State> states = systemDynamic.getAllStates();
        foreach (State state in states)
        {
            if (!state.IsTerminal)
            {
                if (int.Parse(state.Name) < n / 2) // If state is on the left of the middle state, go left in priority.
                {
                    List<ActionProbability> ap = new List<ActionProbability>();
                    var actions = systemDynamic.GetActionsForState(state);
                    foreach (Action action in actions)
                    {
                        if (action.Name == "right")
                        {
                            ap.Add(new ActionProbability(action, epsilon / 2));
                        }
                        else
                        {
                            ap.Add(new ActionProbability(action, 1 - epsilon / 2));
                        }
                    }

                    policyTarget.AddPolicyForState(state, ap);

                }

                if (int.Parse(state.Name) > n / 2) // If state is on the right of the middle state, go right in priority.
                {
                    List<ActionProbability> ap = new List<ActionProbability>();
                    var actions = systemDynamic.GetActionsForState(state);
                    foreach (Action action in actions)
                    {
                        if (action.Name == "left")
                        {
                            ap.Add(new ActionProbability(action, epsilon / 2));
                        }
                        else
                        {
                            ap.Add(new ActionProbability(action, 1 - epsilon / 2));
                        }
                    }

                    policyTarget.AddPolicyForState(state, ap);

                }

                if (int.Parse(state.Name) == n / 2) // If it's the middle state, choose according to parameter middleGoRight.
                {
                    List<ActionProbability> ap = new List<ActionProbability>();
                    var actions = systemDynamic.GetActionsForState(state);
                    foreach (Action action in actions)
                    {
                        if (action.Name == "left" && middleGoRight)
                        {
                            ap.Add(new ActionProbability(action, epsilon / 2));
                        }
                        else if(action.Name == "left" && !middleGoRight)
                        {
                            ap.Add(new ActionProbability(action, 1 - epsilon / 2));
                        }
                        else if (action.Name == "right" && middleGoRight)
                        {
                            ap.Add(new ActionProbability(action, 1 - epsilon / 2));
                        }
                        else
                        {
                            ap.Add(new ActionProbability(action, epsilon / 2));
                        }

                    }

                    policyTarget.AddPolicyForState(state, ap);

                }
            }
        }

        return policyTarget;
    }
}
