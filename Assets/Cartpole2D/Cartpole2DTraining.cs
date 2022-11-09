using System.IO;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]

//maybe wait for change of state when training using qlearning when applying an action before trying another one.
public class Cartpole2DTraining : Trainer
{

    public TabularQLearning _trainingMethod;

    public float learnRate = 0.9f;
    public float learnRateMultiplier = 0.998f;
    public float learnRateMinimum = 0.01f;
    public float epsilon = 1f;
    public float epsilonMultiplier = 0.999f;
    public float epsilonMinimum = 0.05f;
    public float discount = 0.2f;
    public float defaultQValue = 40;
    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _trainingMethod = new TabularQLearning(learnRate, learnRateMultiplier, learnRateMinimum, epsilon,
           epsilonMultiplier, epsilonMinimum, discount, defaultQValue);

    }

    public override void FixedUpdate()
    {
        if ((_agent as Cartpole2DAgent)._updateCount % (_agent as Cartpole2DAgent)._nFrame == 0)
        {
            if (_agent.State.IsTerminal)
            {
                EndEpisode();
            }

            if (_agent.State.IsTerminal == false)
            {
                _cumulatedReward += _trainingMethod.Step(_agent);
            }
        }
    }

    private static List<float> CreateFromJSONReward(string json)
    {
        return JsonUtility.FromJson<List<float>>(json);
    }

    public override void ChargeTraining()
    {
        if (File.Exists(@".\test.json"))
        {
            string json = File.ReadAllText(@".\test.json");
            _trainingMethod._qValues = QValueCollection.CreateFromJSON(json);
            _trainingMethod._qValues.infoToCollection(_agent);
        }
        if (File.Exists(@".\rewards.json"))
        {
            string jsonRewards = File.ReadAllText(@".\testrewards.json");
            CumulativeRewardEpisodes = CreateFromJSONReward(jsonRewards);
        }
    }

    public override void SaveTraining()
    {
        string jsonString = JsonUtility.ToJson(_trainingMethod._qValues);
        File.WriteAllText(@".\test.json", jsonString);

        string jsonrewards = JsonUtility.ToJson(this);
        File.WriteAllText(@".\testrewards.json", jsonrewards);
    }
}

