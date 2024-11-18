using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MedicalView : MonoBehaviour
{
    private void Awake()
    {
        medicalButton.onClick.AddListener(MedicalButton);
        cancelButton.onClick.AddListener(CancelButton);
        addButton.onClick.AddListener(AddButton);
        confirmButton.onClick.AddListener(ConfirmButton);

        MedicalEvents.DisplayUi += DisplayUi;
        MedicalEvents.OnSaveSuccessful += OnSaveSuccessful;
        MedicalEvents.SetTotalCost += SetTotalCost;
    }

    private void OnDestroy()
    {
        MedicalEvents.DisplayUi -= DisplayUi;
        MedicalEvents.OnSaveSuccessful -= OnSaveSuccessful;
        MedicalEvents.SetTotalCost -= SetTotalCost;
    }

    private void Start()
    {
        medicalCanvasGroup.SetActive(false);
    }

    private void Update()
    {
        MedicalEvents.CheckMedical?.Invoke(confirmButton);
    }

    private void SetTotalCost(float obj)
    {
        totalCostText.text = obj.ToString();
    }
    
    private void OnSaveSuccessful()
    {
        medicalCanvasGroup.SetActive(false);
    }

    private void DisplayUi(string patientName, string date, string doctorName, string pb)
    {
        patientNameText.text = patientName;
        dateText.text = date;
        doctorNameText.text = doctorName;
        pbText.text = pb;
    }
    
    private void ConfirmButton()
    {
        MedicalEvents.OnSave?.Invoke();
    }

    private void AddButton()
    {
        MedicalEvents.OnAddMedical?.Invoke(medicalPrefab, contain);
    }
    
    private void MedicalButton()
    {
        medicalCanvasGroup.SetActive(true);
    }
    
    private void CancelButton()
    {
        medicalCanvasGroup.SetActive(false);
    }
}
