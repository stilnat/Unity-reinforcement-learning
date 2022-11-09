using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartPoleAgent: Agent
{
   
    public GameObject _pole;
    private int _roughStateSpaceMeshAngle;
    private int _roughStateSpaceMeshDistance;
    private float _groundLevel;
    private Vector3 _initialPosition ;
    private float push;
    private float _lastTimeInitialise;
    public int _nFrame;
    public int _updateCount;

    private void Awake()
    {
        _roughStateSpaceMeshAngle = 10;
        _roughStateSpaceMeshDistance = 25;
        push = 150f;
        _groundLevel = 0;
        _initialPosition = gameObject.transform.position;
        _state = ComputeState();
        _lastTimeInitialise = Time.realtimeSinceStartup;
        _nFrame = 25;
    }

    private void Start()
    {
        _pole.GetComponent<ArticulationBody>().AddForce(-transform.right * 50);
    }

    private void FixedUpdate()
    {
        if (_updateCount % _nFrame ==0)
        {
            _state = ComputeState();
        }
        _nFrame += 1;
        
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

        int distance =  (int) DistanceToCenter() / _roughStateSpaceMeshDistance;
        bool isTerminal;


        if ((eulerAngleX >= 80 && eulerAngleX <= 100) || (eulerAngleX >= 260 && eulerAngleX <= 280) ||
            (eulerAngleZ >= 80 && eulerAngleZ <= 100) || (eulerAngleZ >= 260 && eulerAngleZ <= 280))
        {
            isTerminal = true;
        }
        else
        {
            isTerminal = false;
        }

        if(this.transform.position.y < _groundLevel)
        {
            isTerminal = true;
        }

        eulerAngleX = eulerAngleX / _roughStateSpaceMeshAngle;
        eulerAngleY = eulerAngleY / _roughStateSpaceMeshAngle;
        eulerAngleZ = eulerAngleZ / _roughStateSpaceMeshAngle;

        return new State(isTerminal, eulerAngleX, eulerAngleY, eulerAngleZ, distance);
    }

    public void RightAcceleration()
    {
        var articulationBody = GetComponent<ArticulationBody>();
        articulationBody.AddForce(transform.right * push);
    }

    public void LeftAcceleration()
    {
        var articulationBody = GetComponent<ArticulationBody>();
        articulationBody.AddForce(-transform.right * push);
    }

    public void ForwardAcceleration()
    {
        var articulationBody = GetComponent<ArticulationBody>();
        articulationBody.AddForce(transform.forward * push);
    }

    public void BackwardAcceleration()
    {
        var articulationBody = GetComponent<ArticulationBody>();
        articulationBody.AddForce(-transform.forward * push);
    }

    public override List<EnvironmentAction> GetAvailableActions(State s)
    {
        var listAction = new List<EnvironmentAction>();
        listAction.Add(new EnvironmentAction(ForwardAcceleration));
        listAction.Add(new EnvironmentAction(BackwardAcceleration));
        listAction.Add(new EnvironmentAction(RightAcceleration));
        listAction.Add(new EnvironmentAction(LeftAcceleration));

        return listAction;
    }

    public override Reward ObserveReward()
    {
        return new Reward(1);
        
    }

    public override void ExecuteAction(EnvironmentAction action)
    {
        action.Execute();
    }

    public override void Initialise()
    {

        this.gameObject.GetComponent<ArticulationBody>().enabled = false;
        
        this.gameObject.transform.position = new Vector3(0, 1f, 0);
        _pole.transform.rotation =  Quaternion.identity;
        this.gameObject.transform.rotation = Quaternion.identity;
        _pole.transform.localPosition = new Vector3(0, 5.1f, 0);
        this.gameObject.GetComponent<ArticulationBody>().enabled = true;
        _lastTimeInitialise = Time.realtimeSinceStartup;
        _pole.GetComponent<ArticulationBody>().AddForce(-transform.right * 50);
    }










}
