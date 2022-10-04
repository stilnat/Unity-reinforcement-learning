using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerEnvironment : MonoBehaviour
{
    
    void Start()
    {
        Mesh planeMesh = GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyEnvironment(Agent agent)
    {
        AddMovementFromWind(agent);
    }

    private void AddMovementFromWind(Agent agent)
    {
        int[] winds = new int[10] { 0, 0, 0, 1, 1, 1, 2, 2, 1, 0 };
        var wind = new Vector3(winds[(int)agent.gameObject.transform.position.z], 0, 0);
        if (agent.gameObject.transform.position.x - wind.x < 0)
        {
            agent.gameObject.transform.position = new Vector3(0.5f, agent.gameObject.transform.position.y, agent.gameObject.transform.position.z);
        }
        else agent.gameObject.transform.position -= wind;
    }


}
