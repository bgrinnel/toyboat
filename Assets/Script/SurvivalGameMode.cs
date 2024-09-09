using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurvivalModeManager : GameModeObject
{
    public delegate void ScoreEvent(float addScore, float multScore = 1.0f);
    public delegate void ScoreUpdate(float newScore);
    public delegate void TimeEvent(float addTime);
    public delegate void TimeUpdate(float newTime);
    public ScoreUpdate scoreUpdate;
    public TimeUpdate timeUpdate;
    public float roundLength;
    private float secondsTillEnd;
    private float elapsedSeconds;
    private List<float2> ongoingScoreEvents;
    private List<float> ongoingTimeEvents;

    private float score;

    private string gameOverReason = "You Were Sunk";
    
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("SurvivalGameMode Awake");
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ongoingScoreEvents = new();
        ongoingTimeEvents = new();
        score = 0;
        secondsTillEnd = roundLength;
        SetPersistance(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        pauseEvent += OnPauseEvent;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (ongoingScoreEvents.Count > 0)
        {   
            lock (ongoingScoreEvents)
            {
                float score_mod = 1.0f, add_score = 0f;
                foreach (var score_event in ongoingScoreEvents)
                {
                    add_score += score_event.x;
                    score_mod += score_event.y;
                }
                score += add_score * score_mod;
                ongoingScoreEvents.Clear();
                scoreUpdate?.Invoke(score);
            }
        }

        lock (ongoingTimeEvents)
        {
            foreach (var e in ongoingTimeEvents)
            {
                secondsTillEnd += e;
            }
            ongoingTimeEvents.Clear();
        }
        if (!IsPaused())
        {
            secondsTillEnd -= Time.deltaTime;
            elapsedSeconds += Time.deltaTime;
            if (secondsTillEnd <= 0f)
            {
                gameOverReason = "Time Expired";
                SceneManager.LoadScene("GameOver");
            }
            timeUpdate?.Invoke(secondsTillEnd);
        }
    }   

    public float GetTimeLeft()
    {
        return secondsTillEnd;
    }
    public void OnScoreEvent(float addScore, float multScore=0.0f)
    {
        lock (ongoingScoreEvents)
        {
            ongoingScoreEvents.Add(new float2(addScore, multScore));
        }
    } 

    public void OnSceneLoaded(Scene newScene, LoadSceneMode _mode)
    {
        if (newScene.name == "GameOver")
        {
            var gui = GameOverGUIManager.Get();
            gui.SetTitle(gameOverReason);
            var sub_title = $"Score:{score}";
            if (gameOverReason == "Time Expired") sub_title += $" Time Alive: {elapsedSeconds} seconds";
            gui.SetSubTitle(sub_title);
            SetPersistance(false);
        }
    }

    public void OnPauseEvent(bool bPaused)
    {
        SetPersistance(!bPaused);
    }

    public void OnTimeEvent(float addTime)
    {
        lock (ongoingTimeEvents)
        {
            ongoingTimeEvents.Add(addTime);
        }
    }

    public override void DestroyOnSceneUnload(Scene _old)
    {
        base.DestroyOnSceneUnload(_old);
    }
}
