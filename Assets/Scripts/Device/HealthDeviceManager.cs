using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class HealthDeviceManager : MonoBehaviour
{
    [SerializeField] private Button refreshButton;
    private readonly object deviceLockObj = new();
    private string deviceId;

    private void Awake()
    {
        connectButton.onClick.AddListener(ConnectButton);
        connectorButton.onClick.AddListener(ConnectorButton);
        deviceInput.onValueChanged.AddListener(InputValidate);
        backButton.onClick.AddListener(BackButton);
        
        connectButton.interactable = false;
    }


    private void Start()
    {
        deviceCanvasGroup.SetActive(false);
        
        errorText.text = string.Empty;
    }
    
    private void InputValidate(string s)
    {
        connectButton.interactable = !string.IsNullOrEmpty(s);
        errorText.text = string.Empty;
    }

    private void BackButton()
    {
        deviceCanvasGroup.SetActive(false);
        refreshButton.gameObject.SetActive(false);
    }

    private void ConnectorButton()
    {
        deviceCanvasGroup.SetActive(true);
        errorText.text = string.Empty;
    }
    
    private void ConnectButton()
    {
        if (!string.IsNullOrEmpty(deviceInput.text))
        {
            
        }
    }
}
