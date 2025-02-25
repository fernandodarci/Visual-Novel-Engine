using System;
using System.Collections;
using UnityEngine;

public class GSC_Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region SINGLETON

    private static T instance; // Static instance of the singleton.
    public static T Instance => instance; // Accessor for the instance.

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetOrAddComponent<T>(); // Set the instance if it is null.
            // Optional: DontDestroyOnLoad(gameObject); // Keeps the instance across scenes.
        }
        else
        {
            Destroy(gameObject); // Destroy the duplicate instance.
        }
    }

    #endregion
}

