using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Analytics;

public class FirebaseManager : MonoBehaviour
{
    FirebaseApp _app;
    private void Start()
    {

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });

    }
    
}
