using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySingleton<T> : MonoBehaviour where T : DestroySingleton<T>
{
    static volatile T _uniqueInstance = null;
    static volatile GameObject _uniqueObject = null;

    protected DestroySingleton() { }

    public static T _instance
    {
        get
        {
            _uniqueInstance = FindObjectOfType(typeof(T)) as T;
            if (_uniqueInstance == null)
            {
                lock (typeof(T))
                {
                    if (_uniqueInstance == null && _uniqueObject == null)
                    {
                        _uniqueObject = new GameObject(typeof(T).Name, typeof(T));
                        _uniqueInstance = _uniqueObject.GetComponent<T>();
                    }
                }
            }
            else
            {

            }
            return _uniqueInstance;
        }
    }
}
