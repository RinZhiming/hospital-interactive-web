using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordInputfieldUi : MonoBehaviour
{
    [SerializeField] private Button watchPasswordButton;
    [SerializeField] private Sprite eyeOpen, eyeClose;
    [SerializeField] private InputField passwordInput;

    private void Awake()
    {
        watchPasswordButton.onClick.AddListener(ShowPassword);
    }

    private void ShowPassword()
    {
        passwordInput.contentType = passwordInput.contentType == InputField.ContentType.Password ? InputField.ContentType.Standard : InputField.ContentType.Password;
        watchPasswordButton.image.sprite = passwordInput.contentType == InputField.ContentType.Password ? eyeOpen : eyeClose;
        passwordInput.ForceLabelUpdate();
    }
}
