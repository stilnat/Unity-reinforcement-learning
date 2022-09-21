using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : should check if the system contains a terminal state, it's necessary for generateTrajectory();

/// <summary>
/// Class describing the Dynamics of a system with a finite amount of states and actions available.
/// </summary>
public class MCSystemDynamic
{

    /// <summary> _stateActionDic keeps track of the dynamics. In a given state, 
    /// choosing a given action, it stores the probability of receiving a given reward and ending up in a given state.</summary>
    private Dictionary<StateAction, List<StateRewardProbability>> _stateActionDic;

    /// <summary> _directActionDic keeps track of every action available in a given state.  </summary>
    private Dictionary<State, HashSet<Action>> _directActionDic;

    /// <summary>
    /// A list of all terminal states of the system.
    /// </summary>
    /// TODO : replace with an hashset ? this is supposed to be a set and each state in this should be unique.
    private List<State> _finalStates;

    public MCSystemDynamic()
    {
        _stateActionDic = new Dictionary<StateAction, List<StateRewardProbability>>();
        _directActionDic = new Dictionary<State, HashSet<Action>>();
        _finalStates = new List<State>();
    }

    /// <summary>
    /// Get all the states of this system.
    /// </summary>
    /// <returns> A list of all states </returns>
    public List<State>  getAllStates()
    {
        List<State> allStates = new List<State>(_directActionDic.Keys);
        allStates.AddRange(_finalStates);
        return allStates;
    }

    /// <summary>
    /// Count how many states are in this system.
    /// </summary>
    /// <returns> The number of states of the system.</returns>
    public int StateCount()
    {
        return _directActionDic.Count;
    }

    /// <summary>
    /// Add a single dynamic to the system
    /// </summary>
    /// <param name="single">The dynamic to add.</param>
    public void AddDynamic(SingleActionStateDynamic single)
    {

        if (single.S.IsTerminal)
        {
            throw new SystemDynamicsException("can't add a new dynamic from a terminal state", true);
        }
 
        var srp = new StateRewardProbability(single.SP, single.R, single.P);
        var sa = new StateAction(single.S, single.A);
        if (_stateActionDic.ContainsKey(sa))
        {
            _stateActionDic[sa].Add(srp);
        }
        else
        {
            _stateActionDic.Add(sa, new List<StateRewardProbability>());
            _stateActionDic[sa].Add(srp);
        }

        if (_directActionDic.ContainsKey(single.S))
        {
            _directActionDic[single.S].Add(single.A);
        }
        else
        {
            _directActionDic.Add(single.S, new HashSet<Action>());
            _directActionDic[single.S].Add(single.A);
        }

        // Should be fine as long as the dynamics don't contain too much terminal states. (Contains is O(N) in computation time)
        if (single.SP.IsTerminal && !_finalStates.Contains(single.SP))
        {
            _finalStates.Add(single.SP);
        }       
    }

    /// <summary>
    /// Select the next state and action according to the dynamics
    /// </summary>
    /// <param name="s">The state to start from</param>
    /// <param name="a">The chosen action</param>
    /// <returns> The reward from taking action a in state s, and the state the system end up in</returns>
    public (State, Reward) NextStateAndReward(State s, Action a)
    {
        List<StateRewardProbability> srps = _stateActionDic[new StateAction(s, a)];
        StateRewardProbability chosen =  srps.RandomElementByWeight(x => x.P);
        return (chosen.S, chosen.R);
    }

    /// <summary>
    /// get a set of available actions for a given state
    /// </summary>
    public HashSet<Action> GetActionsForState(State s)
    {
        if (s.IsTerminal)
        {
            throw new System.Exception("state is terminal, there is no actions for a terminal state");
        }
        return _directActionDic[s];
    }

    /// <summary>
    /// Generate a full trajectory starting from state "initialState" and folowing policy "policy".
    /// </summary>
    /// <returns> A trajectory object.</returns>
    public Trajectory GenerateTrajectory(State initialState, MCPolicy policy)
    {
        Trajectory trajectory = new Trajectory(initialState);

        if (initialState.IsTerminal)
        {
            return trajectory;
        }

        State currentState = initialState;
        Action chosenAction;
        while (!currentState.IsTerminal)
        {
            chosenAction = policy.ChooseAction(currentState);
            var res =  NextStateAndReward(currentState, chosenAction);
            currentState = res.Item1;
            trajectory.AddAction(chosenAction);
            trajectory.AddReward(res.Item2);
            trajectory.AddState(currentState);
        }

        return trajectory;

    }

    /// <summary>
    /// Generate a random policy by setting the probability to take any action equiprobably, given the available actions.
    /// </summary>
    public MCPolicy GenerateRandomPolicy()
    {
        MCPolicy policy = new MCPolicy();
        foreach (State state in getAllStates())
        {
            if (!state.IsTerminal)
            {
                HashSet<Action> actions = GetActionsForState(state);
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
        return policy;
    }

}
