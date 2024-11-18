using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class AppointmentManager
{
    [SerializeField] private Button appointmentButton, cancelButton, confirmButton;
    [SerializeField] private Text namePatientText;
    [SerializeField] private Transform containPatient;
    [SerializeField] private GameObject patientPrefab;
    [SerializeField] private CanvasGroup confirmAppointmentCanvasGroup;
    [SerializeField] private Sprite[] icons;
}
