using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SurvivalModeManager : GameModeObject
{

    public delegate void ScoreEvent(float addScore, float multScore = 1.0f);
    public delegate void ScoreUpdate(float newScore);
    [HideInInspector] public ScoreUpdate scoreUpdate;
    [HideInInspector] private List<float2> ongoingScoreEvents;
    [HideInInspector] private float score;
    

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("SurvivalGameMode Awake");
    }
    // Start is called before the first frame update
    protected void Start()
    {
        ongoingScoreEvents = new();
        score = 0;
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
    }

    public void OnScoreEvent(float addScore, float multScore=0.0f)
    {
        lock (ongoingScoreEvents)
        {
            ongoingScoreEvents.Add(new float2(addScore, multScore));
        }
    } 
}
