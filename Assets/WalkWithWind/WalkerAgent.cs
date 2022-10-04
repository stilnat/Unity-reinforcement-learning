using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAgent : Agent
{

    public WalkerEnvironment environment;

    public void Start()
    {
        _state = ComputeState();
    }

    public override State ComputeState()
    {
        int xPosition = (int)transform.position.x;
        int zPosition = (int)transform.position.z;
        bool isTerminal = (xPosition == 3 && zPosition == 7);
        return new State(isTerminal, xPosition, zPosition);
    }

    public override void ExecuteAction(EnvironmentAction action)
    {
        action.Execute();
        environment.ApplyEnvironment(this);
        _state = ComputeState();
    }

    public override List<EnvironmentAction> GetAvailableActions(State s)
    {
        var actions = new List<EnvironmentAction>();
        if (CanMoveForwardZ(s)) actions.Add(new EnvironmentAction(MoveForwardZ));
        if (CanMoveBackwardZ(s)) actions.Add(new EnvironmentAction(MoveBackwardZ));
        if (CanMoveForwardX(s)) actions.Add(new EnvironmentAction(MoveForwardX));
        if (CanMoveBackwardX(s)) actions.Add(new EnvironmentAction(MoveBackwardX));
        return actions;
    }

    private bool CanMoveForwardZ(State s)
    {
        return s.Get(1) < 9;
    }

    private bool CanMoveBackwardZ(State s)
    {
        return s.Get(1) > 0;
    }

    private bool CanMoveForwardX(State s)
    {
        return s.Get(0) < 6;
    }

    private bool CanMoveBackwardX(State s)
    {
        return s.Get(0) > 0;
    }

    //TODO that should not be a thing ....
    public override List<EnvironmentAction> GetAvailableActions()
    {
        return GetAvailableActions(new State(false,0,0));
    }

    public void MoveForwardZ()
    {
        transform.position += new Vector3(0, 0, 1);
    }

    public void MoveBackwardZ()
    {
        transform.position += new Vector3(0, 0, -1);
    }

    public void MoveForwardX()
    {
        transform.position += new Vector3(1, 0, 0);
    }

    public void MoveBackwardX()
    {
        transform.position += new Vector3(-1, 0, 0);
    }

    public override void Initialise()
    {
        gameObject.transform.position = new Vector3(3.5f, 0.3f, 0.5f);
        _state = ComputeState();
    }

    public override Reward ObserveReward()
    {
        return new Reward(-1);
    }




}
