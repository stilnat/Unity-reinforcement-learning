using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartPoleState : MonoBehaviour
{
   
    public GameObject _pole;

    private State _state;
    private int fixed_update_number;
    private int _roughStateSpaceMesh;
    private float _groundLevel;
    private Vector3 _initialPosition ;
    private float _totalReward;
    private bool _showTotalReward;

    // Start is called before the first frame update
    void Start()
    {
       
        _roughStateSpaceMesh = 5;
        _groundLevel = 0;
        _initialPosition = gameObject.transform.position;
        _totalReward = 0;
        fixed_update_number = 0;
        _showTotalReward = true;
        _state = computeState();

    }

    private void FixedUpdate()
    {
        
        if(fixed_update_number % 10 == 0)
        {
            _state = computeState();
            _totalReward += ComputeReward().Value;


            if (_state.IsTerminal && _showTotalReward)
            {
                Debug.Log("total reward = " + _totalReward);
                _showTotalReward = false;
            }
        }


        
        fixed_update_number += 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float DistanceToCenter()
    { 
        return Vector3.Distance(_initialPosition, gameObject.transform.position);
    }

    public State computeState()
    {
        int eulerAngleX = (int)_pole.transform.rotation.eulerAngles.x; 
        int eulerAngleY = (int)_pole.transform.rotation.eulerAngles.y;
        int eulerAngleZ = (int)_pole.transform.rotation.eulerAngles.z;

       // Debug.Log(eulerAngleX + "," + eulerAngleY + "," + eulerAngleZ);
        int distance =  (int) DistanceToCenter() / _roughStateSpaceMesh;
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

        eulerAngleX = eulerAngleX / _roughStateSpaceMesh;
        eulerAngleY = eulerAngleY / _roughStateSpaceMesh;
        eulerAngleZ = eulerAngleZ / _roughStateSpaceMesh;

        return new State(isTerminal, eulerAngleX, eulerAngleY, eulerAngleZ, distance);
    }

    public Action RightAcceleration()
    { 

    }

    public Action LeftAcceleration()
    {

    }

    public Action ForwardAcceleration()
    {

    }

    public BackwardAcceleration()
    {

    }

    public Reward ComputeReward()
    {
        return new Reward(-1);
    }

    





  
}
