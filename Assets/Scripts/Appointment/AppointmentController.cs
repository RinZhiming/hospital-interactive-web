using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppointmentController : MonoBehaviour
{
    private AppointmentModel model;

    private void Awake()
    {
        model = new();
        
        AppointmentEvent.OnExitRoom += OnExitRoom;
        AppointmentEvent.OnGraphEnter += GraphEnter;
        AppointmentEvent.OnSpawnAvatar += GetDoctorCharacter;
    }

    private void Start()
    {
        model.Appointment = AppointmentManager.CurrentAppointment;
    }

    private void OnDestroy()
    {
        AppointmentEvent.OnExitRoom -= OnExitRoom;
        AppointmentEvent.OnGraphEnter -= GraphEnter;
        AppointmentEvent.OnSpawnAvatar -= GetDoctorCharacter;
    }

    private void GraphEnter()
    {
        HealthDeviceEvents.OnGraphEnter?.Invoke(model.Appointment.patientId);
    }
    
    private void GetDoctorCharacter(GameObject[] doctors, Transform doctorPos, GameObject[] players, Transform playerPos)
    {
        var data = PlayerPrefs.GetString("profile");
        var profile = JsonUtility.FromJson<DoctorData>(data);
        Instantiate(doctors[profile.iconProfile - 6], doctorPos.position, doctorPos.rotation);

        GetPlayerCharacter(players, playerPos);
    }
    
    private void GetPlayerCharacter(GameObject[] players, Transform playerPos)
    {
        ApiConnector.GetDatabase($"users/{model.Appointment.patientId}/userdata", HttpCaller.Token,Error,s => CompleteGetPatient(s, players, playerPos));
    }
    
    private void Error(HttpErrorCode obj)
    {
        AppointmentEvent.OnEnterRoomError?.Invoke();
        Debug.LogError(obj);
    }
    
    private void CompleteGetPatient(string obj, GameObject[] players, Transform playerPos)
    {
        var profile = JsonUtility.FromJson<DoctorData>(obj);
        Instantiate(players[profile.iconProfile], playerPos.position, playerPos.rotation);
        AppointmentEvent.OnEnterRoom?.Invoke();
    }
    
    private void OnExitRoom()
    {
        var date = $"{model.Appointment.date.Day}-{model.Appointment.date.Month}-{model.Appointment.date.Year}";
        var time = $"{model.Appointment.startTime.Hours}.00-{model.Appointment.endTime.Hours}.00";
        var appointment = new Appointment
        {
            channelName = model.Appointment.channelName,
            date = date,
            time = time,
            doctorId = model.Appointment.doctorId,
            patientId = model.Appointment.patientId,
            hasAppointment = model.Appointment.hasAppointment,
            hasEnd = true,
            sort = model.Appointment.sort,
        };
        
        var newappointment = JsonUtility.ToJson(appointment);
        
        ApiConnector.SetDatabase(newappointment, $"appointments/{model.Appointment.doctorId}/{date}/{model.Appointment.key}", HttpCaller.Token,SetExitError, SetExitComplete);
        
        PhotonChatEvent.OnDisconnect?.Invoke();
    }

    private void SetExitError(HttpErrorCode obj)
    {
        AppointmentEvent.OnExitRoomError?.Invoke();
    }

    private void SetExitComplete(string data)
    {
        SceneManagerExtension.Instance.LoadScene(SceneName.HomeScene);
    }
}
