using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDLambdaPolicyEvaluation
{
    public class TDLambdaStatevalue
    {
        public TDLambdaStatevalue(float stateValue, int numberOfVisit)
        {
            _stateValue = stateValue;
            _numberOfVisit = numberOfVisit;
            _eligibilityTrace = 0;
        }

        public float _eligibilityTrace;
        public float _stateValue;
        public int _numberOfVisit;
    }
    public static Dictionary<State, TDLambdaStatevalue> TDLambdaEvaluate(Dictionary<State, float> initialStateValues, State initialState, MCSystemDynamic systemDynamic,
       MCPolicy policy, float discount, float eligibilityDecay, float learningRate, float nbIterations = 50)
    {


        Dictionary<State, TDLambdaStatevalue> _stateValueDictionary = new Dictionary<State, TDLambdaStatevalue>();


        foreach (State s in systemDynamic.getAllStates())
        {
            TDLambdaStatevalue statevalue = new TDLambdaStatevalue(initialStateValues[s], 0);
            _stateValueDictionary.Add(s, statevalue);
        }


        for (int k = 0; k < nbIterations; k++)
        {
            Trajectory trajectory = new Trajectory(initialState);
            State currentState = initialState;
            float TDError = 0;

            while (!trajectory.CurrentState.IsTerminal)
            {
                Action currentAction = policy.ChooseAction(currentState);
                var res = systemDynamic.NextStateAndReward(currentState, currentAction);
                State nextState = res.Item1;
                Reward currentReward = res.Item2;
                TDError = currentReward.Value + discount * _stateValueDictionary[nextState]._stateValue - _stateValueDictionary[currentState]._stateValue;
                
                _stateValueDictionary[currentState]._eligibilityTrace += 1;


                foreach (State stateEncountered in trajectory.States) //setting elgibility traces
                {
                        _stateValueDictionary[stateEncountered]._eligibilityTrace = discount * eligibilityDecay * _stateValueDictionary[stateEncountered]._eligibilityTrace;
                }

                foreach (State stateEncountered in trajectory.States)
                {
                    _stateValueDictionary[stateEncountered]._stateValue += learningRate * TDError * _stateValueDictionary[stateEncountered]._eligibilityTrace;
                }

                trajectory.AddAction(currentAction);
                trajectory.AddReward(currentReward);
                trajectory.AddState(nextState);
                trajectory.NextStep();
                currentState = nextState;
                
            }

            foreach (State stateEncountered in trajectory.States) //setting elgibility traces
            {
                _stateValueDictionary[stateEncountered]._eligibilityTrace = 0;
            }

        }

        return _stateValueDictionary;
    }
}
