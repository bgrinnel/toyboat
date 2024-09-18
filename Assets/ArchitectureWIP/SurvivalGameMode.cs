
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurvivalGameMode : ManagerBehavior
{
    public SurvivalGameMode() : base(typeof(SurvivalGameMode), false) {}

    void Start()
    {
        Debug.Log("SurvivalGameMode Start");
    }
    
    protected override void HandleDestroyed()
    {
        
    }

    protected override void HandleSceneLoaded(Scene newScene, LoadSceneMode mode)
    {
        
    }

    protected override void HandleSceneUnloaded(Scene oldScene)
    {
        
    }
}