using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAgent : Agent
{

    public GameObject platform;

    public void Start()
    {
        Mesh planeMesh = platform.GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;
        Debug.Log(bounds.size.x);
        Debug.Log(bounds.size.z);
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
        throw new System.NotImplementedException();
    }

    public override List<EnvironmentAction> GetAvailableActions(State s)
    {
        throw new System.NotImplementedException();
    }

    public void moveForwardZ()
    {
        transform.position += new Vector3(0, 0, 1);
    }

    public void moveBackwardZ()
    {
        transform.position += new Vector3(0, 0, -1);
    }

    public void moveForwardX()
    {
        transform.position += new Vector3(1, 0, 0);
    }

    public void moveBackwardX()
    {
        transform.position += new Vector3(-1, 0, 0);
    }

    public override void Initialise()
    {
        gameObject.transform.position = new Vector3(3.5f, 0.3f, 0.5f);
    }

    public override Reward ObserveReward()
    {
        return new Reward(-1);
    }


}
