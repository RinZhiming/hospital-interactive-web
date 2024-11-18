using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public partial class HistoryManager : MonoBehaviour
{
    private Dictionary<string, Appointment> appointments = new();
    private DateTimeFormatInfo info;

    private void Awake()
    {
        backButton.onClick.AddListener(MedicineBackButton);
        HistoryPatientObject.OnClick += OnClick;
    }

    private void OnDestroy()
    {
        HistoryPatientObject.OnClick -= OnClick;
    }

    private void Start()
    {
        info = new();
        medicalCanvas.SetActive(false);
    }

    public void CreateHistory()
    {
        GetAppointmentData();
    }
    
    private void OnClick(HistoryPatientObject obj)
    {
        patientNameText.text = string.Empty;
        dateText.text = string.Empty;
        doctorNameText.text = string.Empty;
        pbText.text = string.Empty;
        totalCostText.text = string.Empty;
        ClearAllChild(medicalContainer);
        
        var date = $"{obj.Appointment.date.Day}-{obj.Appointment.date.Month}-{obj.Appointment.date.Year}";
        ApiConnector.GetDatabase($"medicals/{obj.Appointment.doctorId}/{date}/{obj.Appointment.key}", HttpCaller.Token,GetAppointmentError, GetMedicalDataComplete);
    }

    private void GetAppointmentError(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void GetMedicalDataComplete(string obj)
    {
        medicalCanvas.SetActive(true);
        var medical = JsonUtility.FromJson<MedicalSummary>(obj);
        var date = DateTime.Parse(medical.dateTime);
        patientNameText.text = $"ผู้ป่วย {medical.patientName}";
        dateText.text = $"วันที่ {date.Day} {info.GetMonthName(date.Month)} {date.Year}";
        doctorNameText.text = $"ผู้ตรวจ {medical.doctorName}";
        pbText.text = $"พ.บ. {medical.doctorId}";
        totalCostText.text = medical.totalCost.ToString();
        
        foreach (var m in medical.medicals)
        {
            var newMedicalList = Instantiate(medicalPrefab, medicalContainer);
            var medicalObject = newMedicalList.GetComponent<MedicalObject>();
            
            medicalObject.CostMedicalInputField.interactable = false;
            medicalObject.NameMedicalInputField.interactable = false;
            medicalObject.NumberMedicalInputField.interactable = false;
            
            medicalObject.CostMedicalInputField.text = m.cost.ToString();
            medicalObject.NameMedicalInputField.text = m.name;
            medicalObject.NumberMedicalInputField.text = m.number.ToString();
            
            medicalObject.DeleteButton.gameObject.SetActive(false);
        }
    }
    
    private void MedicineBackButton()
    {
        medicalCanvas.SetActive(false);
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
            if (data.Value != null && (bool)data.Value["hasEnd"] )
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
        ClearAllChild(container);
        
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
        GameObject patientGameObject = Instantiate(historyPrefab, container);
        
        var patientObject = patientGameObject.GetComponent<HistoryPatientObject>();
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
}
