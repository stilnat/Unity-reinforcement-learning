using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSystemDynamics : MonoBehaviour
{

    public enum Action
    {
        up,
        right,
        down,
        left
    }

    public enum Reward
    {
        neg, 
        end
    }



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TestSystemDynamicCreationFromSingleDynamics() : " + TestSystemDynamicCreationFromSingleDynamics());
    }

    /// <summary>
    /// creates a state dynamic which must corresponds to the state dynamic manually created using CreateFourStateGridWorldDynamics()
    /// </summary>
    /// <returns></returns>
    private bool TestSystemDynamicCreationFromSingleDynamics()
    {
        List<SystemDynamic.SingleDynamic> singleDynamics = new List<SystemDynamic.SingleDynamic>();

        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Action.up, (int)Reward.end,0,1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Action.right, (int)Reward.end, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Action.down, (int)Reward.end, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Action.left, (int)Reward.end, 0, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Action.up, (int)Reward.neg, 1, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Action.right, (int)Reward.neg, 1, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Action.down, (int)Reward.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Action.left, (int)Reward.neg, 0, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Action.up, (int)Reward.neg, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Action.right, (int)Reward.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Action.down, (int)Reward.neg, 2, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Action.left, (int)Reward.neg, 2, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Action.up, (int)Reward.neg, 1, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Action.right, (int)Reward.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Action.down, (int)Reward.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Action.left, (int)Reward.neg, 2, 1));

        SystemDynamic systemDynamic = new SystemDynamic(singleDynamics);
        return SystemDynamic.AreEqualDynamics(systemDynamic, new SystemDynamic(TestHelpers.CreateFourStateGridWorldDynamicsMatrix()));
    }
}
