using System;
using UnityEngine;

public class FirebaseApi
{
    private static FirebaseApi instance;
    private static readonly object lockObj = new(); 
    private IApiConfiguration config;
    private string serverAddress;

    private FirebaseApi(IApiConfiguration config)
    {
        this.config = config;
        InitConfig();
    }

    public static FirebaseApi GetInstance(IApiConfiguration config)
    {
        if (instance == null)
        {
            lock (lockObj)
            {
                instance ??= new FirebaseApi(config);
            }
        }

        return instance;
    }

    private void InitConfig()
    {
        switch (config.ServerType)
        {
            case Server.Port:
                if (config.ServerPort <= 0)
                {
                    serverAddress = String.Empty;
                    return;
                }
                serverAddress = "http://localhost:" + config.ServerPort;
                break;
            case Server.Url:
                serverAddress = config.ServerAddress;
                break;
        }
    }

    public void Login(string email, string password, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return;

        var form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        HttpCaller.Instance.Call(serverAddress + "/login", form, error, complete);
    }
    
    public void AutoLogin(string uid, Action<HttpErrorCode> error, Action<string> complete)
    {
        if ( string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(uid)) return;

        var form = new WWWForm();
        form.AddField("uid", uid);
        HttpCaller.Instance.Call(serverAddress + "/autologin", form, error, complete);
    }
    
    public void Register(string email, string password, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return;

        var form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        HttpCaller.Instance.Call(serverAddress + "/register", form, error, complete);
    }
    
    public void GetDatabase(string path,string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(path)) return;

        var form = new WWWForm();
        form.AddField("path", path);
        form.AddField("idToken", token);
        HttpCaller.Instance.Call(serverAddress + "/getdatabase", form, error, complete);
    }
    
    public void SetDatabase(string data, string path,string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(path)) return;

        var form = new WWWForm();
        form.AddField("data", data);
        form.AddField("path", path);
        form.AddField("idToken", token);
        HttpCaller.Instance.Call(serverAddress + "/setdatabase", form, error, complete);
    }
    
    public void PostDatabase(string data, string path,string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(path)) return;

        var form = new WWWForm();
        form.AddField("data", data);
        form.AddField("path", path);
        form.AddField("idToken", token);
        HttpCaller.Instance.Call(serverAddress + "/postdatabase", form, error, complete);
    }
    
    public void PatchDatabase(string data, string path,string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(path)) return;

        var form = new WWWForm();
        form.AddField("data", data);
        form.AddField("path", path);
        form.AddField("idToken", token);
        HttpCaller.Instance.Call(serverAddress + "/patchdatabase", form, error, complete);
    }
    
    public void DeleteDatabase(string data, string path,string token, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(path)) return;

        var form = new WWWForm();
        form.AddField("data", data);
        form.AddField("path", path);
        form.AddField("idToken", token);
        HttpCaller.Instance.Call(serverAddress + "/deletedatabase", form, error, complete);
    }
    
    public void GetUserData(string uid, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(uid)) return;

        var form = new WWWForm();
        form.AddField("uid", uid);
        HttpCaller.Instance.Call(serverAddress + "/getuserdata", form, error, complete);
    }
    
    public void UpdateEmail(string uid,string email, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(uid)) return;

        var form = new WWWForm();
        form.AddField("uid", uid);
        form.AddField("email", email);
        HttpCaller.Instance.Call(serverAddress + "/updateemail", form, error, complete);
    }
    
    public void SendResetPasswordCode(string email, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(email)) return;

        var form = new WWWForm();
        form.AddField("email", email);
        HttpCaller.Instance.Call(serverAddress + "/sendresetpasswordcode", form, error, complete);
    }
    
    public void ResetPassword(string code,string newpassword, Action<HttpErrorCode> error, Action<string> complete)
    {
        if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(code)) return;

        var form = new WWWForm();
        form.AddField("uid", code);
        form.AddField("email", newpassword);
        HttpCaller.Instance.Call(serverAddress + "/resetpasswordcode", form, error, complete);
    }
}