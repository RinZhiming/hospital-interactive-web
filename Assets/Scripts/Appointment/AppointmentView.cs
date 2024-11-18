using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AppointmentView : MonoBehaviour
{
    
    private void Awake()
    {
        exitRoomButton.onClick.AddListener(ExitRoomButton);
        exitRoomButton.interactable = false;
        graphButton.onClick.AddListener(GraphButton);
        medicalButton.onClick.AddListener(MedicalButton);
        
        AppointmentEvent.OnExitRoomError += OnExitRoomError;
        AppointmentEvent.OnEnterRoom += OnEnterRoom;
        AppointmentEvent.OnEnterRoomError += OnEnterRoom;
    }

    private void OnDestroy()
    {
        AppointmentEvent.OnExitRoomError -= OnExitRoomError;
        AppointmentEvent.OnEnterRoom -= OnEnterRoom;
        AppointmentEvent.OnEnterRoomError -= OnEnterRoom;
    }

    private void Start()
    {
        AppointmentEvent.OnSpawnAvatar?.Invoke(doctors, doctorPos, players, playerPos);
    }
    
    private void MedicalButton()
    {
        
    }

    private void GraphButton()
    {
        healthDataCanvasGroup.SetActive(true);
        AppointmentEvent.OnGraphEnter?.Invoke();
    }

    private void OnEnterRoom()
    {
        exitRoomButton.interactable = true;                                                 
    }
    
    private void ExitRoomButton()
    {
        exitRoomButton.interactable = false;
        AppointmentEvent.OnExitRoom?.Invoke();
    }
    
    private void OnExitRoomError()
    {
        exitRoomButton.interactable = true;
    }
}
