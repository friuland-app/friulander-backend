using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BattleUIController : MonoBehaviour
{
    [Header("Battle Elements")]
    [SerializeField] private RectTransform playerCreature;
    [SerializeField] private RectTransform enemyCreature;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private TextMeshProUGUI turnIndicator;

    [Header("Move Menu")]
    [SerializeField] private Button[] moveButtons;
    [SerializeField] private Button fleeButton;

    [Header("Action Log")]
    [SerializeField] private TextMeshProUGUI actionLog;
    [SerializeField] private int maxLogEntries = 5;

    [Header("Particles")]
    [SerializeField] private ParticleSystem attackParticles;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private ParticleSystem healingParticles;
    [SerializeField] private ParticleSystem specialParticles;

    private bool isPlayerTurn = true;
    private System.Collections.Generic.List<string> logEntries = new System.Collections.Generic.List<string>();

    private void Start()
    {
        SetupEventListeners();
        UpdateTurnIndicator();
    }

    private void SetupEventListeners()
    {
        for (int i = 0; i < moveButtons.Length; i++)
        {
            int moveIndex = i;
            moveButtons[i].onClick.AddListener(() => OnMoveClick(moveIndex));
        }
        fleeButton.onClick.AddListener(OnFleeClick);
    }

    public void SetPlayerTurn(bool isPlayerTurn)
    {
        this.isPlayerTurn = isPlayerTurn;
        UpdateTurnIndicator();
        UpdateMoveButtons();
    }

    private void UpdateTurnIndicator()
    {
        turnIndicator.text = isPlayerTurn ? "Turno: TU" : "Turno: NEMICO";
        turnIndicator.color = isPlayerTurn ? Color.green : Color.red;
        
        turnIndicator.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.3f);
    }

    private void UpdateMoveButtons()
    {
        foreach (Button button in moveButtons)
        {
            button.interactable = isPlayerTurn;
        }
        fleeButton.interactable = isPlayerTurn;
    }

    public void UpdatePlayerHealth(int current, int max)
    {
        float targetValue = (float)current / max;
        playerHealthBar.DOValue(targetValue, 0.3f).SetEase(Ease.OutQuad);
        playerHealthText.text = $"{current}/{max}";
    }

    public void UpdateEnemyHealth(int current, int max)
    {
        float targetValue = (float)current / max;
        enemyHealthBar.DOValue(targetValue, 0.3f).SetEase(Ease.OutQuad);
        enemyHealthText.text = $"{current}/{max}";
    }

    public void AddLogEntry(string message, LogEntryType type = LogEntryType.Normal)
    {
        logEntries.Add(message);
        if (logEntries.Count > maxLogEntries)
        {
            logEntries.RemoveAt(0);
        }

        Color logColor = type switch
        {
            LogEntryType.Damage => Color.red,
            LogEntryType.Healing => Color.green,
            LogEntryType.Special => new Color(0.49f, 0.23f, 0.93f),
            _ => Color.white
        };

        actionLog.text = string.Join("\n", logEntries);
    }

    public void PlayAttackAnimation(bool isPlayer)
    {
        RectTransform creature = isPlayer ? playerCreature : enemyCreature;
        Vector3 originalPos = creature.localPosition;
        Vector3 targetPos = isPlayer ? originalPos + Vector3.right * 50 : originalPos + Vector3.left * 50;

        creature.DOLocalMove(targetPos, 0.2f).SetEase(Ease.OutQuad)
            .OnComplete(() => creature.DOLocalMove(originalPos, 0.3f).SetEase(Ease.InQuad));

        if (attackParticles != null)
        {
            attackParticles.Play();
        }
    }

    public void PlayDamageAnimation(bool isPlayer)
    {
        RectTransform creature = isPlayer ? playerCreature : enemyCreature;
        
        creature.DOColor(Color.red, 0.1f)
            .OnComplete(() => creature.DOColor(Color.white, 0.1f));

        creature.DOShakePosition(0.2f, 5f, 10, 90f, false);

        if (damageParticles != null)
        {
            damageParticles.Play();
        }
    }

    public void PlayHealingAnimation(bool isPlayer)
    {
        RectTransform creature = isPlayer ? playerCreature : enemyCreature;
        
        creature.DOColor(Color.green, 0.2f)
            .OnComplete(() => creature.DOColor(Color.white, 0.2f));

        if (healingParticles != null)
        {
            healingParticles.Play();
        }
    }

    public void PlaySpecialMoveAnimation()
    {
        if (specialParticles != null)
        {
            specialParticles.Play();
        }

        Camera.main.transform.DOShakePosition(0.5f, 0.2f, 10, 90f, false);
    }

    private void OnMoveClick(int moveIndex)
    {
        AddLogEntry($"Usa Mossa {moveIndex + 1}", LogEntryType.Normal);
        PlayAttackAnimation(true);
    }

    private void OnFleeClick()
    {
        AddLogEntry("Tentativo di fuga...", LogEntryType.Special);
        fleeButton.transform.DOScale(0.95f, 0.1f)
            .OnComplete(() => fleeButton.transform.DOScale(1f, 0.1f));
    }
}

public enum LogEntryType
{
    Normal,
    Damage,
    Healing,
    Special
}
