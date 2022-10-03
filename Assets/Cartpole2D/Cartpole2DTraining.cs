using System.IO;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Cartpole2DTraining : MonoBehaviour
{

    private Agent _agent;
    private QValueCollection _actionStateValue;
    private EnvironmentPolicy _policyToLearn;
    private EnvironmentPolicy _policyToFollow;
    private Reward _reward;
    public bool render;
    public float timeScale;
    public int maxNumberOfEpisode;

    public float learningRate = 0.9f;
    public float learningRateMultiplier = 0.998f;
    public float learningRateMinimum = 0.01f;
    public float epsilon = 1f;
    public float epsilonMultiplier = 0.999f;
    public float epsilonMinimum = 0.05f;

    [SerializeField]
    private List<float> CumulativeRewardEpisodes;

    private int episodeCounter = 0;

    public void disableRenderOnGameObjectAndChilds(GameObject gameObject)
    {
        if (gameObject.GetComponent<Renderer>() != null)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        foreach (Transform child in gameObject.transform)
        {
            disableRenderOnGameObjectAndChilds(child.gameObject);
        }
    }

    public void disableAllRenderer()
    {
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
        {
            disableRenderOnGameObjectAndChilds(obj);
        }
    }

    void Awake()
    {
        if (Application.isEditor)
            Application.runInBackground = true;
    }

    // Start is called before the first frame update
    void Start()
    {


        if (!render)
        {
            disableAllRenderer();
        }

        Time.timeScale = timeScale;

        CumulativeRewardEpisodes = new List<float>();
        _agent = gameObject.GetComponent<Agent>();
        if (File.Exists(@".\test.json"))
        {
            string json = File.ReadAllText(@".\test.json");
            _actionStateValue = QValueCollection.CreateFromJSON(json);
            _actionStateValue.infoToCollection(_agent);

          
        }
        else _actionStateValue = new QValueCollection();
        if (File.Exists(@".\rewards.json"))
        {
            string jsonRewards = File.ReadAllText(@".\testrewards.json");
            CumulativeRewardEpisodes = CreateFromJSONReward(jsonRewards);
        }
        else CumulativeRewardEpisodes = new List<float>();

        _policyToLearn = new EnvironmentPolicy();
        _policyToFollow = new EnvironmentPolicy();
        _reward = new Reward(0);
    }

    void FixedUpdate()
    {
        if ((_agent as Cartpole2DAgent)._updateCount % (_agent as Cartpole2DAgent)._nFrame == 0)
        {
            if (_agent.State.IsTerminal)
            {
                EndEpisode();
               
            }

            if (_agent.State.IsTerminal == false)
            {
                var res = QLearning.QLearningPolicyWithUnityOneStep(_actionStateValue, _agent.State, _agent, _policyToFollow,
                    _policyToLearn, 1f, 0.1f, epsilon);
                

                _policyToLearn = res.Item2;
                _actionStateValue = res.Item3;
                _reward.Value += res.Item4.Value;
            }
        }


    }

    private void OnApplicationQuit()
    {
        Debug.Log("on application quit");

        string jsonString = JsonUtility.ToJson(_actionStateValue);
        File.WriteAllText(@".\test.json", jsonString);

        string jsonrewards = JsonUtility.ToJson(this);
        File.WriteAllText(@".\testrewards.json", jsonrewards);

        Debug.Log(_actionStateValue);
    }

    private static List<float> CreateFromJSONReward(string json)
    {
        return JsonUtility.FromJson<List<float>>(json);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
             Application.Quit();
        #endif
    }

    public void EndEpisode()
    {
        _agent.Initialise();
        CumulativeRewardEpisodes.Add(_reward.Value);
        _reward = new Reward(0);
        episodeCounter += 1;
        if (epsilon > epsilonMinimum)
            epsilon *= epsilonMultiplier;
        if (learningRate > learningRateMinimum)
            learningRate = 1f / (learningRateMultiplier * (episodeCounter + (1f/learningRateMultiplier))); //must decrease such that the sum diverge but the square converges

        if (episodeCounter >= maxNumberOfEpisode)
        {
            QuitGame();
        }
    }

}

