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
        float[] probabilities = new float[actionProbabilities.Count];

        float totalWeight = actionProbabilities[0].P;
        probabilities[0] = actionProbabilities[0].P;

        for (int i = 1; i < actionProbabilities.Count; i++)
        {
            probabilities[i] = probabilities[i - 1] + actionProbabilities[i].P;
            totalWeight += actionProbabilities[i].P;
        }


        float randomNumber = Random.Range(0, totalWeight);
        bool chosen = false;
        for (int i = 0; i < probabilities.Length; i++)
        {
            if (probabilities[i] > randomNumber)
            {
                return actionProbabilities[i].A;
            }
        }

        if (!chosen)
        {
            throw new System.Exception("no actions were chosen, returning first action by default");
        }

        // return random stuff 
        return actionProbabilities[0].A;

    }

}
