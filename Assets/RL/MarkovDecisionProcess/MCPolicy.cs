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
    private string _name;

    public string Name { get { return _name; }
        set { _name = value; }
    }

    public MCPolicy()
    {
        _stateActionDic = new Dictionary<State, Dictionary<Action, float>>();
    }

    public MCPolicy(string name)
    {
        _stateActionDic = new Dictionary<State, Dictionary<Action, float>>();
        _name = name;
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


    /// <summary>
    /// Choose randomly, and with equal probability, one of the available action in state s. 
    /// </summary>
    /// <param name="s">The state the agent is in.</param>
    /// <param name="actions">The list of actions available in state s.</param>
    /// <returns>The chosen action.</returns>
    public Action ChooseActionUniform(State s, List<Action> actions)
    {
        //TODO check if the sum of every action sum to one. This could easily do something wrong if some actions were defined before.
        // maybe put to 0 any other action 
        if (_stateActionDic.ContainsKey(s)) _stateActionDic[s].Clear();
        else _stateActionDic.Add(s, new Dictionary<Action, float>());

        foreach (Action a in actions)
        {
            _stateActionDic[s].Add(a, 1f / actions.Count);
        }
        return actions.RandomElementByWeight(x => 1f / actions.Count);
    }

    /// <summary>
    /// Choose an action in state s according to the highest state-action value.
    /// </summary>
    /// <param name="s">The state the agent is in. </param>
    /// <param name="stateActionValues"> All available actions in state s, and their respective values.</param>
    /// <returns>The chosen action.</returns>
    public Action ChooseActionGreedy(State s, Dictionary<Action, StateActionValue> stateActionValues)
    {
        Action bestAction = stateActionValues.FirstOrDefault(x => x.Value.Equals(stateActionValues.Values.Max())).Key;

        if (_stateActionDic.ContainsKey(s)) _stateActionDic[s].Clear();
        else _stateActionDic.Add(s, new Dictionary<Action, float>());

        _stateActionDic[s].Add(bestAction, 1f);
        return bestAction;
    }

    /// <summary>
    /// Choose an action in state s according to the highest state-action value, and with probability epsilon, take a random action.
    /// </summary>
    /// <param name="s">The state the agent is in. </param>
    /// <param name="stateActionValues"> All available actions in state s, and their respective values.</param>
    /// <param name="epsilon"> A probability between 0 and 1 to choose a random action instead of the most valuable.
    /// <returns>The chosen action.</returns>
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

    /// <summary>
    /// Returns all differences between two policies. This includes all the missing states and actions in each policies, as well as the differences in probability 
    /// between the two policies when choosing a given action in a given state.
    /// </summary>
    /// <param name="other"> The policy to compare</param>
    /// <returns> 
    /// item1 : The Missing state actions in this Policy. 
    /// item2 : The missing state actions in the compared policy
    /// item 3 : the different probabilities for the same state actions, item3.item3 is the probability for this policy, item3.item4 for the compared policy.
    /// </returns>
    public (List<Tuple<State, Action>>, List<Tuple<State, Action>>, List<Tuple<State, Action, float, float>>) FindDifferenceWith(MCPolicy other)
    {
        List <Tuple<State,Action>> MissingStateActionInOther = new List<Tuple<State, Action>>();
        List<Tuple<State, Action, float, float>> DifferentProbabilities = new List<Tuple<State, Action, float, float>>();

        foreach (State s in _stateActionDic.Keys)
        {
            foreach (Action a in _stateActionDic[s].Keys)
            { 
                if (other._stateActionDic.ContainsKey(s))
                {
                    if (other._stateActionDic[s].ContainsKey(a))
                    {
                        float otherPolicyStateActionProbability = other._stateActionDic[s][a];
                        float thisPolicyStateActionProbability = this._stateActionDic[s][a];
                        if (otherPolicyStateActionProbability != thisPolicyStateActionProbability)
                        {
                            DifferentProbabilities.Add(new Tuple<State, Action, float, float>(s, a, thisPolicyStateActionProbability, otherPolicyStateActionProbability));
                        }
                    }
                    else
                    {
                        MissingStateActionInOther.Add(new Tuple<State, Action>(s, a));
                    }
                   
                }
                else
                {
                    MissingStateActionInOther.Add(new Tuple<State, Action>(s, a));
                }
            }
        }

        List<Tuple<State, Action>> MissingStateActionInThis = new List<Tuple<State, Action>>();

        foreach (State s in other._stateActionDic.Keys)
        {
            foreach (Action a in other._stateActionDic[s].Keys)
            {
                if (_stateActionDic.ContainsKey(s))
                {
                    if (!_stateActionDic[s].ContainsKey(a))
                    {
                        MissingStateActionInThis.Add(new Tuple<State, Action>(s, a));
                    }
                }
                else
                {
                    MissingStateActionInThis.Add(new Tuple<State, Action>(s, a));
                }
            }
        }

        return (MissingStateActionInThis, MissingStateActionInOther, DifferentProbabilities);
    }

    /// <summary>
    /// Gives a human readable version of the method FindDifferencesWith.
    /// </summary>
    /// <param name="other">The policy to compare</param>
    /// <returns></returns>
    public String FindDifferencesWithToString(MCPolicy other)
    {
        string final = "";
        var differences = FindDifferenceWith(other);
        string missingStateActionInOtherString = "the following state actions are not present in other policy " + other.ToString() +  " : \n ";
        string missingStateActionInSelfString = "the following state actions are not present in this policy " + this.ToString() +  " : \n ";
        string differentProbabilitiesString = "the following probabilities differs in this policy " + this.ToString() + " and other policy " + other.ToString() +" : \n ";

        var differentProbabilities = differences.Item3;
        var missingStateActionInSelf = differences.Item1;
        var missingStateActionInOther = differences.Item2;

        foreach (Tuple<State, Action, float, float> df in differentProbabilities)
        {
            differentProbabilitiesString += "State =  " + df.Item1.ToString() + ", Action = " + df.Item2.ToString() + ", in this = " + df.Item3.ToString()
                + ", in other = " + df.Item4.ToString() + "\n";
        }

        foreach (Tuple<State, Action> missing in missingStateActionInSelf)
        {
            missingStateActionInSelfString += "State =  " + missing.Item1.ToString() + ", Action = " + missing.Item2.ToString() + "\n";
        }

        foreach (Tuple<State, Action> missing in missingStateActionInOther)
        {
            missingStateActionInOtherString += "State =  " + missing.Item1.ToString() + ", Action = " + missing.Item2.ToString() + "\n";
        }

        if(missingStateActionInSelf.Count != 0)
        {
            final += missingStateActionInSelfString;
        }
        if (missingStateActionInOther.Count != 0)
        {
            final += missingStateActionInOtherString;
        }
        if (differentProbabilities.Count != 0)
        {
            final += differentProbabilitiesString;
        }

        return final;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is MCPolicy)) return false;

        MCPolicy other = obj as MCPolicy;

        foreach (State s in _stateActionDic.Keys)
        {
            foreach (Action a in _stateActionDic[s].Keys)
            {
                if (other._stateActionDic.ContainsKey(s) && other._stateActionDic[s].ContainsKey(a))
                {
                    float otherPolicyStateActionProbability = other._stateActionDic[s][a];
                    float thisPolicyStateActionProbability = this._stateActionDic[s][a];
                    if (otherPolicyStateActionProbability != thisPolicyStateActionProbability)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override string ToString()
    {
        if (_name == null)
        {
            return base.ToString();
        }
        else
        {
            return _name;
        }

    }




}
