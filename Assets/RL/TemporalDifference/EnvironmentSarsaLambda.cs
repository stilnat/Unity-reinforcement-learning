using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// issue with sarsa lambda, it sometimes stay stuck in a loop with high discount factor and epsilon to zero....
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

    public EnvironmentSarsaLambda(State initialState, float epsilon, float discount, float learnRate, float defautQValue, float eligibilityDecay)
    {
        _epsilon = epsilon;
        _discount = discount;
        _learnRate = learnRate;
        _defaultQValue = defautQValue;
        _eligibilityDecay = eligibilityDecay;
        _trajectory = new EnvironmentTrajectory(initialState);
        _qValues = new QValueCollection();
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

        EnvironmentAction currentA = EnvironmentPolicy.ChooseActionEpsilonGreedy(currentS, _qValues[currentS], _epsilon);
        agent.ExecuteAction(currentA);
        Reward currentR = agent.ObserveReward(); //this should wait for next update
        State nextS = agent.State;

        _trajectory.AddStep(currentA, currentR, nextS);
        if (nextS.IsTerminal)
        {
            ResetEligibilityTrace();
            agent.Initialise();
            _trajectory = new EnvironmentTrajectory(agent.State);
            return;
        }

        if (!_qValues.ContainsKey(nextS)) InitialiseQValues(nextS, agent, _defaultQValue);
        EnvironmentAction nextA = EnvironmentPolicy.ChooseActionEpsilonGreedy(nextS, _qValues[nextS], _epsilon);

        float TDError = ComputeTDError(currentR, currentS, currentA, nextS, nextA);

        _qValues[currentS][currentA]._eligibilityTrace += 1;

        UpdateStateActionValues(TDError);

        SetEligibilityTrace(currentS, currentA);

        _trajectory.NextStep();
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
        foreach (State stateEncountered in _trajectory.States) //setting eligibility traces
        {
            if (i < _trajectory.States.Count -1)
            {
                    EnvironmentAction actionAtStepI = _trajectory.GetActionForStateNumber(i);

                    _qValues[stateEncountered][actionAtStepI]._eligibilityTrace *= _discount * _eligibilityDecay;
                    i++;
            }
        }
    }

    /// <summary>
    /// Update the state-action value of each state-action encountered in the current trajectory.
    /// </summary>
    private void UpdateStateActionValues(float TDError)
    {
        int i = 0;
        foreach (State state in _trajectory.States)
        {
            if (i < _trajectory.States.Count - 1)
            {
                var action = _trajectory.GetActionForStateNumber(i);
                _qValues[state][action]._stateActionValue +=  _learnRate * TDError * _qValues[state][action]._eligibilityTrace;
                i = i + 1;
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
