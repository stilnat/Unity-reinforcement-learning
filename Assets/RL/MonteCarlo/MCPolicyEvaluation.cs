using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MCPolicyEvaluation
{

    public class MCStatevalue
    {
        public MCStatevalue(float stateValue, bool visited, int numberOfVisit, float totalReturn)
        {
            _stateValue = stateValue;
            _visited = visited;
            _numberOfVisit = numberOfVisit;
            _totalReturn = totalReturn;
        }

        public float _stateValue;
        public bool _visited;
        public int _numberOfVisit;
        public float _totalReturn;
    }

    public static Dictionary<State, MCStatevalue> FirstVisitMCEvaluate(Dictionary<State, float> initialStateValues, State initialState, MCSystemDynamic systemDynamic,
        MCPolicy policy, float discount, float nbIterations = 50, int verbose = 0)
    {
        //TODO does not include the final states value ?
        Dictionary<State, MCStatevalue> _stateValueDictionary = new Dictionary<State, MCStatevalue>() ;


        for (int k =0; k< nbIterations; k++)
        {
            Trajectory trajectory = systemDynamic.GenerateTrajectory(initialState , policy);
            for(int i =0; i< trajectory.Lenght(); i++)
            {
                if (trajectory.CurrentState.IsTerminal)
                {
                    break;
                }
                var res = trajectory.NextStep();
                State s = res.Item3;
                Action a = res.Item1;
                Reward r = res.Item2;
                if (_stateValueDictionary.ContainsKey(s))
                {
                    if (!_stateValueDictionary[s]._visited)
                    {
                        _stateValueDictionary[s]._numberOfVisit += 1;
                        _stateValueDictionary[s]._visited = true;
                        _stateValueDictionary[s]._totalReturn += trajectory.ReturnAtTimeStep(i, discount);
                        _stateValueDictionary[s]._stateValue = _stateValueDictionary[s]._totalReturn / _stateValueDictionary[s]._numberOfVisit;
                    }
                }
                else
                {
                    float returnAtStep = trajectory.ReturnAtTimeStep(i, discount);
                    MCStatevalue statevalue = new MCStatevalue(initialStateValues[s] + returnAtStep, true, 1, returnAtStep);
                    _stateValueDictionary.Add(s, statevalue);
                }
            }

            foreach(State s in _stateValueDictionary.Keys)
            {
                _stateValueDictionary[s]._visited = false;
            }
        }

        return _stateValueDictionary;
    }


}
