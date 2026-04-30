using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class UIAnimationManager : MonoBehaviour
{
    public static UIAnimationManager Instance { get; private set; }

    [Header("Animation Settings")]
    [SerializeField] private float defaultDuration = 0.3f;
    [SerializeField] private bool reduceMotion = false;
    [SerializeField] private bool enableHaptics = true;
    [SerializeField] private bool enableSounds = true;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip navigateSound;
    [SerializeField] private AudioClip notificationSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip achievementSound;

    private AudioSource audioSource;
    private List<Tween> activeTweens = new List<Tween>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        DOTween.SetTweensCapacity(500);
        Application.targetFrameRate = 60;
    }

    #region Screen Transitions

    public void TransitionScreen(GameObject fromScreen, GameObject toScreen, TransitionType type)
    {
        float duration = reduceMotion ? defaultDuration * 2 : defaultDuration;

        switch (type)
        {
            case TransitionType.SlideLeft:
                SlideTransition(fromScreen, toScreen, Vector3.left, duration);
                break;
            case TransitionType.SlideRight:
                SlideTransition(fromScreen, toScreen, Vector3.right, duration);
                break;
            case TransitionType.Fade:
                FadeTransition(fromScreen, toScreen, duration);
                break;
            case TransitionType.Scale:
                ScaleTransition(fromScreen, toScreen, duration);
                break;
        }

        PlaySound(navigateSound);
    }

    private void SlideTransition(GameObject from, GameObject to, Vector3 direction, float duration)
    {
        CanvasGroup fromGroup = from.GetComponent<CanvasGroup>();
        CanvasGroup toGroup = to.GetComponent<CanvasGroup>();

        if (fromGroup == null) fromGroup = from.AddComponent<CanvasGroup>();
        if (toGroup == null) toGroup = to.AddComponent<CanvasGroup>();

        to.SetActive(true);
        toGroup.alpha = 0;
        to.transform.localPosition = direction * Screen.width;

        Tween fromTween = fromGroup.DOFade(0, duration).SetEase(Ease.InOutQuad);
        Tween fromMove = from.transform.DOLocalMoveX(-direction.x * Screen.width, duration).SetEase(Ease.InOutQuad);

        Tween toTween = toGroup.DOFade(1, duration).SetEase(Ease.InOutQuad);
        Tween toMove = to.transform.DOLocalMoveX(0, duration).SetEase(Ease.InOutQuad);

        fromTween.OnComplete(() => from.SetActive(false));

        RegisterTween(fromTween);
        RegisterTween(fromMove);
        RegisterTween(toTween);
        RegisterTween(toMove);
    }

    private void FadeTransition(GameObject from, GameObject to, float duration)
    {
        CanvasGroup fromGroup = from.GetComponent<CanvasGroup>();
        CanvasGroup toGroup = to.GetComponent<CanvasGroup>();

        if (fromGroup == null) fromGroup = from.AddComponent<CanvasGroup>();
        if (toGroup == null) toGroup = to.AddComponent<CanvasGroup>();

        to.SetActive(true);
        toGroup.alpha = 0;

        Tween fromTween = fromGroup.DOFade(0, duration).SetEase(Ease.InQuad);
        Tween toTween = toGroup.DOFade(1, duration).SetEase(Ease.OutQuad);

        fromTween.OnComplete(() => from.SetActive(false));

        RegisterTween(fromTween);
        RegisterTween(toTween);
    }

    private void ScaleTransition(GameObject from, GameObject to, float duration)
    {
        CanvasGroup fromGroup = from.GetComponent<CanvasGroup>();
        CanvasGroup toGroup = to.GetComponent<CanvasGroup>();

        if (fromGroup == null) fromGroup = from.AddComponent<CanvasGroup>();
        if (toGroup == null) toGroup = to.AddComponent<CanvasGroup>();

        to.SetActive(true);
        toGroup.alpha = 0;
        to.transform.localScale = Vector3.zero;

        Tween fromTween = fromGroup.DOFade(0, duration).SetEase(Ease.InQuad);
        Tween fromScale = from.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InQuad);

        Tween toTween = toGroup.DOFade(1, duration).SetEase(Ease.OutBack);
        Tween toScale = to.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);

        fromTween.OnComplete(() => from.SetActive(false));

        RegisterTween(fromTween);
        RegisterTween(fromScale);
        RegisterTween(toTween);
        RegisterTween(toScale);
    }

    #endregion

    #region Popup Animations

    public void ShowPopup(GameObject popup, PopupType type)
    {
        float duration = reduceMotion ? defaultDuration * 2 : defaultDuration;

        popup.SetActive(true);
        popup.transform.localScale = Vector3.zero;

        switch (type)
        {
            case PopupType.Notification:
                popup.transform.localPosition = new Vector3(0, 300f, 0);
                popup.transform.DOLocalMoveY(0, duration).SetEase(Ease.OutBack);
                popup.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
                break;
            case PopupType.Alert:
                popup.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
                break;
            case PopupType.BottomSheet:
                popup.transform.localPosition = new Vector3(0, -Screen.height * 0.7f, 0);
                popup.transform.DOLocalMoveY(0, duration).SetEase(Ease.OutBack);
                break;
            case PopupType.Tooltip:
                CanvasGroup group = popup.GetComponent<CanvasGroup>();
                if (group == null) group = popup.AddComponent<CanvasGroup>();
                group.DOFade(1, duration * 0.5f).SetEase(Ease.OutQuad);
                break;
        }

        PlaySound(notificationSound);
    }

    public void HidePopup(GameObject popup, PopupType type)
    {
        float duration = reduceMotion ? defaultDuration * 2 : defaultDuration;

        switch (type)
        {
            case PopupType.Notification:
                popup.transform.DOLocalMoveY(300f, duration).SetEase(Ease.InQuad)
                    .OnComplete(() => popup.SetActive(false));
                break;
            case PopupType.Alert:
                popup.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack)
                    .OnComplete(() => popup.SetActive(false));
                break;
            case PopupType.BottomSheet:
                popup.transform.DOLocalMoveY(-Screen.height * 0.7f, duration).SetEase(Ease.InQuad)
                    .OnComplete(() => popup.SetActive(false));
                break;
            case PopupType.Tooltip:
                CanvasGroup group = popup.GetComponent<CanvasGroup>();
                if (group != null)
                {
                    group.DOFade(0, duration * 0.5f).SetEase(Ease.InQuad)
                        .OnComplete(() => popup.SetActive(false));
                }
                break;
        }
    }

    #endregion

    #region Button Feedback

    public void ButtonPress(Transform button)
    {
        if (reduceMotion) return;

        button.DOScale(0.95f, 0.1f).SetEase(Ease.OutQuad)
            .OnComplete(() => button.DOScale(1f, 0.1f).SetEase(Ease.OutQuad));

        PlayHaptic(HapticType.Light);
        PlaySound(clickSound);
    }

    public void ButtonSuccess(Transform button)
    {
        if (reduceMotion) return;

        button.DOScale(1.2f, 0.25f).SetEase(Ease.OutBack)
            .OnComplete(() => button.DOScale(1f, 0.25f).SetEase(Ease.OutBack));

        PlayHaptic(HapticType.Success);
        PlaySound(successSound);
    }

    public void ButtonError(Transform button)
    {
        if (reduceMotion) return;

        button.DOShakePosition(0.3f, 10f, 10, 90f, false);

        PlayHaptic(HapticType.Error);
        PlaySound(errorSound);
    }

    #endregion

    #region Progress Bar Animations

    public void AnimateProgressBar(Slider slider, float targetValue)
    {
        float duration = reduceMotion ? defaultDuration * 2 : defaultDuration;

        slider.DOValue(targetValue, duration).SetEase(Ease.OutQuad);
    }

    public void PulseProgressBar(Transform progressBar)
    {
        if (reduceMotion) return;

        progressBar.DOScale(1.05f, 0.5f).SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopPulseProgressBar(Transform progressBar)
    {
        progressBar.DOKill();
        progressBar.localScale = Vector3.one;
    }

    public void ProgressComplete(Transform progressBar)
    {
        if (reduceMotion) return;

        progressBar.DOScale(1.1f, 0.15f).SetEase(Ease.OutBack)
            .OnComplete(() => progressBar.DOScale(1f, 0.15f).SetEase(Ease.OutBack));

        PlayHaptic(HapticType.Success);
        PlaySound(successSound);
    }

    #endregion

    #region Haptic Feedback

    public void PlayHaptic(HapticType type)
    {
        if (!enableHaptics || reduceMotion) return;

#if UNITY_IOS && !UNITY_EDITOR
        switch (type)
        {
            case HapticType.Light:
                UnityEngine.iOS.Device.GenerateHaptic((UnityEngine.iOS.NotificationFeedbackType)0);
                break;
            case HapticType.Medium:
                UnityEngine.iOS.Device.GenerateHaptic((UnityEngine.iOS.NotificationFeedbackType)1);
                break;
            case HapticType.Heavy:
                UnityEngine.iOS.Device.GenerateHaptic((UnityEngine.iOS.NotificationFeedbackType)2);
                break;
            case HapticType.Success:
                UnityEngine.iOS.Device.GenerateHaptic(UnityEngine.iOS.NotificationFeedbackType.Success);
                break;
            case HapticType.Warning:
                UnityEngine.iOS.Device.GenerateHaptic(UnityEngine.iOS.NotificationFeedbackType.Warning);
                break;
            case HapticType.Error:
                UnityEngine.iOS.Device.GenerateHaptic(UnityEngine.iOS.NotificationFeedbackType.Error);
                break;
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        long vibrationPattern = type switch
        {
            HapticType.Light => 50,
            HapticType.Medium => 100,
            HapticType.Heavy => 200,
            HapticType.Success => new long[] { 100, 50, 100 },
            HapticType.Warning => new long[] { 50, 50, 50 },
            HapticType.Error => new long[] { 200, 100, 200 },
            _ => 50
        };
        Handheld.Vibrate(vibrationPattern);
#endif
    }

    #endregion

    #region Sound Effects

    public void PlaySound(AudioClip clip)
    {
        if (!enableSounds || clip == null) return;

        audioSource.PlayOneShot(clip);
    }

    public void PlayClickSound()
    {
        PlaySound(clickSound);
    }

    public void PlayNavigateSound()
    {
        PlaySound(navigateSound);
    }

    public void PlayNotificationSound()
    {
        PlaySound(notificationSound);
    }

    public void PlaySuccessSound()
    {
        PlaySound(successSound);
    }

    public void PlayErrorSound()
    {
        PlaySound(errorSound);
    }

    public void PlayAchievementSound()
    {
        PlaySound(achievementSound);
    }

    #endregion

    #region Tween Management

    private void RegisterTween(Tween tween)
    {
        tween.SetRecyclable(true);
        tween.SetAutoKill(true);
        activeTweens.Add(tween);
        tween.OnKill(() => activeTweens.Remove(tween));
    }

    public void KillAllTweens()
    {
        foreach (Tween tween in activeTweens)
        {
            tween.Kill();
        }
        activeTweens.Clear();
    }

    public void SetReduceMotion(bool enabled)
    {
        reduceMotion = enabled;
    }

    public void SetHapticsEnabled(bool enabled)
    {
        enableHaptics = enabled;
    }

    public void SetSoundsEnabled(bool enabled)
    {
        enableSounds = enabled;
    }

    #endregion
}

public enum TransitionType
{
    SlideLeft,
    SlideRight,
    Fade,
    Scale
}

public enum PopupType
{
    Notification,
    Alert,
    BottomSheet,
    Tooltip
}

public enum HapticType
{
    Light,
    Medium,
    Heavy,
    Success,
    Warning,
    Error
}
