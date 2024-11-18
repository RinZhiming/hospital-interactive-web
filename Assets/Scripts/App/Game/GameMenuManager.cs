using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(ExitButton);
        exitButton.interactable = false;
        
        NetworkEvent.OnEnterRoom += OnEnterRoom;
    }

    private void OnDestroy()
    {
        NetworkEvent.OnEnterRoom -= OnEnterRoom;
    }
    
    private void OnEnterRoom()
    {
        exitButton.interactable = true;
    }

    private void ExitButton()
    {
        GameManager.Launcher.OnLeftRoom();
        SceneManager.LoadScene(SceneName.HomeScene.ToString());
    }
}
