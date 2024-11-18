using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUiExtension : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private ColorBlock onColor, offColor;
    private int currentIndex = 0;

    private void Start()
    {
        if (buttons == null || buttons.Length == 0) return;

        buttons[0].colors = onColor;
        buttons[0].interactable = false;
        
        for (int i = 0; i < buttons.Length; i++)
        {
            var index = i;
            buttons[i].onClick.AddListener(() =>
            {
                currentIndex = index;
                ButtonColor(currentIndex);
            });
        }
    }

    private void ButtonColor(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (currentIndex == i)
            {
                buttons[i].colors = onColor;
                buttons[i].interactable = false;
            }
            else
            {
                buttons[i].colors = offColor;
                buttons[i].interactable = true;
            }
            
        }
    }

    private void OnDestroy()
    {
        if (buttons == null || buttons.Length == 0) return;
            
        foreach (var b in buttons)
        {
            b.onClick.RemoveAllListeners();
        }
    }
}
