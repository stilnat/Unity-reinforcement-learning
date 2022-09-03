using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestHelpers
{

    public enum Action
    {
        up,
        right,
        down,
        left
    }

    public enum Rew
    {
        neg,
        end
    }

    public static float[,,,] CreateFourStateGridWorldDynamicsMatrix()
    {
        // simple example for a 4 case gridworld with reward -1 at each step except the terminal state.
        float[,,,] dynamicMatrix = new float[4, 2, 4, 4];

        dynamicMatrix[0, 0, 0, 0] = 0;
        dynamicMatrix[0, 0, 0, 1] = 0;
        dynamicMatrix[0, 0, 0, 2] = 0;
        dynamicMatrix[0, 0, 0, 3] = 0;

        dynamicMatrix[1, 0, 0, 0] = 0;
        dynamicMatrix[1, 0, 0, 1] = 0;
        dynamicMatrix[1, 0, 0, 2] = 0;
        dynamicMatrix[1, 0, 0, 3] = 0;

        dynamicMatrix[2, 0, 0, 0] = 0;
        dynamicMatrix[2, 0, 0, 1] = 0;
        dynamicMatrix[2, 0, 0, 2] = 0;
        dynamicMatrix[2, 0, 0, 3] = 0;

        dynamicMatrix[3, 0, 0, 0] = 0;
        dynamicMatrix[3, 0, 0, 1] = 0;
        dynamicMatrix[3, 0, 0, 2] = 0;
        dynamicMatrix[3, 0, 0, 3] = 0;



        dynamicMatrix[0, 0, 1, 0] = 0;
        dynamicMatrix[0, 0, 1, 1] = 0;
        dynamicMatrix[0, 0, 1, 2] = 0;
        dynamicMatrix[0, 0, 1, 3] = 1;

        dynamicMatrix[1, 0, 1, 0] = 1;
        dynamicMatrix[1, 0, 1, 1] = 1;
        dynamicMatrix[1, 0, 1, 2] = 0;
        dynamicMatrix[1, 0, 1, 3] = 0;

        dynamicMatrix[2, 0, 1, 0] = 0;
        dynamicMatrix[2, 0, 1, 1] = 0;
        dynamicMatrix[2, 0, 1, 2] = 0;
        dynamicMatrix[2, 0, 1, 3] = 0;

        dynamicMatrix[3, 0, 1, 0] = 0;
        dynamicMatrix[3, 0, 1, 1] = 0;
        dynamicMatrix[3, 0, 1, 2] = 1;
        dynamicMatrix[3, 0, 1, 3] = 0;



        dynamicMatrix[0, 0, 2, 0] = 1;
        dynamicMatrix[0, 0, 2, 1] = 0;
        dynamicMatrix[0, 0, 2, 2] = 0;
        dynamicMatrix[0, 0, 2, 3] = 0;

        dynamicMatrix[1, 0, 2, 0] = 0;
        dynamicMatrix[1, 0, 2, 1] = 0;
        dynamicMatrix[1, 0, 2, 2] = 0;
        dynamicMatrix[1, 0, 2, 3] = 0;

        dynamicMatrix[2, 0, 2, 0] = 0;
        dynamicMatrix[2, 0, 2, 1] = 0;
        dynamicMatrix[2, 0, 2, 2] = 1;
        dynamicMatrix[2, 0, 2, 3] = 1;

        dynamicMatrix[3, 0, 2, 0] = 0;
        dynamicMatrix[3, 0, 2, 1] = 1;
        dynamicMatrix[3, 0, 2, 2] = 0;
        dynamicMatrix[3, 0, 2, 3] = 0;



        dynamicMatrix[0, 0, 3, 0] = 0;
        dynamicMatrix[0, 0, 3, 1] = 0;
        dynamicMatrix[0, 0, 3, 2] = 0;
        dynamicMatrix[0, 0, 3, 3] = 0;

        dynamicMatrix[1, 0, 3, 0] = 1;
        dynamicMatrix[1, 0, 3, 1] = 0;
        dynamicMatrix[1, 0, 3, 2] = 0;
        dynamicMatrix[1, 0, 3, 3] = 0;

        dynamicMatrix[2, 0, 3, 0] = 0;
        dynamicMatrix[2, 0, 3, 1] = 0;
        dynamicMatrix[2, 0, 3, 2] = 0;
        dynamicMatrix[2, 0, 3, 3] = 1;

        dynamicMatrix[3, 0, 3, 0] = 0;
        dynamicMatrix[3, 0, 3, 1] = 1;
        dynamicMatrix[3, 0, 3, 2] = 1;
        dynamicMatrix[3, 0, 3, 3] = 0;


        //******************************************************************** //

        dynamicMatrix[0, 1, 0, 0] = 1;
        dynamicMatrix[0, 1, 0, 1] = 1;
        dynamicMatrix[0, 1, 0, 2] = 1;
        dynamicMatrix[0, 1, 0, 3] = 1;

        dynamicMatrix[1, 1, 0, 0] = 0;
        dynamicMatrix[1, 1, 0, 1] = 0;
        dynamicMatrix[1, 1, 0, 2] = 0;
        dynamicMatrix[1, 1, 0, 3] = 0;

        dynamicMatrix[2, 1, 0, 0] = 0;
        dynamicMatrix[2, 1, 0, 1] = 0;
        dynamicMatrix[2, 1, 0, 2] = 0;
        dynamicMatrix[2, 1, 0, 3] = 0;

        dynamicMatrix[3, 1, 0, 0] = 0;
        dynamicMatrix[3, 1, 0, 1] = 0;
        dynamicMatrix[3, 1, 0, 2] = 0;
        dynamicMatrix[3, 1, 0, 3] = 0;



        dynamicMatrix[0, 1, 1, 0] = 0;
        dynamicMatrix[0, 1, 1, 1] = 0;
        dynamicMatrix[0, 1, 1, 2] = 0;
        dynamicMatrix[0, 1, 1, 3] = 0;

        dynamicMatrix[1, 1, 1, 0] = 0;
        dynamicMatrix[1, 1, 1, 1] = 0;
        dynamicMatrix[1, 1, 1, 2] = 0;
        dynamicMatrix[1, 1, 1, 3] = 0;

        dynamicMatrix[2, 1, 1, 0] = 0;
        dynamicMatrix[2, 1, 1, 1] = 0;
        dynamicMatrix[2, 1, 1, 2] = 0;
        dynamicMatrix[2, 1, 1, 3] = 0;

        dynamicMatrix[3, 1, 1, 0] = 0;
        dynamicMatrix[3, 1, 1, 1] = 0;
        dynamicMatrix[3, 1, 1, 2] = 0;
        dynamicMatrix[3, 1, 1, 3] = 0;



        dynamicMatrix[0, 1, 2, 0] = 0;
        dynamicMatrix[0, 1, 2, 1] = 0;
        dynamicMatrix[0, 1, 2, 2] = 0;
        dynamicMatrix[0, 1, 2, 3] = 0;

        dynamicMatrix[1, 1, 2, 0] = 0;
        dynamicMatrix[1, 1, 2, 1] = 0;
        dynamicMatrix[1, 1, 2, 2] = 0;
        dynamicMatrix[1, 1, 2, 3] = 0;

        dynamicMatrix[2, 1, 2, 0] = 0;
        dynamicMatrix[2, 1, 2, 1] = 0;
        dynamicMatrix[2, 1, 2, 2] = 0;
        dynamicMatrix[2, 1, 2, 3] = 0;

        dynamicMatrix[3, 1, 2, 0] = 0;
        dynamicMatrix[3, 1, 2, 1] = 0;
        dynamicMatrix[3, 1, 2, 2] = 0;
        dynamicMatrix[3, 1, 2, 3] = 0;



        dynamicMatrix[0, 1, 3, 0] = 0;
        dynamicMatrix[0, 1, 3, 1] = 0;
        dynamicMatrix[0, 1, 3, 2] = 0;
        dynamicMatrix[0, 1, 3, 3] = 0;

        dynamicMatrix[1, 1, 3, 0] = 0;
        dynamicMatrix[1, 1, 3, 1] = 0;
        dynamicMatrix[1, 1, 3, 2] = 0;
        dynamicMatrix[1, 1, 3, 3] = 0;

        dynamicMatrix[2, 1, 3, 0] = 0;
        dynamicMatrix[2, 1, 3, 1] = 0;
        dynamicMatrix[2, 1, 3, 2] = 0;
        dynamicMatrix[2, 1, 3, 3] = 0;

        dynamicMatrix[3, 1, 3, 0] = 0;
        dynamicMatrix[3, 1, 3, 1] = 0;
        dynamicMatrix[3, 1, 3, 2] = 0;
        dynamicMatrix[3, 1, 3, 3] = 0;

        return dynamicMatrix;

    }

    public static List<SystemDynamic.SingleDynamic> CreateSixteenStateGridWorldDynamicsListSingleDynamics()
    {
        List<SystemDynamic.SingleDynamic> singleDynamics = new List<SystemDynamic.SingleDynamic>();

        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Action.up, (int)Rew.end, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Action.right, (int)Rew.end, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Action.down, (int)Rew.end, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(0, (int)Action.left, (int)Rew.end, 0, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Action.up, (int)Rew.neg, 1, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Action.right, (int)Rew.neg, 2, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Action.down, (int)Rew.neg, 5, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(1, (int)Action.left, (int)Rew.neg, 0, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Action.up, (int)Rew.neg, 2, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Action.right, (int)Rew.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Action.down, (int)Rew.neg, 6, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(2, (int)Action.left, (int)Rew.neg, 1, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Action.up, (int)Rew.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Action.right, (int)Rew.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Action.down, (int)Rew.neg, 7, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(3, (int)Action.left, (int)Rew.neg, 2, 1));

        //**********************************************************************************************//

        singleDynamics.Add(new SystemDynamic.SingleDynamic(4, (int)Action.up, (int)Rew.neg, 0, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(4, (int)Action.right, (int)Rew.neg, 5, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(4, (int)Action.down, (int)Rew.neg, 8, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(4, (int)Action.left, (int)Rew.neg, 4, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(5, (int)Action.up, (int)Rew.neg, 1, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(5, (int)Action.right, (int)Rew.neg, 6, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(5, (int)Action.down, (int)Rew.neg, 9, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(5, (int)Action.left, (int)Rew.neg, 4, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(6, (int)Action.up, (int)Rew.neg, 2, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(6, (int)Action.right, (int)Rew.neg, 7, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(6, (int)Action.down, (int)Rew.neg, 10, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(6, (int)Action.left, (int)Rew.neg, 5, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(7, (int)Action.up, (int)Rew.neg, 3, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(7, (int)Action.right, (int)Rew.neg, 7, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(7, (int)Action.down, (int)Rew.neg, 11, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(7, (int)Action.left, (int)Rew.neg, 6, 1));

        //**********************************************************************************************//

        singleDynamics.Add(new SystemDynamic.SingleDynamic(8, (int)Action.up, (int)Rew.neg, 4, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(8, (int)Action.right, (int)Rew.neg, 9, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(8, (int)Action.down, (int)Rew.neg, 12, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(8, (int)Action.left, (int)Rew.neg, 8, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(9, (int)Action.up, (int)Rew.neg, 5, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(9, (int)Action.right, (int)Rew.neg, 10, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(9, (int)Action.down, (int)Rew.neg, 13, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(9, (int)Action.left, (int)Rew.neg, 8, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(10, (int)Action.up, (int)Rew.neg, 6, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(10, (int)Action.right, (int)Rew.neg, 11, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(10, (int)Action.down, (int)Rew.neg, 14, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(10, (int)Action.left, (int)Rew.neg, 9, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(11, (int)Action.up, (int)Rew.neg, 7, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(11, (int)Action.right, (int)Rew.neg, 11, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(11, (int)Action.down, (int)Rew.neg, 15, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(11, (int)Action.left, (int)Rew.neg, 10, 1));

        //**********************************************************************************************//

        singleDynamics.Add(new SystemDynamic.SingleDynamic(12, (int)Action.up, (int)Rew.neg, 8, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(12, (int)Action.right, (int)Rew.neg, 13, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(12, (int)Action.down, (int)Rew.neg, 12, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(12, (int)Action.left, (int)Rew.neg, 12, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(13, (int)Action.up, (int)Rew.neg, 9, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(13, (int)Action.right, (int)Rew.neg, 14, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(13, (int)Action.down, (int)Rew.neg, 13, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(13, (int)Action.left, (int)Rew.neg, 12, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(14, (int)Action.up, (int)Rew.neg, 10, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(14, (int)Action.right, (int)Rew.neg, 15, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(14, (int)Action.down, (int)Rew.neg, 14, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(14, (int)Action.left, (int)Rew.neg, 13, 1));

        singleDynamics.Add(new SystemDynamic.SingleDynamic(15, (int)Action.up, (int)Rew.end, 15, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(15, (int)Action.right, (int)Rew.end, 15, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(15, (int)Action.down, (int)Rew.end, 15, 1));
        singleDynamics.Add(new SystemDynamic.SingleDynamic(15, (int)Action.left, (int)Rew.end, 15, 1));

        //**********************************************************************************************//

        return singleDynamics;
    }
}
