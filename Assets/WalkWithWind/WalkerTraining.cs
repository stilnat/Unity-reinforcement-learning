using System.IO;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WalkerTraining : Trainer
{ 

    private TabularQLearning _trainingMethod;

    public float learnRate = 0.9f;
    public float learnRateMultiplier = 0.005f;
    public float learnRateMinimum = 0.1f;
    public float epsilon = 1f;
    public float epsilonMultiplier = 0.99f;
    public float epsilonMinimum = 0.01f;
    public float discount = 1f;
    public float defaultQValue = 0;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _trainingMethod = new TabularQLearning(learnRate, learnRateMultiplier, learnRateMinimum, epsilon,
            epsilonMultiplier, epsilonMinimum, discount, 0);
    }

    public override void FixedUpdate()
    {
        if (_updateCount % _nFrame == 0)
        {
            if (_agent.State.IsTerminal)
            {
                EndEpisode();
            }

            if (_agent.State.IsTerminal == false)
            {
                _trainingMethod.Step(_agent);
                epsilon = _trainingMethod._epsilon;
                learnRate = _trainingMethod._learnRate;
            }
            _step += 1;
        }
    }


    public override void ChargeTraining()
    {
        if (File.Exists(@".\test.json"))
        {
            string json = File.ReadAllText(@".\test.json");
            _trainingMethod._qValues = QValueCollection.CreateFromJSON(json);
            _trainingMethod._qValues.infoToCollection(_agent);
        }
        else _trainingMethod._qValues = new QValueCollection();
        if (File.Exists(@".\rewards.json"))
        {
            string jsonRewards = File.ReadAllText(@".\testrewards.json");
            CumulativeRewardEpisodes = CreateFromJSONReward(jsonRewards);
        }
        else CumulativeRewardEpisodes = new List<float>();
    }

    public override void SaveTraining()
    {
        string jsonrewards = JsonUtility.ToJson(this);
        File.WriteAllText(@".\testrewards.json", jsonrewards);

        string jsonString = JsonUtility.ToJson(_trainingMethod._qValues);
        File.WriteAllText(@".\test.json", jsonString);
    }
    private static List<float> CreateFromJSONReward(string json)
    {
        return JsonUtility.FromJson<List<float>>(json);
    }

}

