using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public partial class CalendarInformationManager : MonoBehaviour
{
    private readonly List<Appointment> times = new()
    {
        new Appointment { sort = 0, time = "9.00-10.00 น." },
        new Appointment { sort = 1, time = "10.00-11.00 น." },
        new Appointment { sort = 2, time = "11.00-12.00 น." },
        new Appointment { sort = 3, time = "13.00-14.00 น." },
        new Appointment { sort = 4, time = "14.00-15.00 น." },
        new Appointment { sort = 5, time = "15.00-16.00 น." },
    };

    private readonly List<string> timesTemp = new();
    private readonly DateTimeFormatInfo info = new();
    public static Action OnAddAppointment;
    private DateTime? currentDate;
    
    private void Awake()
    {
        CalendarManager.OnResetCalendar += OnResetCalendar;
        CalendarManager.CurrentCalendar += CurrentCalendar;
        addTimeButton.onClick.AddListener(AddTimeButton);
        cancelButton.onClick.AddListener(CancelAddTimeButton);
        confirmButton.onClick.AddListener(ConfirmAddTimeButton);
    }
    
    private void Start()
    {
        informationCanvasGroup.SetActive(false);
        selectTimeCanvasGroup.SetActive(false);
    }

    private void ConfirmAddTimeButton()
    {
        if (string.IsNullOrEmpty(addDateDropdown.captionText.text)) return;
        
        var profile = PlayerPrefs.GetString("profile");
        var profileDoctor = HttpDecoder.Decode<DoctorData>(profile);
        var uid = PlayerPrefs.GetString("uid");
        
        if (currentDate != null)
        {
            var datedb = currentDate.Value.Day.ToString() + "-" + currentDate.Value.Month.ToString() + "-" +
                         currentDate.Value.Year.ToString();

            var path = $"appointments/{uid}/{datedb}";

            var date = new Appointment();
            foreach (var time in times)
            {
                if(time.time == addDateDropdown.captionText.text)
                    date = new Appointment
                    {
                        sort = time.sort, 
                        time = addDateDropdown.captionText.text, 
                        hasAppointment = false, 
                        hasEnd = false, 
                        date = datedb,
                        channelName = string.Empty, 
                        doctorId = uid, 
                        patientId = string.Empty,
                    };
            }

            var data = JsonUtility.ToJson(date);
            ApiConnector.PostDatabase(data, path, HttpCaller.Token,e  => Error(e, "ConfirmAddTimeButton : "), SetEmptyTimeComplete);
        }

        selectTimeCanvasGroup.SetActive(false);
        addDateDropdown.ClearOptions();
    }

    private void SetEmptyTimeComplete(string obj)
    {
        var uid = PlayerPrefs.GetString("uid");

        if (currentDate != null) GetDatabase();
    }

    private void CancelAddTimeButton()
    {
        selectTimeCanvasGroup.SetActive(false);
    }
    
    private void CurrentCalendar(DateTime date)
    {
        ClearAllChild(containAppointmentEmpty);
        
        dateText.text = $"{date.Day} {info.GetMonthName(date.Month)} {date.Year}";
        
        currentDate = date;

        GetDatabase();
        
        noSelectText.gameObject.SetActive(false);
    }

    private void GetDatabase()
    {
        var datedb = currentDate.Value.Day.ToString() + "-" + currentDate.Value.Month.ToString() + "-" +
                     currentDate.Value.Year.ToString();
        
        var uid = PlayerPrefs.GetString("uid");
        var path = $"appointments/{uid}/{datedb}";
        
        ApiConnector.GetDatabase(path, HttpCaller.Token,e => Error(e, "CurrentCalendar : "),  GetAppointmentTime);
    }

    private void AddTimeButton()
    {
        addDateDropdown.ClearOptions();
        addDateDropdown.value = 0;

        var handleTime = new List<string>();

        foreach (var time in times)
        {
            if (!timesTemp.Contains(time.time)) handleTime.Add(time.time);
        }

        foreach (var newtime in handleTime)
        {
            addDateDropdown.options.Add(new Dropdown.OptionData(newtime));
        }

        if (addDateDropdown.options.Count > 0)
            addDateDropdown.captionText.text = addDateDropdown.options[0].text;
        
        if (string.IsNullOrEmpty(addDateDropdown.captionText.text)) 
            confirmButton.interactable = false;

        selectTimeCanvasGroup.SetActive(true);
    }


    private void OnDestroy()
    {
        CalendarManager.OnResetCalendar -= OnResetCalendar;
        CalendarManager.CurrentCalendar -= CurrentCalendar;
        addTimeButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        confirmButton.onClick.RemoveAllListeners();
    }

    private void GetAppointmentTime(string obj)
    {
        ClearAllChild(containAppointmentEmpty);
        
        informationCanvasGroup.SetActive(true);
        
        if (!string.IsNullOrEmpty(obj))
        {

            var times = HttpDecoder.Decode(obj);
            var handleAppointment = new List<Appointment>();

            foreach (var time in times)
            {
                var appointment = JsonUtility.FromJson<Appointment>(time.Value.ToString());
                handleAppointment.Add(appointment);
            }
            
            handleAppointment.Sort();

            foreach (var time in handleAppointment)
            {
                GameObject newAppointment = Instantiate(appointmentEmptyPrefab, containAppointmentEmpty);
                var appointmentObject = newAppointment.GetComponent<AppointmentEmptyObject>();
                if (currentDate != null)
                {
                    appointmentObject.DateText.text =
                        $"{currentDate.Value.Day} {info.GetMonthName(currentDate.Value.Month)} {currentDate.Value.Year}";
                }
                
                appointmentObject.TimeText.text = time.time;
                timesTemp.Add(time.time);
            }

            OnAddAppointment?.Invoke();
            
            // containAppointmentEmpty.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            // containAppointmentEmpty.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            // containAppointmentEmpty.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
        }
    }

    private void Error(HttpErrorCode obj,string s)
    {
        Debug.Log(s + obj);
    }

    private void OnResetCalendar()
    {
        ClearAllChild(containAppointmentEmpty);
        currentDate = null;
        noSelectText.gameObject.SetActive(true);
        informationCanvasGroup.SetActive(false);
    }

    private void ClearAllChild(Transform parent)
    {
        timesTemp.Clear();
        
        if (parent.childCount <= 0) return;
        
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
