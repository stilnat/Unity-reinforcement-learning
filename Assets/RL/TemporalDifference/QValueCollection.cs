using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// The serialization completely fails, it's very visible in the walkwithwind environment ....
/// try to stop and start again, you'll get terribly false value.
/// </summary>
[System.Serializable]
public class QValueCollection : 
    Dictionary<State, Dictionary<EnvironmentAction, StateActionValue>>, ISerializationCallbackReceiver
{
    
    [System.Serializable]
    private class QValueCollectionInfo
    {
        [SerializeField]
        public List<State> KeyStates = new List<State>();

        [SerializeField]
        public List<StateActionValue> values = new List<StateActionValue>();

        [SerializeField]
        public List<string> KeyEnvironmentActionsName = new List<string>();

        [SerializeField]
        public List<int> ActionNumberPerState = new List<int>();
    }

    [SerializeField]
    private QValueCollectionInfo info = new QValueCollectionInfo();

    public QValueCollection()
    {

    }


    public void OnBeforeSerialize()
    {
        info.KeyStates.Clear();
        info.values.Clear();
        info.KeyEnvironmentActionsName.Clear();
        foreach (KeyValuePair<State, Dictionary<EnvironmentAction, StateActionValue>> pair in this)
        {
            info.KeyStates.Add(pair.Key);
            info.ActionNumberPerState.Add(pair.Value.Count);
            foreach(KeyValuePair<EnvironmentAction, StateActionValue> innerPair in pair.Value)
            {
                info.values.Add(innerPair.Value);
                var name = innerPair.Key._actionToDoNoParameters.Method.Name;
                info.KeyEnvironmentActionsName.Add(name);
            }
           
        }
    }

    public void OnAfterDeserialize()
    {

    }

    public void infoToCollection(Agent agent)
    {
        var type = agent.GetType();

        for (int j = 0; j < info.KeyStates.Count; j++)
        {
            var dic = new Dictionary<EnvironmentAction, StateActionValue>();
            for (int i = j; i < j+info.ActionNumberPerState[j]; i++)
            {
                string name = type.GetMethod(info.KeyEnvironmentActionsName[i]).Name;
                System.Action action = (System.Action) Delegate.CreateDelegate(typeof(System.Action), agent, name);
                dic.Add(new EnvironmentAction(action), info.values[i]);
            }
            this.Add(info.KeyStates[j], dic);
        }
    }

    public static QValueCollection CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<QValueCollection>(jsonString);
    }

    public override string ToString()
    {
        string str = "";
        foreach (KeyValuePair<State, Dictionary<EnvironmentAction, StateActionValue>> pair in this)
        {
           
            foreach (KeyValuePair<EnvironmentAction, StateActionValue> innerPair in pair.Value)
            {
                str += "State = " + pair.Key.ToString() + " : Action = " + innerPair.Key.ToString() + " Value = " + innerPair.Value.ToString() + " . \n";
            }

        }

        return str;
    }

}
