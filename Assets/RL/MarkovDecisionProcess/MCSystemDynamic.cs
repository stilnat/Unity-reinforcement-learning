using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCSystemDynamic
{

    public class SingleActionStateDynamic
    {

        public SingleActionStateDynamic(State s, Action a, Reward r, State sp, float p)
        {
            _s = s;
            _a = a;
            _r = r;
            _sp = sp;
            _p = p;
        }

        private State _s;
        private Action _a;
        private Reward _r;
        private State _sp;
        private float _p;

        public State S { get { return _s; } }
        public Action A { get { return _a; } }
        public Reward R { get { return _r; } }
        public State SP { get { return _sp; } }
        public float P { get { return _p; } }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            SingleActionStateDynamic dyn = (SingleActionStateDynamic)obj;

            return _s == dyn._s && _a == dyn._a && _r == dyn._r && _sp == dyn._sp && _p == dyn._p ? true : false;

        }
    }

    private Dictionary<StateAction, List<StateRewardProbability>> _stateActionDic;
    private Dictionary<State, HashSet<Action>> _directActionDic;
    private List<State> _finalStates;

    public MCSystemDynamic()
    {
        _stateActionDic = new Dictionary<StateAction, List<StateRewardProbability>>();
        _directActionDic = new Dictionary<State, HashSet<Action>>();
        _finalStates = new List<State>();
    }

    public List<State>  getAllStates()
    {
        List<State> allStates = new List<State>(_directActionDic.Keys);
        allStates.AddRange(_finalStates);
        return allStates;
    }

    public int StateCount()
    {
        return _directActionDic.Count;
    }

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

        // Should be fine as long as the dynamics don't contain too much terminal states.
        if (single.SP.IsTerminal && !_finalStates.Contains(single.SP))
        {
            _finalStates.Add(single.SP);
        }       
    }

    /// <summary>
    /// Select the next state and action according to the dynamics
    /// </summary>
    /// <param name="s"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public (State, Reward) NextStateAndReward(State s, Action a)
    {
        List<StateRewardProbability> srps = _stateActionDic[new StateAction(s, a)];
        StateRewardProbability chosen =  srps.RandomElementByWeight(x => x.P);
        return (chosen.S, chosen.R);
    }

    public HashSet<Action> GetActionsForState(State s)
    {
        if (s.IsTerminal)
        {
            throw new System.Exception("state is terminal, there is no actions for a terminal state");
        }
        return _directActionDic[s];
    }

    public Trajectory GenerateTrajectory(State initialState, MCPolicy policy)
    {
        Trajectory trajectory = new Trajectory(initialState);

        if (initialState.IsTerminal)
        {
            trajectory.AddState(initialState);
            return trajectory;
        }

        State currentState = initialState;
        Action chosenAction;
        while (!currentState.IsTerminal)
        {
            if(trajectory._states.Count > 100)
            {
                Debug.Log("check debug");
            }
            trajectory.AddState(currentState);
            chosenAction = policy.ChooseAction(currentState);
            var res =  NextStateAndReward(currentState, chosenAction);
            currentState = res.Item1;
            trajectory.AddAction(chosenAction);
            trajectory.AddReward(res.Item2);
        }

        trajectory.AddState(currentState);

        return trajectory;

    }

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

    private  void verifyDynamics()
    {

    }


}
