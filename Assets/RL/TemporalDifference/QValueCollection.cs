using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QValueCollection : 
    Dictionary<State, Dictionary<EnvironmentAction, StateActionValue>>, ISerializationCallbackReceiver
{

    [SerializeField]
    private List<State> KeyStates = new List<State>();

    [SerializeField]
    private List<StateActionValue> values = new List<StateActionValue>();

    [SerializeField]
    private List<EnvironmentAction> KeyEnvironmentActions = new List<EnvironmentAction>();

    public QValueCollection()
    {

    }


    public void OnBeforeSerialize()
    {
        KeyStates.Clear();
        values.Clear();
        KeyEnvironmentActions.Clear();
        foreach (KeyValuePair<State, Dictionary<EnvironmentAction, StateActionValue>> pair in this)
        {
            KeyStates.Add(pair.Key);
            foreach(KeyValuePair<EnvironmentAction, StateActionValue> innerPair in pair.Value)
            {
                values.Add(innerPair.Value);
                KeyEnvironmentActions.Add(innerPair.Key);
            }
           
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        
        for (int j = 0; j < KeyStates.Count; j++)
        {
            var dic = new Dictionary<EnvironmentAction, StateActionValue>();
            for (int i = 0; i < KeyEnvironmentActions.Count; i++)
            {
                dic.Add(KeyEnvironmentActions[i], values[i]);
            }
            this.Add(KeyStates[j], dic);
        }
    }

    public static QValueCollection CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<QValueCollection>(jsonString);
    }

}
