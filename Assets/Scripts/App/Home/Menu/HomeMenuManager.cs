using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuName
{
    Appointment,
    Calendar,
    History,
}

public partial class HomeMenuManager : MonoBehaviour
{
    private MenuName currentMenuName = MenuName.Appointment;
    private Dictionary<MenuName, CanvasGroup> canvasUiMap = new ();

    private void Awake()
    {
        iconProfileButton.onClick.AddListener(IconProfileButton);
        profileEditButton.onClick.AddListener(ProfileEditButton);
        metaverseButton.onClick.AddListener(MetaverseButton);
    }

    private void MetaverseButton()
    {
        GameManager.StartShared("Main", 3);
    }

    private void ProfileEditButton()
    {
        //profileEditCanvasGroup.SetActive(true);
    }

    private void IconProfileButton()
    {
        SceneManagerExtension.Instance.LoadScene(SceneName.CharacterSelectScene);
    }

    private void Start()
    {
        canvasUiMap = new()
        {
            { MenuName.Appointment, appointmentCanvasGroup }, 
            { MenuName.Calendar, calendarCanvasGroup },
            { MenuName.History, historyCanvasGroup }
        };
        
        CanvasToggle(currentMenuName);

        
        appointmentButton.onClick.AddListener(delegate
        {
            CanvasToggle(MenuName.Appointment);
            appointment.CreateAppointment();
        });
        calendarButton.onClick.AddListener(delegate
        {
            CanvasToggle(MenuName.Calendar);
            calendar.SetCalendar(0);
        });
        historyButton.onClick.AddListener(delegate
        {
            CanvasToggle(MenuName.History);
            history.CreateHistory();
        });
        
        metaverseButton.onClick.AddListener(() =>
        {
            SceneManagerExtension.Instance.LoadScene(SceneName.GameScene);
        });

        var profileData = PlayerPrefs.GetString("profile");
        var profile = JsonUtility.FromJson<DoctorData>(profileData);

        iconProfileButton.image.sprite = profile.iconProfile > -1 ? icons[profile.iconProfile - 6] : null;
        nameText.text = $"{profile.firstName} {profile.lastName}";
        
        appointment.CreateAppointment();
    }

    private void OnDestroy()
    {
        appointmentButton.onClick.RemoveAllListeners();
        calendarButton.onClick.RemoveAllListeners();
        historyButton.onClick.RemoveAllListeners();
    }

    private void CanvasToggle(MenuName target)
    {
        foreach (var ui in canvasUiMap)
        {
            if (ui.Key == target)
            {
                ui.Value.SetActive(true);
                currentMenuName = ui.Key;
            }
            else
            {
                ui.Value.SetActive(false);
            }
        }
    }
}