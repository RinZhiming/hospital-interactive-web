using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class MedicalView
{
    [SerializeField] private Button medicalButton, addButton, cancelButton, confirmButton;
    [SerializeField] private Text patientNameText, dateText, doctorNameText, pbText, totalCostText;
    [SerializeField] private CanvasGroup medicalCanvasGroup;
    [SerializeField] private GameObject medicalPrefab;
    [SerializeField] private Transform contain;

    public Transform Container
    {
        get => contain;
        set => contain = value;
    }
}