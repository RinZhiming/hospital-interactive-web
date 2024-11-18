using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NetworkLauncher launcher;
    private static GameManager instance;
    private static readonly object lockObj = new ();
    
    private void Awake()
    {
        if (!instance)
        {
            lock (lockObj)
            {
                if (!instance) instance = this;
            }
        }
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        DontDestroyOnLoad(gameObject);
    }

    public static void StartShared(string roomName, int sceneIndex)
    {
        if (CharacterSelectManager.CharacterIndex == -1)
        {
            // instance.dbRef.Child("users").Child(instance.auth.CurrentUser.UserId).Child("userdata").GetValueAsync()
            //     .ContinueWithOnMainThread(
            //         result =>
            //         {
            //             if (result.IsFaulted || result.IsCompleted)
            //             {
            //                 Debug.LogError(result.Exception.Message);
            //                 return;
            //             }
            //
            //             if (result.IsCompleted)
            //             {
            //                 var rawdata = result.Result.GetRawJsonValue();
            //                 var userdata = JsonUtility.FromJson<UserData>(rawdata);
            //                 
            //                 CharacterSelectManager.CharacterIndex = userdata.iconProfile;
            //             }
            //         });
        }
        
        instance.launcher.OnEnterRoom(
            GameMode.Shared, 
            roomName, 
            sceneIndex
            );
    }

    public static NetworkLauncher Launcher => instance.launcher;
}
