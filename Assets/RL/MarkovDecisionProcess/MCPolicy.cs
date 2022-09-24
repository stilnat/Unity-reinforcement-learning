using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// A class to represent a policy. This class explicitly defines the probability to pick up an action in any given state.
/// </summary>
public class MCPolicy
{
    /// <summary>
    /// _stateActionDic contains for each state added, a list of potential actions and their respective probability to be picked up.
    /// </summary>
    private Dictionary<State, Dictionary<Action, float>> _stateActionDic;

    public MCPolicy()
    {
        _stateActionDic = new Dictionary<State, Dictionary<Action, float>>();
    }

   /// <summary>
   /// Adds a particular policy for a given state.
   /// </summary>
   /// <param name="s">The state to define a policy for.</param>
   /// <param name="actionProbabilities"> A list of action and the associated probability they have to be picked up by the policy.</param>
    public void AddPolicyForState(State s, List<ActionProbability> actionProbabilities)
    {
        float sum = 0; //Check if the sum of probabilities for picking up an action amount to one.
        foreach(ActionProbability actionProbability in actionProbabilities)
        {
            if (!_stateActionDic.ContainsKey(s))
            {
                _stateActionDic.Add(s, new Dictionary<Action, float>());
            }
            _stateActionDic[s].Add(actionProbability.A, actionProbability.P);

        sum += actionProbability.P;
        }
        if(sum != 1)
        {
            throw new System.Exception("The sum of probabilities is " + sum + " instead of 1.");
        }

    }

    /// <summary>
    /// compute the probabilities of taking any action in state s according to the policy.
    /// </summary>
    /// <param name="s"></param>
    /// <returns> A list of actions and their probabilities to be selected in state s following this policy.</returns>
    public Dictionary<Action, float> GetPolicyForState(State s)
    {
        try
        {
            return _stateActionDic[s];
        }
        catch (KeyNotFoundException e)
        {
            throw e;
        }
       
    }

    /// <summary>
    /// Using the current policy, choose an action when in state S.
    /// </summary>
    /// <returns> Return the chosen action according to the policy.</returns>
    public Action ChooseAction(State s)
    {
            Dictionary<Action, float> actionProbabilities = GetPolicyForState(s);
            return actionProbabilities.RandomElementByWeight(x => x.Value).Key;
    }

    //TODO check if the sum of every action sum to one. This could easily do something wrong if some actions were defined before.
    // maybe put to 0 any other action 
    public Action ChooseActionUniform(State s, List<Action> actions)
    {
        if (_stateActionDic.ContainsKey(s)) _stateActionDic[s].Clear();
        else _stateActionDic.Add(s, new Dictionary<Action, float>());

        foreach (Action a in actions)
        {
            _stateActionDic[s].Add(a, 1f / actions.Count);
        }
        return actions.RandomElementByWeight(x => 1f / actions.Count);
    }

    public Action ChooseActionGreedy(State s, Dictionary<Action, StateActionValue> stateActionValues)
    {
        Action bestAction = stateActionValues.FirstOrDefault(x => x.Value.Equals(stateActionValues.Values.Max())).Key;

        if (_stateActionDic.ContainsKey(s)) _stateActionDic[s].Clear();
        else _stateActionDic.Add(s, new Dictionary<Action, float>());

        _stateActionDic[s].Add(bestAction, 1f);
        return bestAction;
    }

    public Action ChooseActionEpsilonGreedy(State s, Dictionary<Action,StateActionValue> stateActionValues, float epsilon)
    {
        List<bool> GreedyOrRandom = new List<bool>() {true, false};
        Func<bool, float> weights = x => { if (x == true) return epsilon; else return 1 - epsilon; };
        bool isRandom = GreedyOrRandom.RandomElementByWeight(weights);
        Action randomAction = ChooseActionUniform(s, stateActionValues.Keys.ToList());
        Action greedyAction = ChooseActionGreedy(s, stateActionValues);

        if (_stateActionDic.ContainsKey(s)) _stateActionDic[s].Clear();
        else _stateActionDic.Add(s, new Dictionary<Action, float>());

        foreach (Action a in stateActionValues.Keys)
        {
            _stateActionDic[s].Add(a,epsilon / stateActionValues.Keys.Count);
        }
        _stateActionDic[s][greedyAction] += 1 - epsilon;

        return isRandom ? randomAction : greedyAction; 
    }

   


}
