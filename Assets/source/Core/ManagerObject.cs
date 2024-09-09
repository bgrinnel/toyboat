using System;
using System.Collections.Generic;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerObject<SubclassT> : MonoBehaviour where SubclassT : MonoBehaviour
{
    private static Dictionary<Type, MonoBehaviour> singletons;

    private bool bPersistsBetweenScenes = false;

    protected virtual void Awake()
    {
        if (singletons == null) singletons = new();
        lock (singletons)
        {
            var subclass_type = typeof(SubclassT);
            if (singletons.ContainsKey(subclass_type))
            {
                Debug.LogWarning($"Manager \"{subclass_type}\" already exists in the Scene. This may lead to unexpected behavior.");
                DestroyImmediate(singletons[subclass_type]);
            }
            Debug.Log($"New Manager Created! Name:\"{this.name}\" | typeof({subclass_type}) | GetType({this.GetType()})");
            singletons[subclass_type] = this;
        }
    }

    protected virtual void Start()
    {
        if (!bPersistsBetweenScenes) SceneManager.sceneUnloaded += DestroyOnSceneUnload;
    }

    // Set Whether this ManagerObject is not Destroyed when a new Scene becomes active
    public void SetPersistance(bool bPersists)
    {
        if (bPersists && !bPersistsBetweenScenes) SceneManager.sceneUnloaded -= DestroyOnSceneUnload;
        if (!bPersists && bPersistsBetweenScenes) SceneManager.sceneUnloaded += DestroyOnSceneUnload;
        bPersistsBetweenScenes = bPersists;
    }

    public static SubclassT Get()
    {
        if (!singletons.ContainsKey(typeof(SubclassT))) 
        {
            Debug.LogWarning($"No Manager Exists for typeof({typeof(SubclassT)})!");
            return null;
        }

        return (SubclassT)singletons[typeof(SubclassT)];
    }

    // The What destroys a Manager. Extend and place `base.DestroyOnSceneUnload` at the end for further cleanup.
    public virtual void DestroyOnSceneUnload(Scene _old)
    {
        Debug.LogWarning($"SceneUnloaded: Destroying Manager \"{typeof(SubclassT)}\"");
        singletons.Remove(typeof(SubclassT));
        Destroy(this, .25f);
    }
}