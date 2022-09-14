using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestHelper
{
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
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToC, ac, c, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(c, CToD, cd, d, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(d, DToE, de, e, 1));

        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ad, d, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab1, b, 0.1f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab2, b, 0.4f));

        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(b, BToA, ba, a, 1));

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
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToC, ac, c, 1));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab1, b, 0.05f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ab2, b, 0.45f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(a, AToBOrD, ad, d, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(b, BToC, bc1, c, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(b, BToC, bc2, c, 0.5f));
        systemDynamic.AddDynamic(new MCSystemDynamic.SingleActionStateDynamic(c, CToD, cd, d, 1));

        return systemDynamic;
    }
}
