using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CalendarManager
{
    [SerializeField] private GameObject dateTemplatePrefab;
    [SerializeField] private Transform dateTimeCalendarContainer;
    [SerializeField] private Button nextButton, backButton;
    [SerializeField] private Text headerDateText;
    [SerializeField] private Color paddingColor, mainColor, pressColor;
    [SerializeField] private int[] dayTest;
    [SerializeField] private GridLayoutGroup calendarGrid;
    [SerializeField] private RectTransform gridParent;
}