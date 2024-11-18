using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using UnityEngine;

public class CinemachineTouchInput : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private Transform cameraTransform;

    private void Start()
    {
        if (PlayerManager.IsPause) return;
    }

    public Transform CameraTransform
    {
        get => cameraTransform;
        set => cameraTransform = value;
    }
    
    public CinemachineVirtualCamera VirtualCamera
    {
        get => camera;
        set => camera = value;
    }
}
