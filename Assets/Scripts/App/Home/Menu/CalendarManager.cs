using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public partial class CalendarManager : MonoBehaviour
{
    private DateTime currentDate;
    private DateTimeFormatInfo info;
    private readonly Dictionary<GameObject, CalendarDateObject> calendarMapUi = new();
    public static Action OnResetCalendar;
    public static Action<DateTime> CurrentCalendar;

    private void Awake()
    {
        CalendarDateObject.OnClick += SelectDate;
        CalendarInformationManager.OnAddAppointment += GetDateEmptyAppointment;
    }

    private void Start()
    {
        nextButton.onClick.AddListener(NextButton);
        backButton.onClick.AddListener(BackButton);
        currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        info = new DateTimeFormatInfo();
    }

    private void Update()
    {
        AdjustSpacing();
    }

    private void AdjustSpacing()
    {
        float width = gridParent.rect.width;
        float spacing = width / 17;
        
        calendarGrid.spacing = new Vector2(spacing, 20);
    }

    private void OnDestroy()
    {
        CalendarInformationManager.OnAddAppointment -= GetDateEmptyAppointment;
        CalendarDateObject.OnClick -= SelectDate;
        nextButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    private void CreateCalendar()
    {
        calendarMapUi.Clear();
        OnResetCalendar();
        DateTime startDate = currentDate.AddDays(-(int)currentDate.DayOfWeek);
        
        if (dateTimeCalendarContainer.childCount == 0)
        {
            for (int i = 0; i < 42; i++)
            {
                DateTime day = startDate.AddDays(i);
                GameObject go = Instantiate(dateTemplatePrefab, dateTimeCalendarContainer);
                go.GetComponent<CalendarDateObject>().Marker.gameObject.SetActive(false);
                if (day < currentDate || day.Month != currentDate.Month)
                {
                    go.GetComponent<CalendarDateObject>().Image.color = paddingColor;
                    go.GetComponent<CalendarDateObject>().IsPadding = true;
                }
                else
                {
                    go.GetComponent<CalendarDateObject>().Image.color = mainColor;
                    go.GetComponent<CalendarDateObject>().IsPadding = false;
                    calendarMapUi.TryAdd(go, go.GetComponent<CalendarDateObject>());
                }
                go.GetComponent<CalendarDateObject>().Date.text = currentDate.AddDays(i - (int)currentDate.DayOfWeek).Day.ToString();
            }
        }
        else
        {
            var children = dateTimeCalendarContainer.childCount;
            for (int j = 0; j < children; j++)
            {
                DateTime day = startDate.AddDays(j);
                dateTimeCalendarContainer.GetChild(j).GetComponent<CalendarDateObject>().Marker.gameObject.SetActive(false);
                if (day < currentDate || day.Month != currentDate.Month)
                {
                    dateTimeCalendarContainer.GetChild(j).GetComponent<CalendarDateObject>().Image.color = paddingColor;
                    dateTimeCalendarContainer.GetChild(j).GetComponent<CalendarDateObject>().IsPadding = true;
                }
                else
                {
                    dateTimeCalendarContainer.GetChild(j).GetComponent<CalendarDateObject>().Image.color = mainColor;
                    dateTimeCalendarContainer.GetChild(j).GetComponent<CalendarDateObject>().IsPadding = false;
                    calendarMapUi.TryAdd(
                        dateTimeCalendarContainer.GetChild(j).gameObject,
                        dateTimeCalendarContainer.GetChild(j).GetComponent<CalendarDateObject>());
                }
                dateTimeCalendarContainer.GetChild(j).GetComponent<CalendarDateObject>().Date.text =
                    currentDate.AddDays(j - (int)currentDate.DayOfWeek).Day.ToString();
            }
        }

        GetDateEmptyAppointment();
    }

    private void GetDateEmptyAppointment()
    {
        var uid = PlayerPrefs.GetString("uid");
        var path = $"appointments/{uid}";
        
        ApiConnector.GetDatabase(path,HttpCaller.Token, Error, GetDateComplete);
    }

    private void GetDateComplete(string obj)
    {
        if (!string.IsNullOrEmpty(obj))
        {
            var dates = HttpDecoder.Decode(obj);

            var handleDate = new List<int>(); 
            
            foreach (var date in dates)
            {
                var rawdate = date.Key.Split("-");
                var d = new DateTime(int.Parse(rawdate[2]), int.Parse(rawdate[1]), int.Parse(rawdate[0]));
                if (d.Month == currentDate.Month && d.Year == currentDate.Year)
                {
                    handleDate.Add(d.Day);
                }
            }

            if (handleDate.Count > 0)
            {
                foreach (var ui in calendarMapUi)
                {
                    foreach (var day in handleDate)
                    {
                        if (day == int.Parse(ui.Value.Date.text))
                        {
                            ui.Value.Marker.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    private void Error(HttpErrorCode obj)
    {
        Debug.Log(obj.ToString());
    }

    private void SelectDate(CalendarDateObject target)
    {
        foreach (var ui in calendarMapUi)
        {
            ui.Value.Image.color = target.Date.text == ui.Value.Date.text && target.IsPadding == ui.Value.IsPadding
                ? pressColor
                : mainColor;
            ui.Key.GetComponent<CalendarDateObject>().IsPress = target.Date.text == ui.Value.Date.text;
        }
        
        CurrentCalendar(new DateTime(currentDate.Year, currentDate.Month, int.Parse(target.Date.text)));
    }

    private void NextButton()
    {
        SetCalendar(1);
    }

    private void BackButton()
    {
        SetCalendar(-1);
    }

    public void SetCalendar(int value)
    {
        currentDate = currentDate.AddMonths(value);
        CreateCalendar();
        headerDateText.text = info.GetMonthName(currentDate.Month) + " " + currentDate.Year;
    }
}