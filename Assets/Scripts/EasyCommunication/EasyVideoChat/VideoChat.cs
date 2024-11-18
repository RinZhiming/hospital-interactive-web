using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoChat : IVideoChat
{
    private readonly string videoUrl;
    private readonly string roomName;
    public bool IsCallActive { get; private set; }

    public VideoChat(string videoUrl, string roomName)
    {
        this.videoUrl = videoUrl;
        this.roomName = roomName;
    }

    public void OnCall()
    {
        Application.OpenURL($"{videoUrl}/{roomName}");
        IsCallActive = true;
    }
    
    public void OnEndCall(Action onComplete)
    {
        if (onComplete != null) onComplete();
        IsCallActive = false;
    }
}