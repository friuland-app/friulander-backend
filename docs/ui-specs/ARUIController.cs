using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ARUIController : MonoBehaviour
{
    [Header("AR Elements")]
    public RectTransform crosshair;
    public Image[] stabilityDots;
    public TextMeshProUGUI stabilityText;
    public GameObject creatureHealthBar;
    public Slider healthSlider;

    [Header("Result Screens")]
    public GameObject successOverlay;
    public GameObject failureOverlay;
    public ParticleSystem confettiParticles;

    private bool isTargetLocked = false;

    private void Start()
    {
        StartCrosshairAnimation();
    }

    private void StartCrosshairAnimation()
    {
        crosshair.DORotate(new Vector3(0, 0, 360), 30f, RotateMode.WorldAxisAdd)
            .SetLoops(-1, LoopType.Incremental);
    }

    public void UpdateStability(int level)
    {
        for (int i = 0; i < stabilityDots.Length; i++)
        {
            stabilityDots[i].color = i < level ? Color.green : Color.gray;
        }
    }

    public void SetTargetLocked(bool locked)
    {
        isTargetLocked = locked;
        creatureHealthBar.SetActive(locked);
    }

    public void OnSwipeDetected()
    {
        if (!isTargetLocked) return;
        crosshair.DOScale(1.5f, 0.2f).OnComplete(() => crosshair.DOScale(1f, 0.2f));
        Invoke(nameof(ShowCaptureResult), 1f);
    }

    private void ShowCaptureResult()
    {
        bool success = Random.value > 0.3f;
        if (success) ShowSuccess();
        else ShowFailure();
    }

    private void ShowSuccess()
    {
        successOverlay.SetActive(true);
        confettiParticles.Play();
    }

    private void ShowFailure()
    {
        failureOverlay.SetActive(true);
    }
}
