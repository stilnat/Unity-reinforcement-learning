using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAgent : Agent
{

    public GameObject platform;
    public int _updateCount =0;
    public int _nFrame;

    public void Start()
    {
        Mesh planeMesh = platform.GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;
        Debug.Log(bounds.size.x);
        Debug.Log(bounds.size.z);
        _state = ComputeState();
    }

    public void FixedUpdate()
    {
        _updateCount += 1;
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
        AddMovementFromWind();
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

    public void AddMovementFromWind()
    {
        int[] winds = new int[10] { 0,0,0,1,1,1,2,2,1,0};
        var wind = new Vector3(winds[(int)gameObject.transform.position.z], 0, 0);
        if (gameObject.transform.position.x - wind.x < 0)
        {
            gameObject.transform.position = new Vector3(0.5f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else gameObject.transform.position -= wind;
    }


}
