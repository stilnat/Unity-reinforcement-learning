using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class TestEnvironmentPolicy
{
    [Test]
    public void TestChooseActionEpsilonGreedy()
    {
        var state = new State(false, 0);
        float eps = 0.3f;
        var environmentPolicy = new EnvironmentPolicy();
        var stateActionValues = new Dictionary<EnvironmentAction, StateActionValue>();
        stateActionValues.Add(new EnvironmentAction(ActionOne), new StateActionValue(10));
        stateActionValues.Add(new EnvironmentAction(ActionTwo), new StateActionValue(5));
        stateActionValues.Add(new EnvironmentAction(ActionThree), new StateActionValue(7));
        EnvironmentAction actionChosen;

       
        var freq = new float[3] { 0, 0, 0 };
        int count = 10000;
        for(int i = 0; i<count; i++)
        {
            actionChosen = environmentPolicy.ChooseActionEpsilonGreedy(state, stateActionValues, eps);

            if (actionChosen._actionToDoNoParameters.Method.Name == "ActionOne")
            {
                freq[0] += 1;
            }
            else if(actionChosen._actionToDoNoParameters.Method.Name == "ActionTwo")
            {
                freq[1] += 1;
            }
            else if (actionChosen._actionToDoNoParameters.Method.Name == "ActionThree")
            {
                freq[2] += 1;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            freq[i] = freq[i] / count;
        }

        float error = 0.01f;

        Assert.IsTrue(freq[0] <= (1 - eps + eps / 3 + error) && freq[0] >= (1 - eps + eps / 3 - error) && freq[1] <= eps/3 + error && freq[1] >= eps / 3 - error
            && freq[2] <= eps / 3 + error && freq[2] >= eps / 3 - error);

    }

    private void ActionOne() { }
    private void ActionTwo() { }
    private void ActionThree() { }
}
