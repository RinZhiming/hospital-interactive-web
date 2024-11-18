using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoChatManager : MonoBehaviour
{
    /// <summary>
    /// Call this action when you need to start video call URL.
    /// </summary>
    public static Action<string, string> OnBeginCall;
    
    /// <summary>
    /// Subscribe this action to implement validate video call check then it will return <c>VideoChatErrorCode</c>
    /// <see cref="VideoChatErrorCode"/>
    /// </summary>
    public static Action<VideoChatErrorCode> OnCallValidate;
    
    /// <summary>
    /// Call this action when you need to end call successful way.
    /// </summary>
    public static Action<Action> OnEndCall;
    
    /// <summary>
    /// Call this action if you have any issue error on video call.
    /// </summary>
    public static Action OnEndCallImmediately;
    
    private VideoChat currentVideoChat;

    private void Start()
    {
        OnBeginCall += BeginCall;
        OnEndCall += EndCall;
        OnEndCallImmediately += EndCallImmediately;
    }

    private void OnDestroy()
    {
        if (currentVideoChat != null) OnEndCallImmediately?.Invoke();
        
        OnBeginCall -= BeginCall;
        OnEndCall -= EndCall;
        OnEndCallImmediately -= EndCallImmediately;
        
    }

    private void BeginCall(string url, string roomName)
    {
        if (currentVideoChat is { IsCallActive: true })
        {
            OnCallValidate?.Invoke(VideoChatErrorCode.AnotherCallActive);
            EndCallImmediately();
            return;
        }
        
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            OnCallValidate?.Invoke(VideoChatErrorCode.NoInternet);
            EndCallImmediately();
            return;
        }

        currentVideoChat = new VideoChat(url, roomName);

        if (currentVideoChat == null)
        {
            OnCallValidate?.Invoke(VideoChatErrorCode.InitializationFailure);
            EndCallImmediately();
            return;
        }
        
        currentVideoChat.OnCall();
    }

    private void EndCallImmediately()
    {
        currentVideoChat = null;
    }

    private void EndCall(Action onComplete)
    {
        if (currentVideoChat == null)
        {
            OnCallValidate?.Invoke(VideoChatErrorCode.CallNotActive);
            EndCallImmediately();
            return;
        }
        currentVideoChat.OnEndCall(onComplete);
        currentVideoChat = null;
    }
}
