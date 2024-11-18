using System;
using ExitGames.Client.Photon;
using Fusion.Photon.Realtime;
using Photon.Chat;
using UnityEngine;
using AuthenticationValues = Photon.Chat.AuthenticationValues;

public enum ChatSender
{
    Me,
    Other
}
public class PhotonChatController : MonoBehaviour, IChatClientListener
{
    private PhotonChatModel model;
    private AppointmentClone appointment;
    
    private void Awake()
    {
        PhotonChatEvent.SendMessage += SendPrivateMessage;
        PhotonChatEvent.OnDisconnect += Disconnect;
        model = new();
    }

    private void OnDestroy()
    {
        PhotonChatEvent.SendMessage -= SendPrivateMessage;
        PhotonChatEvent.OnDisconnect -= Disconnect;
    }

    private void OnDisable()
    {
        PhotonChatEvent.OnDisconnect?.Invoke();
    }

    private void Start()
    {
        model.Setting = PhotonAppSettings.Global.AppSettings;
        appointment = AppointmentManager.CurrentAppointment;

        var uid = PlayerPrefs.GetString("uid");
        model.ChatClient = new ChatClient(this);
        model.ChatClient.Connect(model.Setting.AppIdChat, model.Setting.AppVersion, new AuthenticationValues(uid));
    }

    private void Update()
    {
        if (model.ChatClient != null)
        {
            model.ChatClient.Service();
        }
    }

    private void Disconnect()
    {
        SendPrivateMessage("<b>Goodbye... Doctor Left Room</b>");
        model.ChatClient.Disconnect();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnDisconnected()
    {
        model.ChatClient.RemoveFriends(new [] { appointment.patientId });
    }

    public void OnConnected()
    {
        var uid = PlayerPrefs.GetString("uid");
        model.ChatClient.AddFriends(new [] { appointment.patientId });
        PhotonChatEvent.OnConnectChat?.Invoke(uid);
        Debug.Log("Chat Connected");
        SendPrivateMessage("<b>Hi! Doctor Enter Room</b>");
    }

    private void SendPrivateMessage(string message)
    {
        model.ChatClient.SendPrivateMessage(appointment.patientId, message);
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        var uid = PlayerPrefs.GetString("uid");
        PhotonChatEvent.OnMessageReceive?.Invoke(
            uid == sender ? ChatSender.Me : ChatSender.Other ,
            sender, 
            message);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }
}
