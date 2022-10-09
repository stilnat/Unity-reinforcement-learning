using System.IO;
using UnityEngine;

public class CartPoleTraining : Trainer
{
    private TabularQLearning _trainingMethod;

    public float learnRate = 0.9f;
    public float learnRateMultiplier = 0.99f;
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

        if (chargeData) ChargeTraining();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((_agent as CartPoleAgent)._updateCount % (_agent as CartPoleAgent)._nFrame == 0)
        {
            if (_agent.State.IsTerminal) _agent.Initialise();

            if (_agent.State.IsTerminal == false)
            {
                _trainingMethod.Step(_agent);
            }
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
    }

    public override void SaveTraining()
    {
        string jsonString = JsonUtility.ToJson(_trainingMethod._qValues);
        File.WriteAllText(@".\test.json", jsonString);
    }
}
