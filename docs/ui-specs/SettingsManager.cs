using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider masterVolume;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown frameRateDropdown;

    [Header("Notification Settings")]
    [SerializeField] private Toggle pushNotifications;
    [SerializeField] private Toggle inAppNotifications;

    [Header("Privacy Settings")]
    [SerializeField] private Toggle gpsToggle;
    [SerializeField] private Toggle cameraToggle;

    [Header("Account Settings")]
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button deleteAccountButton;

    [Header("Accessibility Settings")]
    [SerializeField] private TMP_Dropdown textSizeDropdown;
    [SerializeField] private Toggle highContrastToggle;
    [SerializeField] private Toggle reduceMotionToggle;

    [Header("Confirmation Dialogs")]
    [SerializeField] private GameObject logoutDialog;
    [SerializeField] private GameObject deleteAccountDialog;

    private void Start()
    {
        LoadSettings();
        SetupEventListeners();
    }

    private void LoadSettings()
    {
        // Audio
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f);

        // Graphics
        qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
        frameRateDropdown.value = PlayerPrefs.GetInt("FrameRate", 1);

        // Notifications
        pushNotifications.isOn = PlayerPrefs.GetInt("PushNotifications", 1) == 1;
        inAppNotifications.isOn = PlayerPrefs.GetInt("InAppNotifications", 1) == 1;

        // Privacy
        gpsToggle.isOn = PlayerPrefs.GetInt("GPS", 1) == 1;
        cameraToggle.isOn = PlayerPrefs.GetInt("Camera", 1) == 1;

        // Accessibility
        textSizeDropdown.value = PlayerPrefs.GetInt("TextSize", 1);
        highContrastToggle.isOn = PlayerPrefs.GetInt("HighContrast", 0) == 1;
        reduceMotionToggle.isOn = PlayerPrefs.GetInt("ReduceMotion", 0) == 1;

        ApplySettings();
    }

    private void SetupEventListeners()
    {
        // Audio
        musicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolume.onValueChanged.AddListener(OnSFXVolumeChanged);
        masterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);

        // Graphics
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
        frameRateDropdown.onValueChanged.AddListener(OnFrameRateChanged);

        // Notifications
        pushNotifications.onValueChanged.AddListener(OnPushNotificationsChanged);
        inAppNotifications.onValueChanged.AddListener(OnInAppNotificationsChanged);

        // Privacy
        gpsToggle.onValueChanged.AddListener(OnGPSToggleChanged);
        cameraToggle.onValueChanged.AddListener(OnCameraToggleChanged);

        // Account
        logoutButton.onClick.AddListener(OnLogoutClick);
        deleteAccountButton.onClick.AddListener(OnDeleteAccountClick);

        // Accessibility
        textSizeDropdown.onValueChanged.AddListener(OnTextSizeChanged);
        highContrastToggle.onValueChanged.AddListener(OnHighContrastChanged);
        reduceMotionToggle.onValueChanged.AddListener(OnReduceMotionChanged);
    }

    #region Audio Settings

    private void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        AudioListener.volume = masterVolume.value;
    }

    private void OnSFXVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void OnMasterVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        AudioListener.volume = value;
    }

    #endregion

    #region Graphics Settings

    private void OnQualityChanged(int value)
    {
        PlayerPrefs.SetInt("Quality", value);
        QualitySettings.SetQualityLevel(value);
    }

    private void OnFrameRateChanged(int value)
    {
        PlayerPrefs.SetInt("FrameRate", value);
        int targetFPS = value switch
        {
            0 => 30,
            1 => 60,
            2 => 120,
            _ => 60
        };
        Application.targetFrameRate = targetFPS;
    }

    #endregion

    #region Notification Settings

    private void OnPushNotificationsChanged(bool value)
    {
        PlayerPrefs.SetInt("PushNotifications", value ? 1 : 0);
        // Request or revoke push notifications
    }

    private void OnInAppNotificationsChanged(bool value)
    {
        PlayerPrefs.SetInt("InAppNotifications", value ? 1 : 0);
    }

    #endregion

    #region Privacy Settings

    private void OnGPSToggleChanged(bool value)
    {
        PlayerPrefs.SetInt("GPS", value ? 1 : 0);
        // Request or revoke GPS permission
    }

    private void OnCameraToggleChanged(bool value)
    {
        PlayerPrefs.SetInt("Camera", value ? 1 : 0);
        // Request or revoke camera permission
    }

    #endregion

    #region Account Settings

    private void OnLogoutClick()
    {
        logoutDialog.SetActive(true);
    }

    public void ConfirmLogout()
    {
        logoutDialog.SetActive(false);
        // Perform logout
        Debug.Log("Logout confirmed");
    }

    private void OnDeleteAccountClick()
    {
        deleteAccountDialog.SetActive(true);
    }

    public void ConfirmDeleteAccount()
    {
        deleteAccountDialog.SetActive(false);
        // Perform account deletion
        Debug.Log("Account deletion confirmed");
    }

    #endregion

    #region Accessibility Settings

    private void OnTextSizeChanged(int value)
    {
        PlayerPrefs.SetInt("TextSize", value);
        ApplyTextSize(value);
    }

    private void ApplyTextSize(int sizeIndex)
    {
        float fontSize = sizeIndex switch
        {
            0 => 12f,
            1 => 14f,
            2 => 16f,
            3 => 18f,
            _ => 14f
        };

        TMP_Text[] textComponents = FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text text in textComponents)
        {
            text.fontSize = fontSize;
        }
    }

    private void OnHighContrastChanged(bool value)
    {
        PlayerPrefs.SetInt("HighContrast", value ? 1 : 0);
        ApplyHighContrast(value);
    }

    private void ApplyHighContrast(bool enabled)
    {
        if (enabled)
        {
            // Apply high contrast colors
            Camera.main.backgroundColor = Color.black;
        }
        else
        {
            // Restore normal colors
            Camera.main.backgroundColor = new Color(0.95f, 0.95f, 0.95f);
        }
    }

    private void OnReduceMotionChanged(bool value)
    {
        PlayerPrefs.SetInt("ReduceMotion", value ? 1 : 0);
        // Disable/enable animations globally
    }

    #endregion

    private void ApplySettings()
    {
        // Apply all loaded settings
        OnMusicVolumeChanged(musicVolume.value);
        OnSFXVolumeChanged(sfxVolume.value);
        OnMasterVolumeChanged(masterVolume.value);
        OnQualityChanged(qualityDropdown.value);
        OnFrameRateChanged(frameRateDropdown.value);
        OnTextSizeChanged(textSizeDropdown.value);
        OnHighContrastChanged(highContrastToggle.isOn);
        OnReduceMotionChanged(reduceMotionToggle.isOn);
    }

    public void ResetToDefaults()
    {
        PlayerPrefs.DeleteAll();
        LoadSettings();
    }
}
