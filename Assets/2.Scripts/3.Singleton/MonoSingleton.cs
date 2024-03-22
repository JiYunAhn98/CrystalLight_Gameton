using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    static volatile T _uniqueInstance = null;
    static volatile GameObject _uniqueObject = null;

    protected MonoSingleton() { }

    public static T _instance
    {
        get
        {
            if (_uniqueInstance == null)
            {
                lock (typeof(T))
                {
                    _uniqueInstance = FindObjectOfType(typeof(T)) as T;

                    if (_uniqueInstance == null && _uniqueObject == null)
                    {
                        _uniqueObject = new GameObject(typeof(T).Name, typeof(T));
                        _uniqueInstance = _uniqueObject.GetComponent<T>();
                    }
                    else
                    {
                        _uniqueObject = _uniqueInstance.gameObject;
                    }
                    _uniqueInstance.Init();
                }
            }
            else
            {
            }
            return _uniqueInstance;
        }
    }
    public virtual void Init()
    {
        DontDestroyOnLoad(_uniqueObject);
    }
}
