using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpCaller : MonoBehaviour
{
    private static HttpCaller instance;
    private static readonly object lockObj = new();
    [SerializeField] private float maxTime;
    private Coroutine apiCall;
    public static string Token { get; set; }

    private void Awake()
    {
        if (!instance)
        {
            lock (lockObj)
            {
                instance = this;
            }
        }
    }
    
    public static HttpCaller Instance => instance;

    public void Call(string url, WWWForm form, Action<HttpErrorCode> error = null, Action<string> complete= null)
    {
        DotLoadManager.Instance.Loading(true);
        StartCoroutine(ApiCall(url, form, error, complete));
    }
    
    private IEnumerator ApiCall(string url, WWWForm form, Action<HttpErrorCode> error = null, Action<string> complete = null)
    {
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        
        yield return www.SendWebRequest();
    
        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.DataProcessingError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            if (error != null) error(HttpError.HandleErrorCode(www.error));
        }
        else
        {
            var data = www.downloadHandler.text;
            if (complete != null) complete(data);
        }
        
        DotLoadManager.Instance.Loading(false);
    }

    private void OnDestroy()
    {
        Token = string.Empty;
    }
}