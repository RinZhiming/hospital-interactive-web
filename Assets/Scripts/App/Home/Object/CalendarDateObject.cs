using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CalendarDateObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image, marker;
    [SerializeField] private Text date;
    [SerializeField] private GameObject target;

    public bool HasAppointment { get; set; }
    public bool IsPadding { get; set; }
    
    public bool IsPress { get; set; }

    public static Action<CalendarDateObject> OnClick;
    public Image Image
    {
        get => image;
        set => image = value;
    }
    public Text Date
    {
        get => date;
        set => date = value;
    }
    public Image Marker
    {
        get => marker;
        set => marker = value;
    }
    public GameObject Target
    {
        get => target;
        set => target = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsPadding && !IsPress) OnClick(this);
    }
}
