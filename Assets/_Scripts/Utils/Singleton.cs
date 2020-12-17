using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to quickly implement Singleton classes.
/// </summary>
/// <typeparam name="T">Class to convert to Singleton</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    /// <summary>
    /// Single instance of the class.
    /// </summary>
    private static T _instance;

    /// <summary>
    /// Allows access to the instance object from other classes
    /// </summary>
    public static T Instance
    {
        get { return _instance; }
    }

    /// <summary>
    /// Returns a bool value depending on whether the instance object is declared or not.
    /// </summary>
    private static bool IsInitialized
    {
        get { return _instance != null; }
    }

    /// <summary>
    /// Method called at the beginning of the execution that creates the instance or returns an error.
    /// </summary>
    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("[Singleton] Trying to instantiate a second instance of a singleton class.");
        }
        else
        {
            _instance = (T)this;
        }
    }


    /// <summary>
    /// Method to delete the object instance before destroying the object.
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
