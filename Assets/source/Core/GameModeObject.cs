using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeObject : ManagerObject<GameModeObject>
{
    
    public delegate void PauseEvent(bool bPaused);
    public PauseEvent pauseEvent;
    [HideInInspector] public List<MonoBehaviour> pausableBehaviors;
    [HideInInspector] public List<GameObject> deactivatableObjects;

    private bool bGameModePaused;

    protected override void Awake()
    {
        Debug.LogWarning($"Awake Timescale; {Time.timeScale}");
        base.Awake();
        Debug.Log("GameModeAwake");
        pausableBehaviors = new();
        deactivatableObjects = new();
        bGameModePaused = false;
    }

    protected override void Start()
    {
        Debug.LogWarning($"Start Timescale; {Time.timeScale}");
        base.Start();
        SetPaused(false);
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            SetPaused(!bGameModePaused);
        }
    }

    public void RegisteryPausableBehavior(MonoBehaviour behavior)
    {
        lock (pausableBehaviors)
        {
            pausableBehaviors.Add(behavior);
            behavior.enabled = !bGameModePaused;
        }
    }

    public void RegisterDisabeableObjects(GameObject obj)
    {
        lock (deactivatableObjects)
        {
            deactivatableObjects.Add(obj);
            obj.SetActive(!bGameModePaused);
        }
    }
    public void UnregisteryPausableBehavior(MonoBehaviour behavior)
    {
        lock (pausableBehaviors)
        {
            pausableBehaviors.Remove(behavior);
        }
    }

    public void UnregisterDisabeableObjects(GameObject obj)
    {
        lock (deactivatableObjects)
        {
            deactivatableObjects.Remove(obj);
        }
    }

    public void SetPaused(bool bPaused)
    {
        if (bGameModePaused == bPaused) return;

        bGameModePaused = bPaused;
        lock (deactivatableObjects)
        {
            lock (pausableBehaviors)
            {
                foreach (var behaviour in pausableBehaviors)
                {
                    if (!behaviour) continue;
                    if (behaviour.IsDestroyed()) continue;
                    behaviour.enabled = !bGameModePaused;
                }
                foreach (var obj in deactivatableObjects)
                {
                    if (!obj) continue;
                    if (obj.IsDestroyed()) continue;
                    obj.SetActive(!bGameModePaused);
                }
            }
        }
        Time.timeScale = bGameModePaused ? 0f : 1f;
        Debug.LogWarning($"SetPaused Timescale; {Time.timeScale}");
        pauseEvent?.Invoke(bGameModePaused);
    }
    
    public bool IsPaused()
    {
        return bGameModePaused;
    }

    public static void Register(UnityEngine.Object other)
    {
        var mode = GameModeObject.Get();
        if (!mode) return; 
        if (other is null) return;
        var mono_behaviour = other as MonoBehaviour;
        var game_obj = other as GameObject;
        if (mono_behaviour is not null) mode.RegisteryPausableBehavior(mono_behaviour);
        else if (game_obj is not null) mode.RegisterDisabeableObjects(game_obj);
        else throw new InvalidCastException($"Only GameObjects or MonoBehaviours can register with a GameMode: Obj_name:{other.name} typeof({other.GetType()})");
        return;
    }
    
    public static void Unregister(UnityEngine.Object other)
    {
        var mode = GameModeObject.Get();
        if (!mode) return; 
        if (other is null) return;
        var mono_behaviour = other as MonoBehaviour;
        var game_obj = other as GameObject;
        if (mono_behaviour is not null) mode.UnregisteryPausableBehavior(mono_behaviour);
        else if (game_obj is not null) mode.UnregisterDisabeableObjects(game_obj);
        else throw new InvalidCastException($"Only GameObjects or MonoBehaviours can register with a GameMode: Obj_name:{other.name} typeof({other.GetType()})");
        return;
    }

    public override void DestroyOnSceneUnload(Scene _old)
    {
        Time.timeScale = 1f;
        Debug.LogWarning($"Unloading Timescale; {Time.timeScale}");
        base.DestroyOnSceneUnload(_old);
    }
}