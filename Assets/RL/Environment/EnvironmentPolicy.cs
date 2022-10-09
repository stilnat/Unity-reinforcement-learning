using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class EnvironmentPolicy
{
    private Dictionary<State, Dictionary<EnvironmentAction, float>> _stateActionDic;
    private string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public EnvironmentPolicy()
    {
        _stateActionDic = new Dictionary<State, Dictionary<EnvironmentAction, float>>();
    }

    public EnvironmentPolicy(string name)
    {
        _stateActionDic = new Dictionary<State, Dictionary<EnvironmentAction, float>>();
        _name = name;
    }


    /// <summary>
    /// Choose an action in state s according to the highest state-action value. This method modify this policy.
    /// </summary>
    /// <param name="s">The state the agent is in. </param>
    /// <param name="stateActionValues"> All available actions in state s, and their respective values.</param>
    /// <returns>The chosen action.</returns>
    public EnvironmentAction ChooseActionGreedyAndLearn(State s, Dictionary<EnvironmentAction, StateActionValue> stateActionValues)
    {
        EnvironmentAction bestAction = stateActionValues.FirstOrDefault(x => x.Value.Equals(stateActionValues.Values.Max())).Key;

        if (_stateActionDic.ContainsKey(s)) _stateActionDic[s].Clear();
        else _stateActionDic.Add(s, new Dictionary<EnvironmentAction, float>());

        _stateActionDic[s].Add(bestAction, 1f);
        return bestAction;
    }

    /// <summary>
    /// Choose an action in state s according to the highest state-action value. This method modify this policy.
    /// </summary>
    /// <param name="s">The state the agent is in. </param>
    /// <param name="stateActionValues"> All available actions in state s, and their respective values.</param>
    /// <returns>The chosen action.</returns>
    public static EnvironmentAction ChooseActionGreedy(State s, Dictionary<EnvironmentAction, StateActionValue> stateActionValues)
    {
        return stateActionValues.FirstOrDefault(x => x.Value.Equals(stateActionValues.Values.Max())).Key;
    }

    public EnvironmentAction ChooseActionUniformAndLearn(State s, List<EnvironmentAction> actions)
    {
        //TODO check if the sum of every action sum to one. This could easily do something wrong if some actions were defined before.
        // maybe put to 0 any other action 
        if (_stateActionDic.ContainsKey(s)) _stateActionDic[s].Clear();
        else _stateActionDic.Add(s, new Dictionary<EnvironmentAction, float>());

        foreach (EnvironmentAction a in actions)
        {
            _stateActionDic[s].Add(a, 1f / actions.Count);
        }
        return actions.RandomElementByWeight(x => 1f / actions.Count);
    }

    public static EnvironmentAction ChooseActionUniform(State s, List<EnvironmentAction> actions)
    {
        return actions.RandomElementByWeight(x => 1f / actions.Count);
    }

    /// <summary>
    /// Choose an action in state s according to the highest state-action value, and with probability epsilon, take a random action.
    /// </summary>
    /// <param name="s">The state the agent is in. </param>
    /// <param name="stateActionValues"> All available actions in state s, and their respective values.</param>
    /// <param name="epsilon"> A probability between 0 and 1 to choose a random action instead of the most valuable.
    /// <returns>The chosen action.</returns>
    public static EnvironmentAction ChooseActionEpsilonGreedy(State s, Dictionary<EnvironmentAction, StateActionValue> qValues, float epsilon)
    {
        List<bool> GreedyOrRandom = new List<bool>() { true, false };
        Func<bool, float> weights = x => { if (x == true) return epsilon; else return 1 - epsilon; };
        bool isRandom = GreedyOrRandom.RandomElementByWeight(weights);
        EnvironmentAction randomAction = ChooseActionUniform(s, qValues.Keys.ToList());
        EnvironmentAction greedyAction = ChooseActionGreedy(s, qValues);
        return isRandom ? randomAction : greedyAction;
    }

    public EnvironmentAction ChooseActionEpsilonGreedyAndLearn(State s, Dictionary<EnvironmentAction, StateActionValue> qValues, float epsilon)
    {
        List<bool> GreedyOrRandom = new List<bool>() { true, false };
        Func<bool, float> weights = x => { if (x == true) return epsilon; else return 1 - epsilon; };
        bool isRandom = GreedyOrRandom.RandomElementByWeight(weights);
        EnvironmentAction randomAction = ChooseActionUniform(s, qValues.Keys.ToList());
        EnvironmentAction greedyAction = ChooseActionGreedy(s, qValues);

        if (_stateActionDic.ContainsKey(s)) _stateActionDic[s].Clear();
        else _stateActionDic.Add(s, new Dictionary<EnvironmentAction, float>());

        foreach (EnvironmentAction a in qValues.Keys)
        {
            _stateActionDic[s].Add(a, epsilon / qValues.Keys.Count);
        }
        _stateActionDic[s][greedyAction] += 1 - epsilon;

        return isRandom ? randomAction : greedyAction;
    }



    public override bool Equals(object obj)
    {
        if (!(obj is EnvironmentPolicy)) return false;

        EnvironmentPolicy other = obj as EnvironmentPolicy;

        foreach (State s in _stateActionDic.Keys)
        {
            foreach (EnvironmentAction a in _stateActionDic[s].Keys)
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

    /// <summary>
    /// Return as a string the name of the Policy
    /// </summary>
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
