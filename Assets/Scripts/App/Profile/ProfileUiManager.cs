using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ProfileManager
{
    [SerializeField] private Button profileButton, cancelButton, saveButton;
    [SerializeField] private CanvasGroup profileCanvas;

    [SerializeField] private InputField 
        emailInput,
        firstNameInput,
        lastNameInput,
        birthDayInput,
        ageInput,
        addressInput,
        doctorIdInput,
        hospitalInput;
    
    [SerializeField] private Text 
        emailErrorText,
        firstNameErrorText,
        lastNameErrorText,
        birthDayErrorText,
        ageErrorText,
        addressErrorText,
        doctorIdErrorText,
        hospitalErrorText;
}
