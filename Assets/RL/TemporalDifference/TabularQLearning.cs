using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class TabularQLearning 
{
    public float _learnRate;
    public float? _learnRateMultiplier;
    public float? _learnRateMinimum;
    public float _epsilon;
    public float? _epsilonMultiplier;
    public float? _epsilonMinimum;
    public float _discount;
    public float _defaultQValue;
    public QValueCollection _qValues;
    public int _step;

    private bool HasLearnRateMultiplier => !(_learnRateMultiplier is null) ;
    private bool HasLearnRateMinimum => !(_learnRateMinimum is null);
    private bool HasEpsilonMultiplier => !(_epsilonMultiplier is null);
    private bool HasEpsilonMinimum => !(_epsilonMinimum is null);

    public TabularQLearning(float learnRate, float epsilon, float discount, float defaultQValues = 0)
    {
        _qValues = new QValueCollection();
        _learnRate = learnRate;
        _epsilon = epsilon;
        _discount = discount;
        _defaultQValue = defaultQValues;
        _step = 0;
    }

    public TabularQLearning(float learnRate, float learRateMultiplier, float epsilon, float epsilonMultiplier,
    float discount, float defaultQValues = 0) : this(learnRate, epsilon, discount, defaultQValues)
    {
        _epsilonMultiplier = epsilonMultiplier;
        _learnRateMultiplier = learRateMultiplier;
    }

    public TabularQLearning(float learnRate, float learRateMultiplier, float learnRateMinimum,
        float epsilon, float epsilonMultiplier, float epsilonMinimum, float discount,
        float defaultQValues = 0) : this(learnRate, learRateMultiplier, epsilon, epsilonMultiplier, discount, defaultQValues)
    {
        _epsilonMinimum = epsilonMinimum;
        _learnRateMinimum = learnRateMinimum;
    }

    public Reward Step(Agent agent)
    {
        State currentS = agent.State;
        if (!_qValues.ContainsKey(agent.State)) InitialiseQValues(_qValues, currentS, agent, _defaultQValue);

        EnvironmentAction currentA = EnvironmentPolicy.ChooseActionEpsilonGreedy(currentS, _qValues[currentS], _epsilon);
        agent.ExecuteAction(currentA);
        // TODO maybe change by observeStateAndReward and do computeState in it....
        Reward currentR = agent.ObserveReward(); //this should wait for next update
        State nextS = agent.State; 

        UpdateQValues(currentR, currentS, currentA, nextS, agent);
        if (HasLearnRateMultiplier) ComputeNewLearnRate();
        if (HasEpsilonMultiplier) ComputeNewEpsilon();
        _step += 1;
        return currentR;
    }

    private void ComputeNewEpsilon() 
    {
        if (HasEpsilonMinimum) {
            if (_epsilon > _epsilonMinimum) _epsilon *= (float)_epsilonMultiplier;
            if (_epsilon <= _epsilonMinimum) _epsilon = (float)_epsilonMinimum;
        }
        else _epsilon *= (float)_epsilonMultiplier;
    }

    //learn rate goes way too fast to minimum !!
    private void ComputeNewLearnRate()
    {
        if (HasLearnRateMinimum)
        {
            if (_learnRate > _learnRateMinimum) _learnRate = (float) (1f / (_learnRateMultiplier * (_step + (1f / _learnRateMultiplier))));
            if (_learnRate <= _learnRateMinimum) _learnRate = (float)_learnRateMinimum;
        }
        else _learnRate = (float)(1f / (_learnRateMultiplier * _step + 1f));
    }

    private void UpdateQValues(Reward currentR, State currentS, EnvironmentAction currentA, State nextS, Agent agent)
    {
        float TDError = ComputeTDError(currentR, currentS, currentA, nextS, agent);

        _qValues[currentS][currentA]._stateActionValue = _qValues[currentS][currentA]._stateActionValue
            + _learnRate * TDError;
    }

    private static QValueCollection InitialiseQValues(QValueCollection QValues, State s, Agent agent, float defautValue = 0)
    {
        var actionsList = agent.GetAvailableActions(s);
        var actionValueDictionary = new Dictionary<EnvironmentAction, StateActionValue>();
        foreach (EnvironmentAction action in actionsList)
        {
            actionValueDictionary.Add(action, new StateActionValue(defautValue));
        }
        QValues.Add(s, actionValueDictionary);
        return QValues;
    }

    private float ComputeTDError(Reward currentReward, State currentState, EnvironmentAction currentAction,
        State nextState, Agent agent)
    {
        if (!_qValues.ContainsKey(nextState)) InitialiseQValues(_qValues, nextState, agent);

        float maxQValue = _qValues[nextState].Max(x => x.Value._stateActionValue);

        return currentReward.Value + _discount * maxQValue - _qValues[currentState][currentAction]._stateActionValue;
    }

}
