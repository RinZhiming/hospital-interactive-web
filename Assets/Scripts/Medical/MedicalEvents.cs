using System;
using UnityEngine;
using UnityEngine.UI;

public class MedicalEvents
{
    public static Action<GameObject, Transform> OnAddMedical;
    public static Action OnCostChange;
    public static Action<MedicalObject> OnDelete;
    public static Action OnSave;
    public static Action<string, string, string, string> DisplayUi;
    public static Action OnSaveSuccessful;
    public static Action<float> SetTotalCost;
    public static Action<Button> CheckMedical;
}