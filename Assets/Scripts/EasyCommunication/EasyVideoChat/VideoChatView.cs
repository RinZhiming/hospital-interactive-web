using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoChatView : MonoBehaviour
{
    [SerializeField] private Button callButton;
    [SerializeField] private string url;
    private AppointmentClone appointment;

    private void Awake()
    {
        callButton.onClick.AddListener(CallButton);
    }

    private void Start()
    {
        appointment = AppointmentManager.CurrentAppointment;
    }

    private void CallButton()
    {
        VideoChatManager.OnBeginCall?.Invoke(url, appointment.channelName);
    }
}
