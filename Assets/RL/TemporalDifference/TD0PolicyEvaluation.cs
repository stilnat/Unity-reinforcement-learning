using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD0PolicyEvaluation
{
    public class TDStatevalue
    {
        public TDStatevalue(float stateValue, int numberOfVisit)
        {
            _stateValue = stateValue;
            _numberOfVisit = numberOfVisit;
        }

        public float _stateValue;
        public int _numberOfVisit;
    }
    public static Dictionary<State, TDStatevalue> TD0Evaluate(Dictionary<State, float> initialStateValues, State initialState, MCSystemDynamic systemDynamic,
       MCPolicy policy, float discount, float alpha, float nbIterations = 50)
    {


        Dictionary<State, TDStatevalue> _stateValueDictionary = new Dictionary<State, TDStatevalue>();

        // does not return the last state ? state e is not in the dictionary.
        foreach(State s in systemDynamic.getAllStates())
        {
            TDStatevalue statevalue = new TDStatevalue(initialStateValues[s], 0);
            _stateValueDictionary.Add(s, statevalue);
        }


        for (int k = 0; k < nbIterations; k++)
        {
            Trajectory trajectory = systemDynamic.GenerateTrajectory(initialState, policy);
            for (int i = 0; i < trajectory.Lenght(); i++)
            {
                if (trajectory.CurrentState.IsTerminal)
                {
                    break;
                }
                var res = trajectory.NextStep();
                State s = res.Item3;
                Action a = res.Item1;
                Reward r = res.Item2;

                _stateValueDictionary[s]._stateValue = _stateValueDictionary[s]._stateValue 
                                    + alpha * (r.Value + discount * _stateValueDictionary[trajectory.CurrentState]._stateValue - _stateValueDictionary[s]._stateValue);
                _stateValueDictionary[s]._numberOfVisit += 1;


            }

        }

        return _stateValueDictionary;
    }

}
