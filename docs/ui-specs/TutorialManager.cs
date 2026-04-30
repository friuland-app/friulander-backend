using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("Tutorial Panels")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Image tutorialIllustration;
    [SerializeField] private TextMeshProUGUI tutorialTitle;
    [SerializeField] private TextMeshProUGUI tutorialDescription;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("Contextual Tooltips")]
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private Image tooltipIcon;

    [Header("Tutorial Steps")]
    [SerializeField] private GameObject[] stepHighlights;

    private int currentStep = 0;
    private const int TOTAL_STEPS = 5;
    private bool isTutorialCompleted = false;
    private bool isTutorialSkipped = false;

    private readonly string[] stepTitles = new string[]
    {
        "Benvenuto in Friulander!",
        "Esplora la Mappa",
        "La tua Prima Creatura!",
        "Cattura in AR",
        "Inventario e Missioni"
    };

    private readonly string[] stepDescriptions = new string[]
    {
        "Il Friuli è una terra magica abitata da creature straordinarie. Diventa il miglior allenatore e scopri i segreti della regione.",
        "Usa il GPS per trovare creature e POI. Muoviti nel mondo reale per scoprire nuovi luoghi.",
        "C'è una creatura nelle vicinanze! Segui l'indicatore per trovarla.",
        "Usa la fotocamera per vedere le creature nel mondo reale. Lancia una trappola per catturarle!",
        "Gestisci le tue creature nell'inventario e completa missioni per guadagnare ricompense."
    };

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
        LoadTutorialState();
        SetupEventListeners();

        if (!isTutorialCompleted && !isTutorialSkipped)
        {
            StartTutorial();
        }
    }

    private void LoadTutorialState()
    {
        isTutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;
        isTutorialSkipped = PlayerPrefs.GetInt("TutorialSkipped", 0) == 1;
        currentStep = PlayerPrefs.GetInt("TutorialStep", 0);
    }

    private void SetupEventListeners()
    {
        continueButton.onClick.AddListener(OnContinueClick);
        skipButton.onClick.AddListener(OnSkipClick);
    }

    public void StartTutorial()
    {
        currentStep = 0;
        ShowTutorialStep(currentStep);
    }

    public void ShowTutorialStep(int stepIndex)
    {
        if (stepIndex >= TOTAL_STEPS)
        {
            CompleteTutorial();
            return;
        }

        tutorialPanel.SetActive(true);
        tutorialTitle.text = stepTitles[stepIndex];
        tutorialDescription.text = stepDescriptions[stepIndex];
        progressText.text = $"{stepIndex + 1}/{TOTAL_STEPS}";

        // Highlight relevant UI element
        HighlightStepElement(stepIndex);

        // Show illustration (placeholder)
        tutorialIllustration.color = GetStepColor(stepIndex);

        // Animate panel in
        tutorialPanel.transform.localScale = Vector3.zero;
        tutorialPanel.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
    }

    private Color GetStepColor(int stepIndex)
    {
        return stepIndex switch
        {
            0 => new Color(0.83f, 0.63f, 0.09f), // Gold
            1 => new Color(0.29f, 0.56f, 0.89f), // Blue
            2 => new Color(0.06f, 0.72f, 0.5f), // Green
            3 => new Color(0.49f, 0.23f, 0.93f), // Purple
            4 => new Color(0.94f, 0.27f, 0.27f), // Red
            _ => Color.white
        };
    }

    private void HighlightStepElement(int stepIndex)
    {
        // Reset all highlights
        foreach (GameObject highlight in stepHighlights)
        {
            if (highlight != null)
            {
                highlight.SetActive(false);
            }
        }

        // Highlight current step element
        if (stepIndex < stepHighlights.Length && stepHighlights[stepIndex] != null)
        {
            stepHighlights[stepIndex].SetActive(true);
            stepHighlights[stepIndex].transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.5f, 5, 1)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnContinueClick()
    {
        continueButton.transform.DOScale(0.95f, 0.1f)
            .OnComplete(() => continueButton.transform.DOScale(1f, 0.1f));

        currentStep++;
        PlayerPrefs.SetInt("TutorialStep", currentStep);

        if (currentStep < TOTAL_STEPS)
        {
            ShowTutorialStep(currentStep);
        }
        else
        {
            CompleteTutorial();
        }
    }

    private void OnSkipClick()
    {
        skipButton.transform.DOScale(0.95f, 0.1f)
            .OnComplete(() => skipButton.transform.DOScale(1f, 0.1f));

        isTutorialSkipped = true;
        PlayerPrefs.SetInt("TutorialSkipped", 1);
        HideTutorial();
    }

    private void CompleteTutorial()
    {
        isTutorialCompleted = true;
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.SetInt("TutorialStep", 0);

        HideTutorial();
        ShowCompletionCelebration();
    }

    private void HideTutorial()
    {
        tutorialPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InQuad)
            .OnComplete(() => tutorialPanel.SetActive(false));

        // Stop all highlights
        foreach (GameObject highlight in stepHighlights)
        {
            if (highlight != null)
            {
                highlight.transform.DOKill();
                highlight.SetActive(false);
            }
        }
    }

    private void ShowCompletionCelebration()
    {
        // Show completion notification
        Debug.Log("Tutorial completato!");
    }

    public void ResetTutorial()
    {
        isTutorialCompleted = false;
        isTutorialSkipped = false;
        currentStep = 0;

        PlayerPrefs.SetInt("TutorialCompleted", 0);
        PlayerPrefs.SetInt("TutorialSkipped", 0);
        PlayerPrefs.SetInt("TutorialStep", 0);

        StartTutorial();
    }

    #region Contextual Tooltips

    public void ShowTooltip(string text, string iconName, float duration = 3f)
    {
        if (isTutorialCompleted) return;

        tooltipText.text = text;
        tooltipPanel.SetActive(true);

        tooltipPanel.transform.localScale = Vector3.zero;
        tooltipPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);

        StartCoroutine(HideTooltipAfterDelay(duration));
    }

    private IEnumerator HideTooltipAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);

        tooltipPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InQuad)
            .OnComplete(() => tooltipPanel.SetActive(false));
    }

    public void ShowMapTooltip()
    {
        ShowTooltip("Tocca per vedere la mappa", "map", 3f);
    }

    public void ShowARTooltip()
    {
        ShowTooltip("Tocca per attivare la AR", "camera", 3f);
    }

    public void ShowCreatureTooltip()
    {
        ShowTooltip("Tocca per vedere i dettagli", "creature", 5f);
    }

    public void ShowQuestTooltip()
    {
        ShowTooltip("Nuova missione disponibile!", "scroll", 5f);
    }

    public void ShowInventoryTooltip()
    {
        ShowTooltip("Tocca per vedere l'inventario", "backpack", 3f);
    }

    #endregion

    #region Tutorial State

    public bool IsTutorialCompleted()
    {
        return isTutorialCompleted;
    }

    public bool IsTutorialSkipped()
    {
        return isTutorialSkipped;
    }

    public int GetCurrentStep()
    {
        return currentStep;
    }

    public float GetProgress()
    {
        return (float)currentStep / TOTAL_STEPS;
    }

    #endregion
}
