using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class HomeMenuManager
{
    [SerializeField] private CanvasGroup appointmentCanvasGroup, calendarCanvasGroup, historyCanvasGroup, profileEditCanvasGroup;
    [SerializeField] private Button appointmentButton, calendarButton, historyButton;
    [SerializeField] private Button iconProfileButton, profileEditButton, metaverseButton;
    [SerializeField] private Text nameText;
    [SerializeField] private AppointmentManager appointment;
    [SerializeField] private CalendarManager calendar;
    [SerializeField] private HistoryManager history;
    [SerializeField] private Sprite[] icons;
}
