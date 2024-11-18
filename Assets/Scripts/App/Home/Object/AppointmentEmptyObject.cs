using System;
using UnityEngine;
using UnityEngine.UI;

public class AppointmentEmptyObject : MonoBehaviour
{
    [SerializeField] private Text dateText, timeText;

    public Text DateText
    {
        get => dateText;
        set => dateText = value;
    }

    public Text TimeText
    {
        get => timeText;
        set => timeText = value;
    }
}
