using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class ProfileManager : MonoBehaviour
{
    private Dictionary<InputField, Text> errorMapUi;
    private DoctorData currentDoctorData;
    private void Awake()
    {
        errorMapUi = new()
        {
            {emailInput, emailErrorText},
            {firstNameInput, firstNameErrorText},
            {lastNameInput, lastNameErrorText},
            {birthDayInput, birthDayErrorText},
            {ageInput, ageErrorText},
            {addressInput, addressErrorText},
            {doctorIdInput, doctorIdErrorText},
            {hospitalInput, hospitalErrorText},
        };
        
        birthDayInput.onValueChanged.AddListener(s => BirthdayFormat(s, birthDayInput, ageInput));
        profileButton.onClick.AddListener(ProfileButton);
        cancelButton.onClick.AddListener(CancelButton);
        saveButton.onClick.AddListener(SaveButton);
    }

    private void Start()
    {
        foreach (var map in errorMapUi)
        {
            map.Value.text = string.Empty;
        }
        
        profileCanvas.SetActive(false);
        ageInput.interactable = false;
    }

    private void ProfileButton()
    {
        var uid = PlayerPrefs.GetString("uid");
        ApiConnector.GetUserData(uid, GetUserDataError, GetUserDataComplete);
        
        var profileRaw = PlayerPrefs.GetString("profile");
        var profile = JsonUtility.FromJson<DoctorData>(profileRaw);
        
        currentDoctorData = profile;
        firstNameInput.text = profile.firstName;
        lastNameInput.text = profile.lastName;
        birthDayInput.text = profile.birthday;
        ageInput.text = profile.age.ToString();
        addressInput.text = profile.address;
        doctorIdInput.text = profile.numberId;
        hospitalInput.text = profile.hospitalName;
    }

    private void GetUserDataComplete(string obj)
    {
        var rawdata = JArray.Parse(obj);
        var data = JObject.Parse(rawdata[0].ToString());
        SetUi(data["email"].ToString());
    }

    private void GetUserDataError(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void SetUi(string email)
    {
        profileCanvas.SetActive(true);
        emailInput.text = email;
    }
    
    private void SaveButton()
    {
        var error = ValidateInput();
        if (error.Count > 0)
        {
            DisplayError(error);
        }
        else
        {
            var uid = PlayerPrefs.GetString("uid");
            ApiConnector.UpdateEmail(uid, emailInput.text,UpdateEmailError, UpdateEmailComplete);
        }
    }
    
    private void DisplayError(Dictionary<Text, string> error)
    {
        foreach (var err in error)
        {
            err.Key.gameObject.SetActive(true);
            err.Key.text = err.Value;
        }
    }

    private Dictionary<Text, string> ValidateInput()
    {
        var handleError = new Dictionary<Text, string>();
        foreach (var ui in errorMapUi)
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
        }

        return handleError;
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

    private void UpdateEmailComplete(string obj)
    {
        var uid = PlayerPrefs.GetString("uid");
        var newProfile = new DoctorData
        {
            doctorId = uid,
            firstName = firstNameInput.text,
            lastName = lastNameInput.text,
            birthday = birthDayInput.text,
            age = int.Parse(ageInput.text),
            address = addressInput.text,
            numberId = doctorIdInput.text,
            hospitalName = hospitalInput.text,
            iconProfile = currentDoctorData.iconProfile,
        };
        var profilejson = JsonUtility.ToJson(newProfile);
        ApiConnector.SetDatabase(profilejson, $"doctors/{uid}",HttpCaller.Token,SetDatabaseError, SetDatabaseComplete);
    }

    private void SetDatabaseComplete(string obj)
    {
        UpdateProfile();
    }

    private void UpdateProfile()
    {
        var uid = PlayerPrefs.GetString("uid");
        ApiConnector.GetDatabase($"doctors/{uid}",HttpCaller.Token,GetDatabaseError, GetDatabaseComplete);
    }

    private void GetDatabaseComplete(string obj)
    {
        PlayerPrefs.SetString("profile", obj);
        PlayerPrefs.Save();
        
        profileCanvas.SetActive(false);
    }

    private void GetDatabaseError(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void SetDatabaseError(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void UpdateEmailError(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void CancelButton()
    {
        profileCanvas.SetActive(false);
    }
}
