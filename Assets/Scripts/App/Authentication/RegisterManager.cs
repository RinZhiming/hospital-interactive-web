using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public partial class RegisterManager : MonoBehaviour
{
    private Dictionary<InputField, Text> errorUiMapper = new();
    private void Awake()
    {
        errorUiMapper = new()
        {
            {emailInput, emailError},
            {firstNameInput, firstNameError},
            {lastNameInput, lastNameError},
            {birthDayInput, birthDayError},
            {ageInput, ageError},
            {addressInput, addressError},
            {numberIdInput, numberIdError},
            {hospitalInput, hospitalError},
            {passwordInput, passwordError},
            {confirmPasswordInput, confirmPasswordError},
        };
    }

    private void Start()
    {
        registerCanvasGroup.SetActive(true);
        successCanvasGroup.SetActive(false);
        loginButton.onClick.AddListener(LoginButton);
        birthDayInput.onValueChanged.AddListener(s => BirthdayFormat(s, birthDayInput, ageInput));
        registerButton.onClick.AddListener(RegisterButton);
        emailInput.Select();
        foreach (var ui in errorUiMapper)
        {
            ui.Value.gameObject.SetActive(false);
        }
    }

    private Dictionary<Text, string> RegisterValidate()
    {
        var handleError = new Dictionary<Text, string>();
        foreach (var ui in errorUiMapper)
        {
            if (string.IsNullOrEmpty(ui.Key.text))
            {
                handleError.TryAdd(ui.Value, "กรุณากรอกให้ครบ");
            }

            if (ui.Key == birthDayInput)
            {
                var birthday = BirthdayValidate(ui.Key);
                if (!string.IsNullOrEmpty(birthday))
                {
                    handleError.TryAdd(ui.Value, birthday);
                }
            }

            if (ui.Key == passwordInput || ui.Key == confirmPasswordInput)
            {
                var password = PasswordValidate(passwordInput, confirmPasswordInput);
                foreach (var error in password)
                {
                    if (ui.Key == error.Key)
                    {
                        handleError.TryAdd(ui.Value, error.Value);
                    }
                }
            }

        }

        return handleError;
    }

    private void RegisterButton()
    {
        ClearAllError();
        var error = RegisterValidate();
        if (error.Count > 0)
        {
            DisplayError(error);
        }
        else
        {
            OnRegister();
        }
    }

    private void OnRegister()
    {
        ApiConnector.Register(emailInput.text, passwordInput.text, Error, CompleteRegister);
    }

    private void CompleteRegister(string obj)
    {
        var userdata = HttpDecoder.Decode(obj);
        var uid = userdata["uid"]?.ToString();
        var token = userdata["token"]?.ToString();
        OnDatabaseProfile(uid,token);
    }

    private void OnDatabaseProfile(string uid, string token)
    {
        var rawdata = new DoctorData
        {
            firstName = firstNameInput.text,
            lastName = lastNameInput.text,
            birthday = birthDayInput.text,
            age = int.Parse(ageInput.text),
            address = addressInput.text,
            numberId = numberIdInput.text,
            hospitalName = hospitalInput.text,
            doctorId = uid,
            iconProfile = -1,
        };
        var data = JsonUtility.ToJson(rawdata);
        var path = $"doctors/{uid}";
        ApiConnector.SetDatabase(data, path,token, Error, CompleteDatabaseProfile);
    }

    private void CompleteDatabaseProfile(string obj)
    {
        registerCanvasGroup.SetActive(false);
        successCanvasGroup.SetActive(true);
        StartCoroutine(DelayPage());
    }

    private IEnumerator DelayPage()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManagerExtension.Instance.LoadScene(SceneName.LoginScene);
    }

    private void Error(HttpErrorCode obj)
    {
        Debug.LogError("Error" + obj);
    }

    private void DisplayError(Dictionary<Text, string> error)
    {
        foreach (var err in error)
        {
            err.Key.gameObject.SetActive(true);
            err.Key.text = err.Value;
        }
    }

    private void BirthdayFormat(string input, InputField bithdayinput, InputField ageinput)
    {
        string limitstring = Regex.Replace(input, @"[^\d]", "");

        string formatdate = string.Empty;
        for (int i = 0; i < limitstring.Length; i++)
        {
            if(i == 2 || i == 4)
            {
                formatdate += "/";
            }
            if(i < 8)
            {
                formatdate += limitstring[i];
            }
        }
        
        AgeCalculate(formatdate, ageinput);
        bithdayinput.text = formatdate;
        bithdayinput.caretPosition = bithdayinput.text.Length;
    }

    private string BirthdayValidate(InputField birthdayinput)
    {
        var currentYear = DateTime.Now.Year + 543;
        var dateraw = birthdayinput.text;
        
        if (dateraw.Length < 10) return "กรุณากรอกวันเกิดให้ครบ";
        
        var day = $"{dateraw[0]}{dateraw[1]}";
        var month = $"{dateraw[3]}{dateraw[4]}";
        var yearraw = $"{dateraw[6]}{dateraw[7]}{dateraw[8]}{dateraw[9]}";
        var year = int.Parse(yearraw) - 543;
        var date = $"{month}/{day}/{year}";
        
        var isdatevalidate = DateTime.TryParse(date, out DateTime _);

        var yearthai = year + 543;
        if (isdatevalidate && currentYear - yearthai <= 100 && currentYear - yearthai >= 20) return string.Empty;
        return "กรุณากรอกวันเกิดให้ถูกต้อง";
    }

    private void AgeCalculate(string date, InputField ageinput)
    {
        if (date.Length < 10)
        {
            ageinput.text = string.Empty;
            return;
        }
        var yearraw = $"{date[6]}{date[7]}{date[8]}{date[9]}";
        var year = int.Parse(yearraw);
        var currentYear = DateTime.Now.Year + 543;
        ageinput.text = (currentYear - year).ToString();
    }

    private Dictionary<InputField, string> PasswordValidate(InputField password, InputField confirmpassword)
    {
        var handleError = new Dictionary<InputField, string>();
        if (password.text != confirmpassword.text) handleError.TryAdd(confirmpassword, "รหัสผ่านไม่ตรงกัน");
        var hasLetter = Regex.IsMatch(password.text, "[a-z]");
        var hasNumber = Regex.IsMatch(password.text, "[0-9]");
        if(password.text.Length < 8 || !hasLetter || !hasNumber) handleError.TryAdd(password, "รหัสผ่านไม่ตรงตามเงื่อนไข");
        return handleError;
    }

    private void LoginButton()
    {
        SceneManagerExtension.Instance.LoadScene(loginScene);
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
