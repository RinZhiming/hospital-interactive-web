using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class LoginManager : MonoBehaviour
{
    private Dictionary<InputField, Text> errorUiMapper = new();

    private void Awake()
    {
        errorUiMapper = new()
        {
            {emailInput, emailError},
            {passwordInput, passwordError},
        };
    }

    private void Start()
    {
        loginButton.onClick.AddListener(LoginButton);
        registerButton.onClick.AddListener(RegisterButton);
        forgetPasswordButton.onClick.AddListener(ForgetPasswordButton);
        emailInput.Select();

        foreach (var ui in errorUiMapper)
        {
            ui.Value.gameObject.SetActive(false);
        }
    }

    private void LoginButton()
    {
        ClearAllError();
        ApiConnector.Login(emailInput.text, passwordInput.text, Error, LoginComplete );
    }

    private void LoginComplete(string obj)
    {
        var data = HttpDecoder.Decode(obj);
        var uid = data["uid"]?.ToString();
        HttpCaller.Token = data["token"]?.ToString();
        PlayerPrefs.SetString("uid", uid);
        PlayerPrefs.Save();
        
        ApiConnector.GetDatabase($"doctors/{uid}", HttpCaller.Token, Error, DatabaseComplete);
    }

    private void DatabaseComplete(string obj)
    {
        PlayerPrefs.SetString("profile", obj);
        PlayerPrefs.Save();
        var profile = JsonUtility.FromJson<DoctorData>(obj);
        SceneManagerExtension.Instance.LoadScene(profile.iconProfile == -1
            ? SceneName.CharacterSelectScene
            : SceneName.HomeScene);
    }

    private void Error(HttpErrorCode obj)
    {
        DisplayError(passwordError,"กรุณาใส่อีเมลหรือรหัสผ่านให้ถูกต้อง");
    }

    private void DisplayError(Text input, string error)
    {
        input.gameObject.SetActive(true);
        input.text = error;
    }

    private void RegisterButton()
    {
        SceneManagerExtension.Instance.LoadScene(SceneName.RegisterScene);
    }

    private void ForgetPasswordButton()
    {
        SceneManagerExtension.Instance.LoadScene(SceneName.ForgetPasswordScene);
    }
    
    private void ClearAllError()
    {
        foreach (var ui in errorUiMapper)
        {
            ClearError(ui.Value);
        }
    }

    private void ClearError(Text error)
    {
        error.text = string.Empty;
        error.gameObject.SetActive(false);
    }
}
