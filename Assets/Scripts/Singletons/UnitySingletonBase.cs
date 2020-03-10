using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitySingletonBase<T> : MonoBehaviour
{
    public static T Instance { get; protected set; }
    protected abstract T GetInstance();

    protected virtual void Awake() 
    {
        if(Instance == null)
        {
            Instance = GetInstance();
        } 
        else
        {
            Debug.LogError($"Cannot initialize singleton instance for {typeof(T).Name}! Already initialized.");
            Destroy(gameObject);
        }
    }
}
