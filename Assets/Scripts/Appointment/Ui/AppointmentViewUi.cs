using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class AppointmentView
{
    [SerializeField] private Button exitRoomButton, graphButton, medicalButton;
    [SerializeField] private GameObject[] players, doctors;
    [SerializeField] private Transform playerPos, doctorPos;
    [SerializeField] private CanvasGroup healthDataCanvasGroup;
}
