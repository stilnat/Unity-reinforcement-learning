using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDWalkerAgent: Agent
{
    //public override State InitialState { get => new State(false, 0); }

    public void Start()
    {
        _state = ComputeState();
    }

    

    public override State ComputeState()
    {
        int zPosition = (int)transform.position.z;
        bool isTerminal = (zPosition == 6 || zPosition == -6);
        return new State(isTerminal, zPosition);
    }

    public override void ExecuteAction(EnvironmentAction action)
    {
        action.Execute();
        _state = ComputeState();
    }

    public override List<EnvironmentAction> GetAvailableActions(State s)
    {
        var actions = new List<EnvironmentAction>();
        actions.Add(new EnvironmentAction(MoveForwardZ));
        actions.Add(new EnvironmentAction(MoveBackwardZ));
        return actions;
    }

    public void MoveForwardZ()
    {
        transform.position += new Vector3(0, 0, 1);
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void MoveBackwardZ()
    {
        transform.position += new Vector3(0, 0, -1);
        transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    public override void Initialise()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        _state = ComputeState();
    }

    public override Reward ObserveReward()
    {
        return new Reward(-1);
    }

}

