using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoutManager : MonoBehaviour
{
    [SerializeField] private Button logoutButton;

    private void Awake()
    {
        logoutButton.onClick.AddListener(LogOutButton);
    }

    private void LogOutButton()
    {
        PlayerPrefs.DeleteKey("uid");
        PlayerPrefs.DeleteKey("profile");
        SceneManagerExtension.Instance.LoadScene(SceneName.LoginScene);
    }
}
