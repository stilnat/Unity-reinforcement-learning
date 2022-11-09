using System.IO;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class WalkerTrainerWithSarsaLambda : Trainer
{

    public EnvironmentSarsaLambda _trainingMethod;

    public float learnRate = 0.9f;
    public float epsilon = 1f;
    public float discount = 1f;
    public float defaultQValue = 0;
    public float eligibilityDecay = 0.99f;

    public override void Start()
    {
        base.Start();
        _trainingMethod = new EnvironmentSarsaLambda(_agent.ComputeState(), _agent.GetAvailableActions(_agent.ComputeState())[0],
            epsilon, discount, learnRate, defaultQValue, eligibilityDecay);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_updateCount % _nFrame == 0)
        {
            if (_agent.State.IsTerminal)
            {
                EndEpisode();
            }
            else
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


