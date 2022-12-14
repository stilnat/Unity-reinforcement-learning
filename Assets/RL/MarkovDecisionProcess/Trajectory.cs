using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a trajectory should add elements in the following order always : state, action, reward.
public class Trajectory
{
    public List<State> _states;
    public List<Reward> _rewards;
    public List<Action> _actions;

    private int _currentStateCounter;
    private State _currentState;
    private LastAdded _lastAdded;

    public List<State> States
    {
        get { return  _states; }
    }

    public int CurrentStateCounter
    {
        get { return _currentStateCounter; }
    }
    public State CurrentState
    {
        get { return _currentState; }
    }

    public void ReinitialiseRun()
    {
        _currentStateCounter = 0;
        _currentState = _states[0];
    }

    public Action GetActionForStateNumber(int number)
    {
        return _actions[number];
    }

    public (Action, Reward, State) NextStep()
    {
        int i = _currentStateCounter;
        if (_states[i].IsTerminal)
        {
            throw new System.Exception("can't have the next step, this is the end of the trajectory");
        }

        if (_currentStateCounter < _states.Count)
        {
            _currentStateCounter++;
            _currentState = _states[_currentStateCounter];
            return (_actions[i], _rewards[i], _states[i]);
        }
        else throw new System.Exception("last reward attained, returning first reward of the trajectory");

    }

    public State GetPreviousState(int step)
    {
        if (_currentStateCounter - step >=0)
        {
            return _states[_currentStateCounter - step];
        }
        else throw new System.Exception("you tried to go " + step + " steps back but the trajectory current state is state number " + _currentStateCounter);
    }

    public Trajectory(State initialState)
    {
        _lastAdded = LastAdded.state;
        _states = new List<State>();
        _rewards = new List<Reward>();
        _actions = new List<Action>();
        _currentStateCounter = 0;
        _currentState = initialState;
        _states.Add(initialState);
        
    }

    public int Lenght()
    {
        return _states.Count;
    }

    private enum LastAdded
    {
        state,
        action,
        reward
    }

    public void AddReward(Reward r)
    {
        if (_lastAdded == LastAdded.action)
        {
            _rewards.Add(r);
            _lastAdded = LastAdded.reward;
        }
        else throw new System.Exception("A reward can only be added after an action");
    }
    public void AddState(State s)
    {
        if (_lastAdded == LastAdded.reward)
        {
            _states.Add(s);
            _lastAdded = LastAdded.state;
        }
        else throw new System.Exception("A state can only be added after a reward");
    }
    public void AddAction(Action a)
    {
        if (_lastAdded == LastAdded.state)
        {
            _actions.Add(a);
            _lastAdded = LastAdded.action;
        }
        else throw new System.Exception("An action can only be added after a state");
    }

    public void AddStep(Action a, Reward r, State s)
    {
        AddAction(a);
        AddReward(r);
        AddState(s);
    }

    /// <summary>
    /// Compute the total return from time step i of the trajectory.
    /// </summary>
    /// <param name="i">The time step.</param>
    /// <param name="discount"> The discount to apply at each step.</param>
    /// <returns></returns>
    public float ReturnAtTimeStep(int i, float discount)
    {
        float res = 0;
        float totalDiscount = 1;
        if (i < _rewards.Count)
        {
            for(int j = i; j < _rewards.Count; j++)
            {
                res += totalDiscount * _rewards[j].Value;
                totalDiscount = totalDiscount * discount;
        }
        }
        else throw new System.Exception("the trajectory contains only " + _rewards.Count + "time steps.");

        return res;
    }

    /// <summary>
    /// Counts in a single trajectory how many times each state was crossed.
    /// </summary>
    /// <returns>A Dictionary containing states as key and the time each state was crossed as value</returns>
    public Dictionary<State, int> CountStatesCrossed()
    {
        Dictionary<State, int> statesAndCount = new Dictionary<State, int>();
        foreach(State s in _states)
        {
            if (statesAndCount.ContainsKey(s))
            {
                statesAndCount[s] += 1;
            }
            else
            {
                statesAndCount.Add(s, 1);
            }
        }

        return statesAndCount;
    }

}
