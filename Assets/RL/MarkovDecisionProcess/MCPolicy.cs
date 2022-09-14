using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCPolicy
{
    private Dictionary<State, List<ActionProbability>> _stateActionDic;

    public MCPolicy()
    {
        _stateActionDic = new Dictionary<State, List<ActionProbability>>();
    }

   
    public void AddPolicyForState(State s, List<ActionProbability> actionProbabilities)
    {
        float sum = 0;
        foreach(ActionProbability actionProbability in actionProbabilities)
        {
            if (_stateActionDic.ContainsKey(s))
            {
                _stateActionDic[s].Add(actionProbability);
            }
            else
            {
                _stateActionDic.Add(s, new List<ActionProbability>());
                _stateActionDic[s].Add(actionProbability);
            }
            
            sum += actionProbability.P;
        }
        if(sum != 1)
        {
            throw new System.Exception("The sum of probabilities is " + sum + " instead of 1.");
        }

    }

    public List<ActionProbability> GetPolicyForState(State s)
    {
        return _stateActionDic[s];
    }

    
    public Action ChooseAction(State s)
    {
        // same code as in MC dynamic, maybe factorizing ?
        List<ActionProbability> actionProbabilities = GetPolicyForState(s);
        return actionProbabilities.RandomElementByWeight(x => x.P).A;
    }

}
