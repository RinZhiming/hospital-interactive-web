using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class HistoryManager
{
    [SerializeField] private GameObject historyPrefab, medicalPrefab;
    [SerializeField] private Transform container, medicalContainer;
    [SerializeField] private Sprite[] icons;
    [SerializeField] private CanvasGroup medicalCanvas;
    [SerializeField] private Button backButton;

    [SerializeField] private Text
        patientNameText,
        dateText,
        doctorNameText,
        pbText,
        totalCostText;
}
