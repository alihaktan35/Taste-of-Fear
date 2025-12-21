using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

/// <summary>
/// Manages Firebase initialization and connection
/// Singleton pattern with DontDestroyOnLoad
/// </summary>
public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager instance;
    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("FirebaseManager");
                instance = go.AddComponent<FirebaseManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private FirebaseApp firebaseApp;
    private DatabaseReference databaseReference;
    private bool isInitialized = false;

    /// <summary>
    /// Returns true if Firebase is ready to use
    /// </summary>
    public bool IsInitialized => isInitialized;

    /// <summary>
    /// Gets the database reference (leaderboard root)
    /// </summary>
    public DatabaseReference Database => databaseReference;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeFirebase();
    }

    /// <summary>
    /// Initializes Firebase and checks dependencies
    /// </summary>
    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            DependencyStatus dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                firebaseApp = FirebaseApp.DefaultInstance;
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                isInitialized = true;

                Debug.Log("Firebase initialized successfully!");
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
                isInitialized = false;
            }
        });
    }

    /// <summary>
    /// Checks if device has internet connection
    /// </summary>
    public bool HasInternetConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    /// <summary>
    /// Validates if Firebase is ready for operations
    /// Shows warning to user if not ready
    /// </summary>
    public bool ValidateFirebaseReady()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Firebase not initialized yet");
            return false;
        }

        if (!HasInternetConnection())
        {
            Debug.LogWarning("No internet connection");
            return false;
        }

        return true;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
