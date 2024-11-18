using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public partial class ForgotPasswordManager : MonoBehaviour
{
    private Dictionary<InputField, Text> errorMappUi = new();
    private string currentEmail;
    private string currentCode;
    
    private void Awake()
    {
        errorMappUi = new()
        {
            {passwordInput, passwordErrorText},
            {confirmPasswordInput, confirmErrorText}
        };
        
        loginButton.onClick.AddListener(LoginButton);
        emailNextButton.onClick.AddListener(EmailNextButton);
        backCodeButton.onClick.AddListener(CodeBackButton);
        codeNextButton.onClick.AddListener(CodeNextButton);
        sendCodeAgainButton.onClick.AddListener(ReSendCodeButton);
        saveButton.onClick.AddListener(SaveButton);
        
        emailInput.onValueChanged.AddListener(_ =>
        {
            emailErrorText.text = string.Empty;
        });
        
        codeInput.onValueChanged.AddListener(_ =>
        {
            codeErrorText.text = string.Empty;
        });
        
        passwordInput.onValueChanged.AddListener(_ =>
        {
            passwordErrorText.text = string.Empty;
        });
        
        confirmPasswordInput.onValueChanged.AddListener(_ =>
        {
            confirmErrorText.text = string.Empty;
        });
    }

    private void Start()
    {
        currentEmail = string.Empty;
        currentCode = string.Empty;
        
        emailCanvasGroup.SetActive(true);
        codeCanvasGroup.SetActive(false);
        passwordCanvasGroup.SetActive(false);
        
        emailErrorText.text = string.Empty;
        codeErrorText.text = string.Empty;
        passwordErrorText.text = string.Empty;
        confirmErrorText.text = string.Empty;
    }
    
    private void SaveButton()
    {
        var patternPassword = @"^(?=.*[a-z])(?=.*\d).{8,}$";
        var haveError = false;
        foreach (var error in errorMappUi)
        {
            if (string.IsNullOrEmpty(error.Key.text))
            {
                error.Value.text = "กรุณากรอกข้อมูล";
                haveError = true;
            }
        }
        
        if (passwordInput.text != confirmPasswordInput.text)
        {
            confirmErrorText.text = "รหัสผ่านไม่ตรงกัน";
            haveError = true;
        }

        if (!Regex.IsMatch(passwordInput.text, patternPassword))
        {
            confirmErrorText.text = "รหัสผ่านไม่ตรงตามเงื่อนไข";
            haveError = true;
        }
        
        if(!haveError) 
            ApiConnector.ResetPassword(currentCode, confirmPasswordInput.text, OnResetPasswordError, OnResetPasswordComplete);
    }

    private void OnResetPasswordComplete(string obj)
    {
        SceneManagerExtension.Instance.LoadScene(SceneName.LoginScene);
    }

    private void OnResetPasswordError(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void ReSendCodeButton()
    {
        ApiConnector.SendResetPasswordCode(currentEmail, OnReSendCodeError, obj => { });
    }

    private void OnReSendCodeError(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }
    
    private void CodeNextButton()
    {
        SceneManagerExtension.Instance.LoadScene(SceneName.LoginScene);
    }

    private void CodeBackButton()
    {
        codeCanvasGroup.SetActive(false);
        emailCanvasGroup.SetActive(true);
    }

    private void EmailNextButton()
    {
        if (!string.IsNullOrEmpty(emailInput.text))
        {
            emailCanvasGroup.SetActive(false);
            ApiConnector.SendResetPasswordCode(emailInput.text, OnSendCodeError, OnSendCodeComplete);
            currentEmail = emailInput.text;

            codeBodyText.text = $"ระบบได้ทำการส่งรหัส สำหรับการตั้งรหัสผ่านใหม่ไปที่อีเมล {currentEmail} เรียบร้อยแล้ว";
        }
        else
        {
            emailErrorText.text = "กรุณากรอกข้อมูล";
        }
    }
    
    private void OnSendCodeError(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void OnSendCodeComplete(string obj)
    {
        codeCanvasGroup.SetActive(true);
    }

    private void LoginButton()
    {
        SceneManagerExtension.Instance.LoadScene(SceneName.LoginScene);
    }
}
