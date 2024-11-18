using System;
using UnityEngine;

public static class AppointmentEvent
{
    public static Action<GameObject[], Transform, GameObject[], Transform> OnSpawnAvatar;
    public static Action OnGraphEnter;
    public static Action OnEnterRoom;
    public static Action OnEnterRoomError;
    public static Action OnExitRoom;
    public static Action OnExitRoomError;
}