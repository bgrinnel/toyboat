using System;
using System.Collections.Generic;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

public class ManagerObject<SubclassT> : MonoBehaviour where SubclassT : MonoBehaviour
{
    private static Dictionary<Type, MonoBehaviour> singletons = new();

    protected virtual void Awake()
    {
        Debug.Log("ManagerObject Awake");
        lock (singletons)
        {
            var subclass_type = typeof(SubclassT);
            if (singletons.ContainsKey(subclass_type))
            {
                Debug.LogWarning($"Manager \"{subclass_type}\" already exists in the Scene. This may lead to unexpected behavior.");
                DestroyImmediate(singletons[subclass_type]);
            }
            Debug.Log($"New Manager Created! Name:\"{this.name}\" | typeof({typeof(SubclassT)}) | GetType({this.GetType()})");
            singletons[subclass_type] = this;
        }
    }

    protected virtual void OnDestroyed()
    {
        Debug.Log($"Destroying Manager: \"{typeof(SubclassT)}\"");
        lock(singletons)
        {
            singletons.Remove(typeof(SubclassT));
        }
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
}