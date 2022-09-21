using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestHelper
{
    /// <summary>
    /// n must be odd.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static MCSystemDynamic GenerateRandomWalk(int n)
    {
        if(n%2 == 0)
        {
             throw new System.ArgumentException(System.String.Format("{0} is not an odd number", n),"n");
        }

        if (n < 3)
        {
            throw new System.ArgumentException(System.String.Format("{0} is too small, not enough states", n), "n");
        }

        List<State> states = new List<State>();
        states.Add(new State(0.ToString(), true, 0));
        for(int i = 1; i<n-1; i++)
        {
            states.Add(new State(i.ToString(), false, i));
        }
        states.Add(new State((n-1).ToString(), true, n-1));

        Reward reward = new Reward(-1);

        Action left = new Action("left",0);
        Action right = new Action("right",1);

        MCSystemDynamic systemDynamic = new MCSystemDynamic();

        foreach(State state in states)
        {
            if (!state.IsTerminal)
            {
                systemDynamic.AddDynamic(new SingleActionStateDynamic(state, right, reward, states[states.IndexOf(state) + 1], 1));
                systemDynamic.AddDynamic(new SingleActionStateDynamic(state, left, reward, states[states.IndexOf(state) - 1], 1));
            }
            
        }

        return systemDynamic;



    }
    public static MCSystemDynamic GenerateSimpleDynamicWithLoop()
    {
        State a = new State("a", false, 0);
        State b = new State("b", false, 1);
        State c = new State("c", false, 2);
        State d = new State("d", false, 3);
        State e = new State("e", true, 4);

        Reward ac = new Reward(2);
        Reward cd = new Reward(-10);
        Reward de = new Reward(-2);
        Reward ad = new Reward(-3);
        Reward ab1 = new Reward(-5);
        Reward ab2 = new Reward(-1);
        Reward ba = new Reward(1);

        Action AToC = new Action("AToC", 0);
        Action AToBOrD = new Action("AToBOrD", 1);
        Action CToD = new Action("CToD", 2);
        Action DToE = new Action("DToE", 3);
        Action BToA = new Action("BToA", 4);

        MCSystemDynamic systemDynamic = new MCSystemDynamic();
        systemDynamic.AddDynamic(new SingleActionStateDynamic(a, AToC, ac, c, 1));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(c, CToD, cd, d, 1));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(d, DToE, de, e, 1));

        systemDynamic.AddDynamic(new SingleActionStateDynamic(a, AToBOrD, ad, d, 0.5f));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(a, AToBOrD, ab1, b, 0.1f));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(a, AToBOrD, ab2, b, 0.4f));

        systemDynamic.AddDynamic(new SingleActionStateDynamic(b, BToA, ba, a, 1));

        return systemDynamic;
    }

    public static MCSystemDynamic GenerateSimpleDynamic()
    {
        State a = new State("a", false, 0);
        State b = new State("b", false, 1);
        State c = new State("c", false, 2);
        State d = new State("e", true, 4);


        Action AToC = new Action("AToC", 0);
        Action AToBOrD = new Action("AToBOrD", 1);
        Action BToC = new Action("BToC", 2);
        Action CToD = new Action("CToD", 3);

        Reward ac = new Reward(2);
        Reward ad = new Reward(-3);
        Reward ab1 = new Reward(-5);
        Reward ab2 = new Reward(-1);
        Reward bc1 = new Reward(-1);
        Reward bc2 = new Reward(-6);
        Reward cd = new Reward(-2);



        MCSystemDynamic systemDynamic = new MCSystemDynamic();
        systemDynamic.AddDynamic(new SingleActionStateDynamic(a, AToC, ac, c, 1));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(a, AToBOrD, ab1, b, 0.05f));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(a, AToBOrD, ab2, b, 0.45f));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(a, AToBOrD, ad, d, 0.5f));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(b, BToC, bc1, c, 0.5f));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(b, BToC, bc2, c, 0.5f));
        systemDynamic.AddDynamic(new SingleActionStateDynamic(c, CToD, cd, d, 1));

        return systemDynamic;
    }
}
