using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// maybe computing state should happen right after taking the action ? Could make more sense for the last reward.
/// 
/// </summary>
public class Cartpole2DAgent : Agent
{

    public GameObject _pole;
    private int _roughStateSpaceMeshAngle;
    private int _roughStateSpaceMeshDistance;
    private float _groundLevel;
    private Vector3 _initialPosition;
    private float push;
    private float _lastTimeInitialise;
    public int _nFrame;
    public int _updateCount;
    private ArticulationBody _articulationBody;

    private void Awake()
    {
        _roughStateSpaceMeshAngle = 2;
        _roughStateSpaceMeshDistance = 25;
        push = 3000f;
        _groundLevel = 0;
        _initialPosition = gameObject.transform.position;
        _articulationBody = GetComponent<ArticulationBody>();
        _state = ComputeState();
        _lastTimeInitialise = Time.realtimeSinceStartup;
        _nFrame = 10;
        
    }

    private void Start()
    {
        _pole.GetComponent<ArticulationBody>().AddForce(-transform.right * 50);
    }

    private void FixedUpdate()
    {
        if (_updateCount % _nFrame == 0)
        {
            _state = ComputeState();
        }
        _updateCount += 1;

    }

    private float DistanceToCenter()
    {
        return Vector3.Distance(_initialPosition, gameObject.transform.position);
    }

    public override State ComputeState()
    {

        int eulerAngleX = (int)_pole.transform.rotation.eulerAngles.x;
        int eulerAngleY = (int)_pole.transform.rotation.eulerAngles.y;
        int eulerAngleZ = (int)_pole.transform.rotation.eulerAngles.z;

        int movement = (int) _articulationBody.velocity.z;
        int poleAngularVelocity = (int)_pole.GetComponent<ArticulationBody>().angularVelocity.x;

        bool isTerminal;


        if ((eulerAngleX >= 50 && eulerAngleX <= 100) || (eulerAngleX >= 260 && eulerAngleX <= 310) ||
            (eulerAngleZ >= 50 && eulerAngleZ <= 100) || (eulerAngleZ >= 260 && eulerAngleZ <= 310))
        {
            isTerminal = true;
        }
        else
        {
            isTerminal = false;
        }

        if (this.transform.position.y < _groundLevel)
        {
            isTerminal = true;
        }

        if (eulerAngleX > 310)
        {
            eulerAngleX = eulerAngleX - 360;
        }

        eulerAngleX = eulerAngleX / _roughStateSpaceMeshAngle;

        

        return new State(isTerminal, eulerAngleX, movement, poleAngularVelocity);
    }

    public void ForwardAcceleration()
    {
        //Debug.Log("forward");
        var articulationBody = GetComponent<ArticulationBody>();
        articulationBody.AddForce(transform.forward * push);
    }

    public void ZeroAcceleration()
    {

    }

    public void BackwardAcceleration()
    {
        //Debug.Log("backward");
        var articulationBody = GetComponent<ArticulationBody>();
        articulationBody.AddForce(-transform.forward * push);
    }

    public override List<EnvironmentAction> GetAvailableActions(State s)
    {
        var listAction = new List<EnvironmentAction>();
        listAction.Add(new EnvironmentAction(ForwardAcceleration));
        listAction.Add(new EnvironmentAction(BackwardAcceleration));
        listAction.Add(new EnvironmentAction(ZeroAcceleration));
        return listAction;
    }

    public override Reward ObserveReward()
    {
        if (_state.IsTerminal) return new Reward(0);
        else return new Reward(1);
    }

    public override void ExecuteAction(EnvironmentAction action)
    {
        action.Execute();
    }

    public override void Initialise()
    {
        this.gameObject.GetComponent<ArticulationBody>().enabled = false;
        this.gameObject.transform.position = new Vector3(0, 1f, 0);
        _pole.transform.rotation = Quaternion.identity;
        this.gameObject.transform.rotation = Quaternion.identity;
        _pole.transform.localPosition = new Vector3(0, 5.1f, 0);
        this.gameObject.GetComponent<ArticulationBody>().enabled = true;
        _lastTimeInitialise = Time.realtimeSinceStartup;
        _pole.GetComponent<ArticulationBody>().AddForce(transform.forward * 50);
    }
}
