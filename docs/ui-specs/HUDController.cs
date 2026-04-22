using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HUDController : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private Image playerAvatar;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerLevel;
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI xpText;

    [Header("Compass")]
    [SerializeField] private RectTransform compassNeedle;
    [SerializeField] private TextMeshProUGUI northLabel;

    [Header("Buttons")]
    [SerializeField] private Button arButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button profileButton;
    [SerializeField] private Button questsButton;

    [Header("Badges")]
    [SerializeField] private TextMeshProUGUI inventoryCount;
    [SerializeField] private TextMeshProUGUI questCount;
    [SerializeField] private GameObject arBadge;

    [Header("Notifications")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private Image notificationIcon;

    [Header("Settings")]
    [SerializeField] private float notificationDuration = 3f;

    private void Start()
    {
        SetupEventListeners();
        UpdatePlayerInfo();
        StartCompassUpdate();
    }

    private void SetupEventListeners()
    {
        arButton.onClick.AddListener(OnARButtonClick);
        inventoryButton.onClick.AddListener(OnInventoryClick);
        profileButton.onClick.AddListener(OnProfileClick);
        questsButton.onClick.AddListener(OnQuestsClick);
    }

    private void UpdatePlayerInfo()
    {
        // Update from player data
        playerName.text = "Giocatore";
        playerLevel.text = "Lv.15";
        xpBar.value = 0.75f;
        xpText.text = "1500/2000 XP";
    }

    private void StartCompassUpdate()
    {
        // Update compass based on player heading
        InvokeRepeating(nameof(UpdateCompass), 0f, 0.1f);
    }

    private void UpdateCompass()
    {
        float heading = Input.compass.trueHeading;
        compassNeedle.rotation = Quaternion.Euler(0, 0, -heading);
    }

    public void UpdateXP(int currentXP, int maxXP)
    {
        float targetValue = (float)currentXP / maxXP;
        xpBar.DOValue(targetValue, 0.5f).SetEase(Ease.OutQuad);
        xpText.text = $"{currentXP}/{maxXP} XP";
    }

    public void UpdateLevel(int level)
    {
        playerLevel.text = $"Lv.{level}";
        ShowNotification("Livello up!", NotificationType.LevelUp);
    }

    public void UpdateInventoryCount(int count)
    {
        inventoryCount.text = count.ToString();
        inventoryCount.transform.parent.gameObject.SetActive(count > 0);
    }

    public void UpdateQuestCount(int count)
    {
        questCount.text = count.ToString();
        questCount.transform.parent.gameObject.SetActive(count > 0);
    }

    public void ShowNotification(string message, NotificationType type)
    {
        notificationText.text = message;
        notificationIcon.color = GetNotificationColor(type);
        
        notificationPanel.SetActive(true);
        notificationPanel.transform.localPosition = new Vector3(0, -50f, 0);
        notificationPanel.transform.DOLocalMoveY(0f, 0.3f).SetEase(Ease.OutBack);

        StartCoroutine(HideNotificationAfterDelay());
    }

    private Color GetNotificationColor(NotificationType type)
    {
        return type switch
        {
            NotificationType.CreatureSpawn => new Color(0.49f, 0.23f, 0.93f),
            NotificationType.QuestComplete => new Color(0.83f, 0.63f, 0.09f),
            NotificationType.LevelUp => new Color(0.06f, 0.72f, 0.5f),
            NotificationType.POIDiscovered => new Color(0.29f, 0.56f, 0.89f),
            _ => Color.white
        };
    }

    private System.Collections.IEnumerator HideNotificationAfterDelay()
    {
        yield return new WaitForSeconds(notificationDuration);
        
        notificationPanel.transform.DOLocalMoveY(-50f, 0.3f).SetEase(Ease.InQuad)
            .OnComplete(() => notificationPanel.SetActive(false));
    }

    private void OnARButtonClick()
    {
        arButton.transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad)
            .OnComplete(() => arButton.transform.DOScale(1f, 0.1f));
        
        // Open AR mode
    }

    private void OnInventoryClick()
    {
        inventoryButton.transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad)
            .OnComplete(() => inventoryButton.transform.DOScale(1f, 0.1f));
        
        // Open inventory
    }

    private void OnProfileClick()
    {
        profileButton.transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad)
            .OnComplete(() => profileButton.transform.DOScale(1f, 0.1f));
        
        // Open profile
    }

    private void OnQuestsClick()
    {
        questsButton.transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad)
            .OnComplete(() => questsButton.transform.DOScale(1f, 0.1f));
        
        // Open quests
    }

    public void SetBattleMode(bool active)
    {
        questsButton.gameObject.SetActive(!active);
        inventoryButton.gameObject.SetActive(!active);
    }

    public void SetARMode(bool active)
    {
        gameObject.SetActive(!active);
    }
}

public enum NotificationType
{
    CreatureSpawn,
    QuestComplete,
    LevelUp,
    POIDiscovered
}
