using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ManagerBehavior : MonoBehaviour
{
    private bool _bPersistant;
    private Type _DerivedType;
    protected ManagerBehavior(Type derivedType, bool bDestroyedOnLoad)
    {
        _bPersistant = !bDestroyedOnLoad;
        _DerivedType = derivedType;
        ToyBoat.RegisterManager(_DerivedType, this);
    }

    protected virtual void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("Awake called in ManagerBehavior");
    }

    protected abstract void HandleDestroyed();

    public void OnSceneUnloaded(Scene oldScene)
    {
        HandleSceneUnloaded(oldScene);
        if (!_bPersistant)
        {
            ToyBoat.UnregisterManager(_DerivedType);
            HandleDestroyed();
            Destroy(this, 2f);
        }
    }

    protected abstract void HandleSceneUnloaded(Scene oldScene);

    public void OnSceneLoaded(Scene newScene, LoadSceneMode mode)
    {
        HandleSceneLoaded(newScene, mode);
    }

    protected abstract void HandleSceneLoaded(Scene newScene, LoadSceneMode mode);
}