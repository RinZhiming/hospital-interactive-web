using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ForgotPasswordManager
{
    [SerializeField] private InputField emailInput, codeInput, passwordInput, confirmPasswordInput;
    [SerializeField] private Button loginButton, emailNextButton, codeNextButton, sendCodeAgainButton, saveButton, backCodeButton;
    [SerializeField] private CanvasGroup emailCanvasGroup, codeCanvasGroup, passwordCanvasGroup;
    [SerializeField] private Text emailErrorText, codeErrorText, passwordErrorText, confirmErrorText, codeBodyText;
}