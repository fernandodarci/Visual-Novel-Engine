using System.Collections;
using UnityEngine;

public static class GSC_MonoBehaviourUtils
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if(component == null) go.AddComponent<T>();
        return component;
    }
}

