using System.Collections.Generic;
using System;

public class SarsaLambda
{

    /// <summary>
    /// Evaluate the state actions values of the system, using the SARSA Lambda algorithm. Also compute the optimal epsilon-greedy policy.
    /// </summary>
    /// <param name="initialStateActionValues">The initial values of each state-action of the system.</param>
    /// <param name="initialState"> The state to begin from.</param>
    /// <param name="systemDynamic">The dynamic of the system.</param>
    /// <param name="policy">The policy to follow.</param>
    /// <param name="discount"></param>
    /// <param name="eligibilityDecay"> The decay parameter of the eligibility trace, must be between 0 and 1.</param>
    /// <param name="learningRate">The learning rate, must be between 0 and 1.</param>
    /// <param name="epsilon"> The probability of choosing randomly an action.</param>
    /// <param name="nbIterations">The number of episodes to run.</param>
    /// <returns>The optimal policy found, and the state action values.</returns>
    public static (MCPolicy,Dictionary<State, Dictionary<Action, StateActionValue>>) SarsaLambdaEvaluate(Dictionary<StateAction, float> initialStateActionValues, State initialState, MCSystemDynamic systemDynamic,
       MCPolicy policy, float discount, float eligibilityDecay, float learningRate, float epsilon, float nbIterations = 50)
    {


        Dictionary<State, Dictionary<Action, StateActionValue>> actionStateValueDictionary = Initialise(systemDynamic, initialStateActionValues);

        for (int k = 0; k < nbIterations; k++)
        {
            Trajectory trajectory = new Trajectory(initialState);
            State currentState = initialState;
            float TDError = 0;

            Action currentAction = policy.ChooseActionEpsilonGreedy(currentState, actionStateValueDictionary[currentState], epsilon);
            Action nextAction = currentAction;

            while (!trajectory.CurrentState.IsTerminal)
            {
                currentAction = nextAction;
                var res = systemDynamic.NextStateAndReward(currentState, currentAction);
                State nextState = res.Item1;
                Reward currentReward = res.Item2;
                trajectory.AddStep(currentAction, currentReward, nextState);
                if (nextState.IsTerminal) { break; }
                nextAction = policy.ChooseActionEpsilonGreedy(nextState, actionStateValueDictionary[nextState], epsilon);

                TDError = ComputeTDError(currentReward, currentState, currentAction, nextState, nextAction, discount, actionStateValueDictionary);

                SetEligibilityTrace(currentState, currentAction, trajectory, discount, eligibilityDecay, actionStateValueDictionary);

                UpdateStateActionValues(trajectory, learningRate, TDError, actionStateValueDictionary);

                
                trajectory.NextStep();
                currentState = nextState;
            }

            ResetEligibilityTrace(trajectory, actionStateValueDictionary);

        }

        return (policy, actionStateValueDictionary);
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
    /// Compute The TD target of Sarsa Lambda.
    /// </summary>
    private static float ComputeTDError(Reward currentReward, State currentState, Action currentAction, State nextState, Action nextAction, float discount,
         Dictionary<State, Dictionary<Action, StateActionValue>> actionStateValueDictionary)
    {
        return currentReward.Value + discount * actionStateValueDictionary[nextState][nextAction]._stateActionValue
               - actionStateValueDictionary[currentState][currentAction]._stateActionValue;
    }

    /// <summary>
    /// Update the eligibility trace of all state actions encountered in the current trajectory.
    /// </summary>
    private static void SetEligibilityTrace(State currentState, Action currentAction, Trajectory trajectory, float discount, float eligibilityDecay,
        Dictionary<State, Dictionary<Action, StateActionValue>> actionStateValueDictionary)
    {
        actionStateValueDictionary[currentState][currentAction]._eligibilityTrace += 1;

        int i = 0;
        foreach (State stateEncountered in trajectory.States) //setting eligibility traces
        {
            if(trajectory.States[trajectory.States.Count-1] != stateEncountered)
            {
                actionStateValueDictionary[stateEncountered][trajectory.GetActionForStateNumber(i)]._eligibilityTrace *= discount * eligibilityDecay;
                i = i + 1;
            }

        }
    }

    /// <summary>
    /// Update the state-action value of each state-action encountered in the current trajectory.
    /// </summary>
    private static void UpdateStateActionValues(Trajectory trajectory, float learningRate, float TDError,
        Dictionary<State, Dictionary<Action, StateActionValue>> actionStateValueDictionary)
    {
        int i = 0;
        foreach (State stateEncountered in trajectory.States)
        {
            if (trajectory.States[trajectory.States.Count - 1] != stateEncountered)
            {
                actionStateValueDictionary[stateEncountered][ trajectory.GetActionForStateNumber(i)]._stateActionValue +=
                learningRate * TDError * actionStateValueDictionary[stateEncountered][trajectory.GetActionForStateNumber(i)]._eligibilityTrace;
            i = i + 1;
            }
        }
    }

    /// <summary>
    /// Set the eligibility trace of all state-action of the trajectory to zero.
    /// </summary>
    private static void ResetEligibilityTrace(Trajectory trajectory, Dictionary<State, Dictionary<Action, StateActionValue>> actionStateValueDictionary)
    {

        int i = 0;
        foreach (State stateEncountered in trajectory.States) //setting elgibility traces
        {
            if (trajectory.States[trajectory.States.Count - 1] != stateEncountered)
            {
                actionStateValueDictionary[stateEncountered][trajectory.GetActionForStateNumber(i)]._eligibilityTrace = 0;
                i = i + 1;
            }
        }
    }


}
