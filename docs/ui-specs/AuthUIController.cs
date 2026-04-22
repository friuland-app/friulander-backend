using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AuthUIController : MonoBehaviour
{
    [Header("Screens")]
    public GameObject loginScreen, registerScreen, recoveryScreen, onboardingScreen;

    [Header("Login UI")]
    public TMP_InputField loginEmail, loginPassword;
    public Button loginButton, googleLoginButton, appleLoginButton;

    [Header("Register UI")]
    public TMP_InputField registerName, registerEmail, registerPassword;
    public Button registerButton;

    private void Start()
    {
        SetupValidation();
        loginButton.onClick.AddListener(OnLoginClick);
        registerButton.onClick.AddListener(OnRegisterClick);
    }

    private void SetupValidation()
    {
        loginEmail.onEndEdit.AddListener(ValidateEmail);
        registerEmail.onEndEdit.AddListener(ValidateEmail);
    }

    private bool ValidateEmail(string email)
    {
        bool isValid = System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return isValid;
    }

    private void OnLoginClick()
    {
        if (ValidateEmail(loginEmail.text))
        {
            ShowLoading();
        }
    }

    private void OnRegisterClick()
    {
        if (ValidateEmail(registerEmail.text))
        {
            ShowLoading();
        }
    }

    private void ShowLoading()
    {
        // Show loading overlay
    }
}
