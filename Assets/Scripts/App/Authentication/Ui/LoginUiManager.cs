using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class LoginManager
{
    [SerializeField] private InputField emailInput, passwordInput;
    [SerializeField] private Text emailError, passwordError;
    [SerializeField] private Button loginButton, registerButton, forgetPasswordButton;
    [SerializeField] private SceneName homeScene, registerScene, forgetPasswordScene;
}
