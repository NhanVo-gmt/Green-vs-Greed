using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SingletonObject<T> : MonoBehaviour where T : SingletonObject<T>
{
    [CanBeNull]
    private static T _instance;
    
    [NotNull]
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object Lock = new object();
    
    public static bool Quitting { get; private set; }

    [NotNull]
    public static T Instance
    {
        get
        {
            // if (Quitting)
            // {
            //     Debug.LogWarning($"[{nameof(_instance)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
            //     // ReSharper disable once AssignNullToNotNullAttribute
            //     return null;
            // }
            lock (Lock)
            {
                if (_instance != null)
                    return _instance;
                var instances = FindObjectsOfType<T>();
                var count = instances.Length;
                if (count > 0)
                {
                    if (count == 1)
                        return _instance = instances[0];
                    Debug.LogWarning($"[{nameof(MonoBehaviour)}<{typeof(T)}>] There should never be more than one {nameof(MonoBehaviour)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                    for (var i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return _instance = instances[0];
                }

                Debug.LogWarning($"[{nameof(MonoBehaviour)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                return null;
            }
        }
    }

    static void DestroyOtherInstances()
    {
        var instances = FindObjectsOfType<T>();
        var count = instances.Length;
        if (count > 0)
        {
            if (count == 1)
                _instance = instances[0];
            Debug.LogWarning($"[{nameof(MonoBehaviour)}<{typeof(T)}>] There should never be more than one {nameof(MonoBehaviour)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
            for (var i = 1; i < instances.Length; i++)
                Destroy(instances[i]);
            _instance = instances[0];
        }

        Debug.LogWarning($"[{nameof(MonoBehaviour)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
        _instance = new GameObject($"({nameof(MonoBehaviour)}){typeof(T)}")
            .AddComponent<T>();
    }

    protected virtual void Awake() 
    {
        if (Instance != this as T)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(Instance);
    }
    
    private void OnApplicationQuit()
    {
        Quitting = true;
    }

}
