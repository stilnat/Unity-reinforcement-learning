using System.Collections.Generic;
using UnityEngine;
using System.IO;

public abstract class Trainer : MonoBehaviour
{
    protected Agent _agent;
    protected Reward _reward;

    protected int _step = 0;

    public float timeScale = 1;
    public bool render = false;
    public bool chargeData = false;
    public bool SaveData = false;
    public int _updateCount = 0;
    public int _nFrame;
    public int maxNumberOfEpisode = 100000;
    public int episodeCounter = 0;


    [SerializeField]
    protected List<float> CumulativeRewardEpisodes;

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (chargeData) ChargeTraining();
        if (!render) disableAllRenderer();

        Time.timeScale = timeScale;

        CumulativeRewardEpisodes = new List<float>();
        _agent = gameObject.GetComponent<Agent>();
        _reward = new Reward(0);
    }

    public abstract void ChargeTraining();
    public abstract void SaveTraining();


    // Update is called once per frame
    public virtual void Update()
    {
        _updateCount += 1;
    }


    void Awake()
    {
        if (Application.isEditor)
            Application.runInBackground = true;
    }

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

    private void OnApplicationQuit()
    {
        if (SaveData) SaveTraining();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                     Application.Quit();
        #endif
    }

    public virtual void EndEpisode()
    {
        _agent.Initialise();
        CumulativeRewardEpisodes.Add(_reward.Value);
        _reward = new Reward(0);
        episodeCounter += 1;

        if (episodeCounter >= maxNumberOfEpisode)
        {
            QuitGame();
        }
    }
}
