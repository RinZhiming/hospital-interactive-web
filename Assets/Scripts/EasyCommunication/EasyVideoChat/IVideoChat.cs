using System;

public enum VideoChatErrorCode
{
    AnotherCallActive,
    CallNotActive,
    NoInternet,
    InitializationFailure,
}

public interface IVideoChat
{
    public void OnCall();
    public void OnEndCall(Action onComplete);
}
