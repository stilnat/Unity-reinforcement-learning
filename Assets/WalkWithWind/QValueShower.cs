using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QValueShower : MonoBehaviour
{
    public QValueCollection _qValueCollection;
    TextMeshPro _qValueDisplay;
    WalkerAgent _agent;
    RectTransform rect;
    State s;
    EnvironmentAction a;
    bool initialized = false;
    // Start is called before the first frame update
    void Start()
    {
       
        _agent = FindObjectOfType<WalkerAgent>();
        
        _qValueDisplay = GetComponent<TextMeshPro>();
        rect = GetComponent<RectTransform>();
        s = ComputeState();

        a = ComputeAction();
    }

    private void LateUpdate()
    {
        if (!initialized)
        {
            var trainer = FindObjectOfType<WalkerTrainerWithSarsaLambda>();
            _qValueCollection = trainer._trainingMethod._qValues;
        }

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (_qValueCollection != null && _qValueCollection.ContainsKey(s) && _qValueCollection[s].ContainsKey(a))
        {
            _qValueDisplay.text = _qValueCollection[s][a]._stateActionValue.ToString();
        }
        else
        {
            _qValueDisplay.text = "";
        }
    }



    State ComputeState()
    {
        int x = (int) rect.position.x;
        int y = (int) rect.position.z;
        return new State(false, x, y);
    }

    EnvironmentAction ComputeAction()
    {
        if (rect.localRotation.eulerAngles.z == 0) return new EnvironmentAction(_agent.MoveForwardZ);
        if (rect.localRotation.eulerAngles.z == 270) return new EnvironmentAction(_agent.MoveForwardX);
        if (rect.localRotation.eulerAngles.z == 180) return new EnvironmentAction(_agent.MoveBackwardZ);
        return new EnvironmentAction(_agent.MoveBackwardX);
    }
}
