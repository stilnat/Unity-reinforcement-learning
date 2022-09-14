using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to represent a policy. This class explicitly defines the probability to pick up an action in any given state.
/// </summary>
public class MCPolicy
{
    /// <summary>
    /// _stateActionDic contains for each state added, a list of potential actions and their respective probability to be picked up.
    /// </summary>
    private Dictionary<State, List<ActionProbability>> _stateActionDic;

    public MCPolicy()
    {
        _stateActionDic = new Dictionary<State, List<ActionProbability>>();
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
            if (_stateActionDic.ContainsKey(s))
            {
                _stateActionDic[s].Add(actionProbability);
            }
            else
            {
                _stateActionDic.Add(s, new List<ActionProbability>());
                _stateActionDic[s].Add(actionProbability);
            }
            
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
    public List<ActionProbability> GetPolicyForState(State s)
    {
        return _stateActionDic[s];
    }

    /// <summary>
    /// Using the current policy, choose an action when in state S.
    /// </summary>
    /// <returns> Return the chosen action according to the policy.</returns>
    public Action ChooseAction(State s)
    {
        List<ActionProbability> actionProbabilities = GetPolicyForState(s);
        return actionProbabilities.RandomElementByWeight(x => x.P).A;
    }

}
