using UnityEngine;
using System;
using BackEnd;

public class BackendManager : MonoBehaviour
{
    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        BackendGameData.Instance.Initilaize();

	}
    private void Update()
    {
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

}