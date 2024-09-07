using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalGUIManager : ManagerObject<SurvivalGUIManager>
{

    private int scoreTxtSrc;
    private bool bScoreChanged;
    public GameObject playerGUICanvas;
    public GameObject pauseSettingsCanvas;
    public TMP_Text scoreText;
    [SerializeField] private string ScoreFormat = "Score : {0}";


    protected new virtual void Awake()
    {
        base.Awake();
        Debug.Log("SurvivalGUIAwake");
    }
    // Start is called before the first frame update
    void Start()
    {
        var mode = (SurvivalModeManager)SurvivalModeManager.Get();
        if (mode is null) Debug.LogWarning("There isn't a SurvivalGameMode in the scene. Score GUI will not update");
        else
        {
            mode.scoreUpdate += OnScoreUpdate;
            mode.pauseEvent += OnPauseEvent;
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
                scoreText.text = String.Format(ScoreFormat, (scoreTxtSrc));
                bScoreChanged = false;
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

    public void OnSettingsBtnClicked()
    {
        SurvivalModeManager.Get().SetPaused(true);
    }

    public void OnContinueGameBtnClicked()
    {
        SurvivalModeManager.Get().SetPaused(false);
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
