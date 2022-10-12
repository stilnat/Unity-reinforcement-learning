using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This finally works !! To optimise it, instead of updating every action value, use the trajectory to determine 
/// the last states in which the eligibility trace is sensibly superior to 0.  Update only those states.
/// Much more computationaly efficient.
/// It would be also cool to show state action using 
/// </summary>

public class EnvironmentSarsaLambda
{

    public QValueCollection _qValues;
    public float _eligibilityDecay;
    public float _learnRate;
    public float _epsilon;
    public float _discount;
    public float _defaultQValue;
    public EnvironmentTrajectory _trajectory;
    public EnvironmentAction _currentA;
    public EnvironmentAction _initialA;

    public EnvironmentSarsaLambda(State initialState, EnvironmentAction initialAction, float epsilon, float discount, float learnRate, float defautQValue, float eligibilityDecay)
    {
        _epsilon = epsilon;
        _discount = discount;
        _learnRate = learnRate;
        _defaultQValue = defautQValue;
        _eligibilityDecay = eligibilityDecay;
        _trajectory = new EnvironmentTrajectory(initialState);
        _qValues = new QValueCollection();
        _currentA = initialAction;
        _initialA = initialAction;
    }

    private QValueCollection InitialiseQValues(State s, Agent agent, float defautValue = 0)
    {
        var actionsList = agent.GetAvailableActions(s);
        var actionValueDictionary = new Dictionary<EnvironmentAction, StateActionValue>();
        foreach (EnvironmentAction action in actionsList)
        {
            actionValueDictionary.Add(action, new StateActionValue(defautValue));
        }
        _qValues.Add(s, actionValueDictionary);
        return _qValues;
    }



    /// <summary>
    /// it seems that execute action sometimes does not actually move in time the agent, resulting in a delay between state updated and 
    /// actions executed and making the algorithm faulty.
    /// </summary>
    /// <param name="agent"></param>
    public void Step(Agent agent)
    {
        State currentS = agent.State;

        if (!_qValues.ContainsKey(currentS)) InitialiseQValues(currentS, agent, _defaultQValue);
        agent.ExecuteAction(_currentA);

        Reward currentR = agent.ObserveReward(); //this should wait for next update
        State nextS = agent.State;

        _trajectory.AddStep(_currentA, currentR, nextS);
        if (nextS.IsTerminal)
        {
            ResetEligibilityTrace();
            agent.Initialise();
            _trajectory = new EnvironmentTrajectory(agent.State);
            _currentA = _initialA;
            return;
        }

        if (!_qValues.ContainsKey(nextS)) InitialiseQValues(nextS, agent, _defaultQValue);
        EnvironmentAction nextA = EnvironmentPolicy.ChooseActionEpsilonGreedy(nextS, _qValues[nextS], _epsilon);

        float TDError = ComputeTDError(currentR, currentS, _currentA, nextS, nextA);

        _qValues[currentS][_currentA]._eligibilityTrace += 1;

        UpdateStateActionValues(TDError);

        SetEligibilityTrace(currentS, _currentA);

        _trajectory.NextStep();

        _currentA = nextA;
    }


    /// <summary>
    /// Compute The TD target of Sarsa Lambda.
    /// </summary>
    private float ComputeTDError(Reward currentR, State currentS, EnvironmentAction currentA, State nextS, EnvironmentAction nextA)
    {
        return currentR.Value + _discount * _qValues[nextS][nextA]._stateActionValue
               - _qValues[currentS][currentA]._stateActionValue;
    }

    /// <summary>
    /// Update the eligibility trace of all state actions encountered in the current trajectory.
    /// issue when getting back on it's path ...
    /// </summary>
    private void SetEligibilityTrace(State currentS, EnvironmentAction currentA)
    {
        State lastState = _trajectory.States[_trajectory.States.Count - 1];
        int i = 0;
        foreach (State state in _qValues.Keys)
        {
            foreach (EnvironmentAction action in _qValues[state].Keys)
            {
                _qValues[state][action]._eligibilityTrace *= _discount * _eligibilityDecay;
            }
        }
    }

    /// <summary>
    /// Update the state-action value of each state-action encountered in the current trajectory.
    /// </summary>
    private void UpdateStateActionValues(float TDError)
    {
        int i = 0;
        foreach (State state in _qValues.Keys)
        {
            foreach(EnvironmentAction action in _qValues[state].Keys)
            {
                _qValues[state][action]._stateActionValue += _learnRate * TDError * _qValues[state][action]._eligibilityTrace;
            }
        }
    }

    /// <summary>
    /// Set the eligibility trace of all state-action of the trajectory to zero.
    /// </summary>
    private void ResetEligibilityTrace()
    {

        int i = 0;
        foreach (State state in _trajectory.States) //setting elgibility traces
        {
            if (i < _trajectory.States.Count - 1)
            {
                _qValues[state][_trajectory.GetActionForStateNumber(i)]._eligibilityTrace = 0;
                i = i + 1;
            }
        }
    }
}
