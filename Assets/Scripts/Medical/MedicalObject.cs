using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MedicalObject : MonoBehaviour
{
    [SerializeField] private InputField nameMedicalInputField, numberMedicalInputField, costMedicalInputField;
    [SerializeField] private Button deleteButton;
    public Medical Medical { get; set; }

    private void Awake()
    {
        Medical = new();
        
        nameMedicalInputField.onValueChanged.AddListener(s =>
        {
            Medical.name = s;
        });
        
        numberMedicalInputField.onValueChanged.AddListener(s =>
        {
            Medical.number = string.IsNullOrEmpty(s) ? 0 : int.Parse(s);
            MedicalEvents.OnCostChange?.Invoke();
        });
        
        costMedicalInputField.onValueChanged.AddListener(s =>
        {
            Medical.cost = string.IsNullOrEmpty(s) ? 0 : float.Parse(s);
            MedicalEvents.OnCostChange?.Invoke();
        });
        
        deleteButton.onClick.AddListener(OnDeleteButton);
    }

    private void OnDeleteButton()
    {
        MedicalEvents.OnDelete?.Invoke(this);
    }

    public InputField NameMedicalInputField
    {
        get => nameMedicalInputField;
        set => nameMedicalInputField = value;
    }

    public InputField NumberMedicalInputField
    {
        get => numberMedicalInputField;
        set => numberMedicalInputField = value;
    }

    public InputField CostMedicalInputField
    {
        get => costMedicalInputField;
        set => costMedicalInputField = value;
    }

    public Button DeleteButton
    {
        get => deleteButton;
        set => deleteButton = value;
    }
}