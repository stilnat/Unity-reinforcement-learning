using System.IO;
using UnityEngine;

public class CartPoleTraining : MonoBehaviour
{

    private Agent _agent;
    private QValueCollection _actionStateValue;
    private EnvironmentPolicy _policyToLearn;
    private EnvironmentPolicy _policyToFollow;

   // Start is called before the first frame update
   void Start()
    {
        _agent = gameObject.GetComponent<Agent>();
        if (File.Exists(@".\test.json"))
        {
            string json = File.ReadAllText(@".\test.json");
            _actionStateValue = QValueCollection.CreateFromJSON(json);
            _actionStateValue.infoToCollection(_agent);
        }
        else _actionStateValue = new QValueCollection();

        _policyToLearn = new EnvironmentPolicy();
        _policyToFollow = new EnvironmentPolicy();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((_agent as CartPoleAgent)._updateCount % (_agent as CartPoleAgent)._nFrame == 0)
        {
            if (_agent.State.IsTerminal)
            {
                _agent.Initialise();
            }

            //Debug.Log("Initialise in cartpoleTraining :  position = " + this.gameObject.transform.position);

            if (_agent.State.IsTerminal == false)
            {
                var res = QLearning.QLearningPolicyWithUnityOneStep(_actionStateValue, _agent.State, _agent, _policyToFollow, _policyToLearn, 1f, 0.2f, 0.3f);

                _policyToLearn = res.Item2;
                _actionStateValue = res.Item3;
            }
        }


    }

    private void OnApplicationQuit()
    {
        Debug.Log("on application quit");

        string jsonString = JsonUtility.ToJson(_actionStateValue);
        File.WriteAllText(@".\test.json", jsonString);
    }



}
