using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{

    private bool _isSimulationPaused = false;
    private Agent[] _agents;
    // Start is called before the first frame update
    void Start()
    {
        _agents = FindObjectsOfType<Agent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isSimulationPaused) PauseSimulation();
            else ResumeSimulation();
        }
    }

    void PauseSimulation()
    {

    }

    void ResumeSimulation()
    {

    }
}
