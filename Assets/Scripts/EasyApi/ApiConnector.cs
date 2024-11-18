using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

[RequireComponent(typeof(ApiConfiguration))]
[RequireComponent(typeof(HttpCaller))]
public class ApiConnector : MonoBehaviour
{
    private ApiConfiguration config;
    private static FirebaseApi api;

    private void Awake()
    {
        config = GetComponent<ApiConfiguration>();
        api = FirebaseApi.GetInstance(config);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Login for the first time and get the uid from http.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <param name="error">Catch error.</param>
    /// <param name="complete">Successful request.</param>
    public static void Login(string email, string password, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.Login(email, password, error, complete);
    }
    
    public static void Register(string email, string password, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.Register(email, password, error, complete);
    }
    
    public static void AutoLogin(string uid, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.AutoLogin(uid, error, complete);
    }
    
    public static void GetDatabase(string path, string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.GetDatabase(path, token,error, complete);
    }
    
    public static void SetDatabase(string data, string path, string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.SetDatabase(data, path, token,error, complete);
    }
    
    public static void PostDatabase(string data, string path, string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.PostDatabase(data, path, token,error, complete);
    }
    
    public static void PatchDatabase(string data, string path, string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.PatchDatabase(data, path, token,error, complete);
    }
    
    public static void DeleteDatabase(string data, string path, string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.DeleteDatabase(data, path, token, error, complete);
    }
    
    public static void GetUserData(string uid, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.GetUserData(uid, error, complete);
    }
    
    public static void UpdateEmail(string uid, string email, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.UpdateEmail(uid,email, error, complete);
    }
    
    public static void SendResetPasswordCode(string email, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.SendResetPasswordCode(email, error, complete);
    }
    
    public static void ResetPassword(string code, string newpassword, Action<HttpErrorCode> error, Action<string> complete)
    {
        api.UpdateEmail(code,newpassword, error, complete);
    }
}
