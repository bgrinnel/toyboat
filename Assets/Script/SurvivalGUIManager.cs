using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SurvivalGUIManager : ManagerObject<SurvivalGUIManager>
{

    private int scoreTxtSrc;
    private bool bScoreChanged;
    private Vector2Int timeTxtSrc;
    private bool bTimeChanged;
    [SerializeField] private string scoreFormat = "Score: {0:0000}";
    [SerializeField] private string timeFormat = "Time: {0:00}:{1:00}";
    [SerializeField] private SceneAsset MainMenuScene;

    public GameObject playerGUICanvas;
    public GameObject pauseSettingsCanvas;
    public TMP_Text scoreText;
    public TMP_Text timeText;


    protected override void Awake()
    {
        base.Awake();
        Debug.Log("SurvivalGUIAwake");
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        var mode = (SurvivalModeManager)SurvivalModeManager.Get();
        if (mode is null) Debug.LogWarning("There isn't a SurvivalGameMode in the scene. Score GUI will not update");
        else
        {
            mode.scoreUpdate += OnScoreUpdate;
            mode.timeUpdate += OnTimeUpdate;
            mode.pauseEvent += OnPauseEvent;
            OnTimeUpdate(mode.GetTimeLeft());
        }
        bScoreChanged = false;
        scoreTxtSrc = 0;
        OnScoreUpdate(0);
    }

    void Update()
    {
        if (bScoreChanged)
        {
            lock (scoreText)
            {
                scoreText.text = String.Format(scoreFormat, (scoreTxtSrc));
                bScoreChanged = false;
            }
        }
        if (bTimeChanged)
        {
            lock (timeText)
            {
                timeText.text = String.Format(timeFormat, timeTxtSrc.x, timeTxtSrc.y);
                bTimeChanged = false;
            }
        }
    }

    public void OnScoreUpdate(float newScore)
    {
        lock (scoreText)
        {
            scoreTxtSrc = Mathf.FloorToInt(newScore);
            bScoreChanged = true;
        }
    }

    public void OnTimeUpdate(float newTime)
    {
        lock (timeText)
        {
            var min = Mathf.Floor(newTime / 60f);
            timeTxtSrc = new((int)min, Mathf.FloorToInt(newTime - min * 60f));
            bTimeChanged = true;
        }
    }

    public void OnSettingsBtnClicked()
    {
        SurvivalModeManager.Get().SetPaused(true);
    }

    public void OnContinueGameBtnClicked()
    {
        SurvivalModeManager.Get().SetPaused(false);
    }
    
    public void OnMainMenuBtnClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenuScene.name);
    }


    public void OnPauseEvent(bool bPaused)
    {
        if (bPaused)
        {
            pauseSettingsCanvas.SetActive(true);
            playerGUICanvas.SetActive(false);
        }
        else
        {
            pauseSettingsCanvas.SetActive(false);
            playerGUICanvas.SetActive(true);
        }
    }   
}
