using UnityEngine;
using UnityEngine.UI;

public partial class CalendarInformationManager
{
    [SerializeField] private CanvasGroup informationCanvasGroup, selectTimeCanvasGroup;
    [SerializeField] private Text noSelectText, dateText;
    [SerializeField] private Button addTimeButton, cancelButton, confirmButton;
    [SerializeField] private Dropdown addDateDropdown;
    [SerializeField] private GameObject appointmentEmptyPrefab;
    [SerializeField] private Transform containAppointmentEmpty;
}