using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestSystemDynamics
{
    public enum Actions
    {
        up,
        right,
        down,
        left
    }

    public enum Rewards
    {
        neg,
        end
    }

    [Test]
    public void TestSystemDynamicCreationFromSingleDynamics()
    {
        List<SystemDynamic.SingleDynamic> singleDynamics = new List<SystemDynamic.SingleDynamic>();

        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Actions.up, (int)Rewards.end, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Actions.right, (int)Rewards.end, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Actions.down, (int)Rewards.end, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Actions.left, (int)Rewards.end, 0, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Actions.up, (int)Rewards.neg, 1, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Actions.right, (int)Rewards.neg, 1, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Actions.down, (int)Rewards.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Actions.left, (int)Rewards.neg, 0, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Actions.up, (int)Rewards.neg, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Actions.right, (int)Rewards.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Actions.down, (int)Rewards.neg, 2, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Actions.left, (int)Rewards.neg, 2, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Actions.up, (int)Rewards.neg, 1, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Actions.right, (int)Rewards.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Actions.down, (int)Rewards.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Actions.left, (int)Rewards.neg, 2, 1));

        SystemDynamic systemDynamic = new SystemDynamic(singleDynamics);
        Assert.IsTrue(SystemDynamic.AreEqualDynamics(systemDynamic, new SystemDynamic(TestHelpers.CreateFourStateGridWorldDynamicsMatrix())));
    }


    [Test]
    public void TestSystemDynamicCreationFromSingleActionStatesDynamics()
    {
        var singleDynamics = new List<SystemDynamic.SingleActionStateDynamic>();

        var states = new List<State>();
        for(int i=0; i<4; i++)
        {
            states.Add(new State(i));
        }
        var up = new Action(0);
        var right = new Action(1);
        var down = new Action(2);
        var left = new Action(3);

        var neg = new Reward(-1);
        var end = new Reward(-1);



        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[0], up, end, states[0], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[0], right, end, states[0], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[0], down, end, states[0], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[0], left, end, states[0], 1));

        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[1], up, neg, states[1], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[1], right, neg, states[1], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[1], down, neg, states[3], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[1], left, neg, states[0], 1));

        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[2], up, neg, states[0], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[2], right, neg, states[3], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[2], down, neg, states[2], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[2], left, neg, states[2], 1));

        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[3], up, neg, states[1], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[3], right, neg, states[3], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[3], down, neg, states[3], 1));
        singleDynamics.Add(new SystemDynamic.SingleActionStateDynamic(states[3], left, neg, states[2], 1));

        SystemDynamic systemDynamic = new SystemDynamic(singleDynamics);
        SystemDynamic testDynamic = new SystemDynamic(TestHelpers.CreateFourStateGridWorldDynamicsMatrix());
        Assert.IsTrue(SystemDynamic.AreEqualDynamics(systemDynamic, testDynamic));
    }

}
