using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class QLearning
{
    public static (MCPolicy, Dictionary<State, Dictionary<Action, StateActionValue>>) QLearningPolicy(Dictionary<StateAction, float> initialStateActionValues, State initialState, MCSystemDynamic systemDynamic,
      MCPolicy policy, float discount, float learningRate, float epsilon, float nbIterations = 50)
    {


        Dictionary<State, Dictionary<Action, StateActionValue>> actionStateValueDictionary = Initialise(systemDynamic, initialStateActionValues);
        MCPolicy policyToLearn = new MCPolicy();

        for (int k = 0; k < nbIterations; k++)
        {
            State currentState = initialState;
            float TDError = 0;

            while (!currentState.IsTerminal)
            {
                policyToLearn.ChooseActionGreedy(currentState, actionStateValueDictionary[currentState]);
                Action currentAction = policy.ChooseActionEpsilonGreedy(currentState, actionStateValueDictionary[currentState], epsilon);
                var res = systemDynamic.NextStateAndReward(currentState, currentAction);
                State nextState = res.Item1;
                Reward currentReward = res.Item2;
                if (nextState.IsTerminal)
                {
                    break;
                }
                TDError = ComputeTDError(currentReward, currentState, currentAction, nextState, discount, actionStateValueDictionary);
                actionStateValueDictionary[currentState][currentAction]._stateActionValue = actionStateValueDictionary[currentState][currentAction]._stateActionValue
                    + learningRate * TDError;
                currentState = nextState;
            }
        }

        return (policyToLearn, actionStateValueDictionary);
    }

    /// <summary>
    /// Initialise the dictionary containing state-action values.
    /// </summary>
    private static Dictionary<State, Dictionary<Action, StateActionValue>> Initialise(MCSystemDynamic systemDynamic, Dictionary<StateAction, float> initialStateActionValues)
    {
        var actionStateValueDictionary = new Dictionary<State, Dictionary<Action, StateActionValue>>();
        List<StateAction> allStates = systemDynamic.getAllStatesActions();

        foreach (StateAction stateAction in allStates)
        {
            if (!actionStateValueDictionary.ContainsKey(stateAction.S))
            {
                actionStateValueDictionary.Add(stateAction.S, new Dictionary<Action, StateActionValue>());
            }

            actionStateValueDictionary[stateAction.S].Add(stateAction.A, new StateActionValue(initialStateActionValues[stateAction]));
        }

        return actionStateValueDictionary;
    }


    /// <summary>
    /// Compute The TD target of Q-Learning.
    /// </summary>
    private static float ComputeTDError(Reward currentReward, State currentState, Action currentAction, State nextState, float discount,
         Dictionary<State, Dictionary<Action, StateActionValue>> actionStateValueDictionary)
    {
        float maxStateActionValue = actionStateValueDictionary[nextState].Max(x => x.Value._stateActionValue);

        return currentReward.Value + discount * maxStateActionValue - actionStateValueDictionary[currentState][currentAction]._stateActionValue;
    }



}
