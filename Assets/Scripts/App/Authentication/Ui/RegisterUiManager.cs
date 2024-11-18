using UnityEngine;
using UnityEngine.UI;

public partial class RegisterManager
{
    [SerializeField] private CanvasGroup registerCanvasGroup, successCanvasGroup;

    [SerializeField] private InputField emailInput,
        firstNameInput,
        lastNameInput,
        birthDayInput,
        ageInput,
        addressInput,
        numberIdInput,
        hospitalInput,
        passwordInput,
        confirmPasswordInput;
    
    [SerializeField] private Text emailError,
        firstNameError,
        lastNameError,
        birthDayError,
        ageError,
        addressError,
        numberIdError,
        hospitalError,
        passwordError,
        confirmPasswordError;
    [SerializeField] private Button registerButton, loginButton;
    [SerializeField] private SceneName loginScene;
}
