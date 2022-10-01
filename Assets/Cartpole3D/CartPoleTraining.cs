using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartPoleTraining : MonoBehaviour
{

    private Agent _agent;
    private Dictionary<State, Dictionary<EnvironmentAction, StateActionValue>> _actionStateValue;
    private EnvironmentPolicy _policyToLearn;
    private EnvironmentPolicy _policyToFollow;

    // Start is called before the first frame update
    void Start()
    {
        _agent = gameObject.GetComponent<Agent>();
        _actionStateValue = new Dictionary<State, Dictionary<EnvironmentAction, StateActionValue>>();
        _policyToLearn = new EnvironmentPolicy();
        _policyToFollow = new EnvironmentPolicy();
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.State.IsTerminal)
        {
            _agent.Initialise();

        }

        //Debug.Log("Initialise in cartpoleTraining :  position = " + this.gameObject.transform.position);

        if (_agent.State.IsTerminal == false)
        {
            var res = QLearning.QLearningPolicyWithUnityOneStep(_actionStateValue, _agent.State, _agent, _policyToFollow, _policyToLearn, 0.99f, 0.2f, 0.1f);
            
            _policyToLearn = res.Item2;
            _actionStateValue = res.Item3;
        }

    }
}
