using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthDataManager : MonoBehaviour
{
    private string currentDeviceId;
    private readonly List<HealthData> dataList = new();
    private readonly List<HealthData> dataListTemp = new();
    private readonly Dictionary<DateTimeRange, List<HealthData>> rangeDataList = new ();
    private static HealthDataManager instance;
    private readonly object lockObject = new();
    public static Action OnAddGraphData;
    [SerializeField] private Button refreshButton;
    
    private void Awake()
    {
        if (instance == null)
        {
            lock (lockObject)
            {
                if (instance == null) instance = this;
            }
        }
        
        HealthDeviceEvents.OnGraphEnter += GetDeviceId;
        HealthDeviceEvents.OnGraphRefresh += Refresh;
        AppointmentEvent.OnExitRoom += ExitRoom;
        
        refreshButton.onClick.AddListener(Refresh);
    }

    private void Start()
    {
        refreshButton.gameObject.SetActive(false);
        currentDeviceId = string.Empty;
    }

    private void ExitRoom()
    {
        dataList.Clear();
        rangeDataList.Clear();
        currentDeviceId = string.Empty;
    }

    private void OnDestroy()
    {
        HealthDeviceEvents.OnGraphEnter -= GetDeviceId;
        HealthDeviceEvents.OnGraphRefresh -= Refresh;
        AppointmentEvent.OnExitRoom -= ExitRoom;
    }

    private void GetDeviceId(string uid)
    {
        refreshButton.gameObject.SetActive(true);
        dataList.Clear();
        rangeDataList.Clear();
        
        ApiConnector.GetDatabase($"users/{uid}/device", HttpCaller.Token, ErrorGetDeviceId, CompleteGetDeviceId);
    }

    private void CompleteGetDeviceId(string obj)
    {
        var data = JObject.Parse(obj);
        currentDeviceId = data["device"].ToString();
        if (!string.IsNullOrEmpty(currentDeviceId)) GetDeviceData();
    }

    private void ErrorGetDeviceId(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void Refresh()
    {
        dataList.Clear();
        rangeDataList.Clear();
        
        if (!string.IsNullOrEmpty(currentDeviceId)) GetDeviceData();
    }

    private void GetDeviceData()
    {
        ApiConnector.GetDatabase($"devices/{currentDeviceId}/history", HttpCaller.Token ,ErrorGetDeviceData, CompleteGetDeviceData);
    }

    private void CompleteGetDeviceData(string obj)
    {
        var dataraw = JObject.Parse(obj);
        var handledata = new Dictionary<string, string>();
        
        foreach (var data in dataraw)
        {
            handledata.TryAdd(data.Key, data.Value.ToString());
        }

        if (handledata.Count > 0) SetData(handledata);
    }

    private void ErrorGetDeviceData(HttpErrorCode obj)
    {
        Debug.LogError(obj);
    }

    private void SetData(Dictionary<string, string> data)
    {
        foreach (var d in data)
        {
            var date = d.Key;
            
            var newdata = new HealthData()
            {
                date = new DateTime(
                    int.Parse(date.Substring(4, 4)),
                    int.Parse(date.Substring(2, 2)),
                    int.Parse(date.Substring(0, 2))),
                healthValue = d.Value,
            };
            
            if(!dataList.Contains(newdata))
                dataList.Add(newdata);
        }
        
        if (rangeDataList.Count == 0) StartCoroutine(CreateRangeData());
    }

    private IEnumerator CreateRangeData()
    {
        dataList.Sort();
        yield return null;
        
        var startDate = dataList[0].date;
        var endDate = dataList[^1].date;
        var totalHandleDay = new List<DateTimeRange>();
        
        var handleRangeDate = new DateTimeRange();
        
        while (!handleRangeDate.InRange(endDate))
        {
            var handleDay = new List<DateTime>();
            if (totalHandleDay.Count <= 0)
            {
                for (int i = 0; i < 7; i++)
                {
                    handleDay.Add(startDate.AddDays(i));
                }
                
                handleRangeDate = new DateTimeRange
                {
                    startDate = startDate,
                    endDate = startDate.AddDays(6),
                    rangeDate = handleDay
                };
                
                totalHandleDay.Add(handleRangeDate);
            }
            else
            {
                for (int i = 1; i < 8; i++)
                {
                    handleDay.Add(totalHandleDay[^1].endDate.AddDays(i));
                }
                
                handleRangeDate = new DateTimeRange
                {
                    startDate = totalHandleDay[^1].endDate.AddDays(1),
                    endDate = totalHandleDay[^1].endDate.AddDays(7),
                    rangeDate = handleDay
                };
                
                totalHandleDay.Add(handleRangeDate);
            }

            yield return null;
        }

        
        foreach (var day in totalHandleDay)
        {
            var handleHealthData = new List<HealthData>();
            
            foreach (var data in dataList)
            {
                if (day.InRange(data.date))
                {
                    handleHealthData.Add(data);
                }
            }
            
            rangeDataList.TryAdd(day, handleHealthData);
        }
        
        yield return null;

        OnAddGraphData();
    }

    public static Dictionary<DateTimeRange, List<HealthData>> RangeDataList => instance.rangeDataList;
}

[Serializable]
public class DateTimeRange
{
    public DateTime startDate;
    public DateTime endDate;
    public List<DateTime> rangeDate = new();

    public bool InRange(DateTime datetime)
    {
        return datetime >= startDate && datetime <= endDate;
    }
}

[Serializable]
public class HealthData : IComparable<HealthData>
{
    public DateTime date;
    public string healthValue;


    public int CompareTo(HealthData other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;

        return DateTime.Compare(date, other.date);
    }
}