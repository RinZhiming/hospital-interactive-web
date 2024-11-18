using System;
using UnityEngine;
using UnityEngine.UI;

public class AppointmentPatientObject : MonoBehaviour
{
    [SerializeField] private Text dateText, nameText;
    [SerializeField] private Button buttons;
    [SerializeField] private Image icon;

    public static Action<AppointmentPatientObject> OnClick;
    public AppointmentClone Appointment { get; set; }

    private void Start()
    {
        buttons.onClick.AddListener(AppointmentClick);
    }

    private void AppointmentClick()
    {
        OnClick?.Invoke(this);
    }

    public Text DateText
    {
        get => dateText;
        set => dateText = value;
    }

    public Text NameText
    {
        get => nameText;
        set => nameText = value;
    }
    
    public Image Icon
    {
        get => icon;
        set => icon = value;
    }
}