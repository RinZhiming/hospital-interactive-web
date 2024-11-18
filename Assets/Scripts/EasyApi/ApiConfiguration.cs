using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Server
{
    Port,
    Url,
}
public class ApiConfiguration : MonoBehaviour, IApiConfiguration
{
    [SerializeField] private Server serverType;
    [SerializeField] private string serverAddress;
    [SerializeField] private uint serverPort;

    public Server ServerType
    {
        get => serverType;
        set => serverType = value;
    }

    public string ServerAddress
    {
        get => serverAddress;
        set => serverAddress = value;
    }

    public uint ServerPort
    {
        get => serverPort;
        set => serverPort = value;
    }
}
