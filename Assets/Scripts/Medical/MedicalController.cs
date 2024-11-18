using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MedicalController : MonoBehaviour
{
    private MedicalModel model;
    private MedicalView view;
    
    private void Awake()
    {
        model = new();
        view = GetComponent<MedicalView>();
        
        MedicalEvents.OnAddMedical += OnAddMedical;
        MedicalEvents.OnCostChange += OnCostChange;
        MedicalEvents.OnDelete += OnDelete;
        MedicalEvents.OnSave += OnSave;
        MedicalEvents.CheckMedical += CheckMedical;
    }

    private void OnDestroy()
    {
        MedicalEvents.OnAddMedical -= OnAddMedical;
        MedicalEvents.OnCostChange -= OnCostChange;
        MedicalEvents.OnDelete -= OnDelete;
        MedicalEvents.OnSave -= OnSave;
        MedicalEvents.CheckMedical -= CheckMedical;
    }

    private void Start()
    {
        model.Appointment = AppointmentManager.CurrentAppointment;
        
        model.Medicals.Clear();
        ClearChild(view.Container);
        InitUi();
    }

    private void InitUi()
    {
        ApiConnector.GetDatabase($"users/{model.Appointment.patientId}/userdata",HttpCaller.Token, Error, GetPatientNameComplete);
    }

    private void GetPatientNameComplete(string data)
    {
        var profile = GetDoctorData();
        var userprofile = JsonUtility.FromJson<UserData>(data);
        model.PatientName = $"{userprofile.firstname} {userprofile.lastname}";
        model.DoctorName = $"{profile.firstName} {profile.lastName}";
        model.DoctorId = profile.numberId;
        MedicalEvents.DisplayUi?.Invoke($"ผู้ป่วย {model.PatientName}",$"วันที่ {model.CurrentDate.Day}/{model.CurrentDate.Month}/{model.CurrentDate.Year}",$"ผู้ตรวจ {model.DoctorName}", $"พ.บ. {model.DoctorId}");
    }

    private void CheckMedical(Button confirmButton)
    {
        confirmButton.interactable = model.Medicals.Count > 0;
    }
    
    private void OnSave()
    {
        var id = PlayerPrefs.GetString("uid");
        var date = $"{model.Appointment.date.Day}-{model.Appointment.date.Month}-{model.Appointment.date.Year}";
        var time = $"{model.Appointment.startTime.Hours}.00-{model.Appointment.endTime.Hours}.00";
        var newdata = new MedicalSummary
        {
            patientId = model.Appointment.patientId,
            patientName = model.PatientName,
            doctorName = model.DoctorName,
            dateTime = model.CurrentDate.Date.ToString(),
            medicals = model.Medicals.ToArray(),
            time = time,
            doctorId = model.DoctorId,
            
            totalCost = model.TotalCost
        };
        var data = JsonUtility.ToJson(newdata);
        
        ApiConnector.SetDatabase(data, $"medicals/{id}/{date}/{model.Appointment.key}",HttpCaller.Token, Error, SetMedicalComplete);
    }

    private void SetMedicalComplete(string obj)
    {
        MedicalEvents.OnSaveSuccessful?.Invoke();
    }

    private void Error(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void OnAddMedical(GameObject prefab, Transform container)
    {
        var newMedical = Instantiate(prefab, container);
        newMedical.transform.SetSiblingIndex(container.childCount - 2);
        var medicalObject = newMedical.GetComponent<MedicalObject>();
        model.Medicals.Add(medicalObject.Medical);
    }
    
    private void OnCostChange()
    {
        var total = 0.0f;
        foreach (var medical in model.Medicals)
        {
            total += medical.cost * medical.number;
        }

        model.TotalCost = total;
        MedicalEvents.SetTotalCost?.Invoke(total);
    }
    
    private void OnDelete(MedicalObject obj)
    {
        model.Medicals.Remove(obj.Medical);
        Destroy(obj.gameObject);
        MedicalEvents.OnCostChange?.Invoke();
    }

    private void ClearChild(Transform container)
    {
        if (!container) return;
        if (container.childCount < 1) return;

        for (int i = 1; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    private DoctorData GetDoctorData()
    {
        var data = PlayerPrefs.GetString("profile");
        var profile = JsonUtility.FromJson<DoctorData>(data);
        return profile;
    }
}