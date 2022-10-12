using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{

    private int stateEveryNUpdate = 1;
    public State _state;
    protected GameObject _agentGameObject;
    protected EnvironmentPolicy environmentPolicy;
    public bool _initialise;

    public State State
    {
        get { return _state; }
    }
  /*  public abstract State InitialState
    {
        get;
    }*/


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract State ComputeState();

    public abstract void ExecuteAction(EnvironmentAction action);

    public abstract Reward ObserveReward();

    public abstract List<EnvironmentAction> GetAvailableActions(State s);

    public abstract void Initialise();

    /*protected virtual void OnProcessCompleted() //protected virtual method
    {
        //if ProcessCompleted is not null then call delegate
        ProcessCompleted?.Invoke();
    }*/


}
