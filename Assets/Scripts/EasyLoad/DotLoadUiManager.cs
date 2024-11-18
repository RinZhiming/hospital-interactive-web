using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DotLoadManager
{
    [SerializeField] private GameObject[] dotObject;
    [SerializeField] private float bounceTime, bounceHeight, repeatTime, maxTime;
    [SerializeField] private CanvasGroup loadUiCanvas;
}
