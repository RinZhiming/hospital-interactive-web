using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public partial class AppointmentManager : MonoBehaviour
{
    private Dictionary<string, Appointment> appointments = new();
    public static AppointmentClone CurrentAppointment { get; set; }
    private DateTimeFormatInfo info;

    private void Awake()
    {
        cancelButton.onClick.AddListener(CancelButton);
        confirmButton.onClick.AddListener(ConfirmButton);
        info = new();
    }

    private void Start()
    {
        appointments.Clear();
        AppointmentPatientObject.OnClick += OnClick;
        confirmAppointmentCanvasGroup.SetActive(false);
    }

    public void CreateAppointment()
    {
        GetAppointmentData();
    }

    private void GetAppointmentData()
    {
        var uid = PlayerPrefs.GetString("uid");
        var currentDate = 
            $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}";
        var path = $"appointments/{uid}/{currentDate}";
        ApiConnector.GetDatabase(path, HttpCaller.Token,Error, GetAppointmentDataComplete);
    }

    private void GetAppointmentDataComplete(string obj)
    {
        appointments.Clear();

        if (string.IsNullOrEmpty(obj))
        {
            return;
        }
        
        var rawdatas = HttpDecoder.Decode(obj);
        foreach (var data in rawdatas)
        {
            if (data.Value != null && (bool)data.Value["hasAppointment"] &&!(bool)data.Value["hasEnd"] )
            {
                var appointment = JsonUtility.FromJson<Appointment>(data.Value.ToString());
                appointments.TryAdd(data.Key, appointment);
            }
        }
        
        DisplayPatient();
    }

    private void Error(HttpErrorCode obj)
    {
        Debug.Log(obj.ToString());
    }

    private void DisplayPatient()
    {
        ClearAllChild(containPatient);
        
        foreach (var appointment in appointments)
        {
            GetPatientData(appointment.Key, appointment.Value);
        }
    }

    private void GetPatientData(string key, Appointment appointment)
    {
        var path = $"users/{appointment.patientId}/userdata";
        ApiConnector.GetDatabase(path, HttpCaller.Token,Error, s => DisplayAppointment(key, s, appointment));
    }

    private void DisplayAppointment(string key, string patient, Appointment appointment)
    {
        var patientdata = JsonUtility.FromJson<UserData>(patient);
        GameObject patientGameObject = Instantiate(patientPrefab, containPatient);
        
        var patientObject = patientGameObject.GetComponent<AppointmentPatientObject>();
        var rawdate = appointment.date.Split('-');
        var date = new DateTime(int.Parse(rawdate[2]), int.Parse(rawdate[1]), int.Parse(rawdate[0]));
        var seperateTime = appointment.time.Split('-');
        var start = seperateTime[0].Split('.')[0];
        var end = seperateTime[1].Split('.')[0];
                        
        var startTime = new TimeSpan(int.Parse(start), 0,0);
        var endTime = new TimeSpan(int.Parse(end), 0,0);
        
        var appointmentClone = new AppointmentClone
        {
            channelName = appointment.channelName,
            date = date,
            startTime = startTime,
            endTime = endTime,
            doctorId = appointment.doctorId,
            patientId = appointment.patientId,
            hasAppointment = appointment.hasAppointment,
            hasEnd = appointment.hasEnd,
            sort = appointment.sort,
            key = key,
        };
        patientObject.Appointment = appointmentClone;
        
        
        patientObject.DateText.text = $"{date.DayOfWeek} {date.Day} {info.GetMonthName(date.Month)} {date.Year} - {appointment.time}";
        patientObject.NameText.text = $"{patientdata.firstname} {patientdata.lastname}";
        patientObject.Appointment = appointmentClone;
        patientObject.Icon.sprite = patientdata.iconProfile > -1 ? icons[patientdata.iconProfile] : null;
    }
    
    private void ClearAllChild(Transform parent)
    {
        if (parent.childCount <= 0) return;
        
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private void OnDestroy()
    {
        AppointmentPatientObject.OnClick -= OnClick;
    }
    
    private void ConfirmButton()
    {
        if (CurrentAppointment != null)
        {
            SceneManagerExtension.Instance.LoadScene(SceneName.AppointmentScene);
        }
    }

    private void CancelButton()
    {
        confirmAppointmentCanvasGroup.SetActive(false);
        namePatientText.text = string.Empty;
        CurrentAppointment = null;
    }

    private void OnClick(AppointmentPatientObject obj)
    {
        namePatientText.text = "เข้าห้องตรวจคนไข้ " + "'" + obj.NameText.text + "'";
        confirmAppointmentCanvasGroup.SetActive(true);
        CurrentAppointment = obj.Appointment;
    }
}
