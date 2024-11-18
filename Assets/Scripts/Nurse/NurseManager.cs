using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;

public partial class NurseManager : MonoBehaviour
{
    private CinemachineVirtualCamera playerBlendCamera;
    public static bool IsDialogue { get; private set; }
    private bool isSeeDoctor;
    private Nurse currentNurse;

    private void Awake()
    {
        interactButton.onClick.AddListener(OnInteract);
        backButton.onClick.AddListener(OnCloseDialogue);
        seeDoctorButton.onClick.AddListener(SeeDoctorButton);
        IsDialogue = false;
    }

    private void Start()
    {
        interactButton.gameObject.SetActive(false);
        isSeeDoctor = false;
    }

    private void OnDestroy()
    {
    }
    
    private void SeeDoctorButton()
    {
        seeDoctorDialogueGroup.SetActive(true);
        mainDialogueGroup.SetActive(false);
    }
    
    private void OnGoSeeDoctor()
    {
        
        isSeeDoctor = true;
        OnCloseDialogue();
        seeDoctorButton.gameObject.SetActive(false);
        interactButton.gameObject.SetActive(false);
    }
    
    private void OnCloseSeeDoctor()
    {
        seeDoctorDialogueGroup.SetActive(false);
        mainDialogueGroup.SetActive(true);
    }

    private void OnTriggerEnterEvent(Nurse nurse, Collider col)
    {
        if (isSeeDoctor) return;
        playerBlendCamera = col.GetComponent<Player>().VirtualCamera;
        currentNurse = nurse;
    }
    
    private void OnTriggerStayEvent(Nurse nurse, Collider col)
    {
        if (isSeeDoctor) return;
        
        var hit = col.gameObject.GetComponent<PlayerInteract>().HitObject;
        if (hit) interactButton.gameObject.SetActive(hit.GetComponent<Nurse>() == nurse && !IsDialogue);
        else interactButton.gameObject.SetActive(false);
    }
    
    private void OnTriggerExitEvent(Collider col)
    {
        if (isSeeDoctor) return;
        
        interactButton.gameObject.SetActive(false);
        currentNurse = null;
        playerBlendCamera = null;
    }


    private void OnInteract()
    {
        IsDialogue = true;
        interactButton.gameObject.SetActive(false);
        mainDialogueGroup.SetActive(true);
        SwitchCamera(true);
    }

    private void OnCloseDialogue()
    {
        IsDialogue = false;
        interactButton.gameObject.SetActive(true);
        mainDialogueGroup.SetActive(false);
        SwitchCamera(false);
    }
    
    private void SwitchCamera(bool show)
    {
        // if (!playerBlendCamera || !currentNurse.NurseBlendCamera) return;
        //
        // if (show)
        // {
        //     currentNurse.NurseBlendCamera.Priority = 1;
        //     playerBlendCamera.Priority = 0;
        // }
        // else
        // {
        //     currentNurse.NurseBlendCamera.Priority = 0;
        //     playerBlendCamera.Priority = 1;
        // }
    }
}
