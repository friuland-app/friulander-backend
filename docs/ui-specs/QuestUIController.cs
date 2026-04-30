using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class QuestUIController : MonoBehaviour
{
    [Header("Tab Navigation")]
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private GameObject[] tabPanels;
    private int currentTab = 0;

    [Header("Daily Quests")]
    [SerializeField] private TextMeshProUGUI dailyTimer;
    [SerializeField] private Transform dailyQuestContainer;
    [SerializeField] private GameObject questCardPrefab;

    [Header("Weekly Quests")]
    [SerializeField] private TextMeshProUGUI weekDisplay;
    [SerializeField] private Transform weeklyQuestContainer;

    [Header("Story Quests")]
    [SerializeField] private Transform storyQuestContainer;

    [Header("Reward Popup")]
    [SerializeField] private GameObject rewardPopup;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private ParticleSystem confettiParticles;

    [Header("HUD Badge")]
    [SerializeField] private GameObject hudBadge;
    [SerializeField] private TextMeshProUGUI badgeCount;

    private System.DateTime dailyResetTime;
    private System.DateTime weeklyResetTime;

    private void Start()
    {
        SetupTabNavigation();
        LoadDailyQuests();
        LoadWeeklyQuests();
        LoadStoryQuests();
        StartTimerUpdate();
    }

    private void SetupTabNavigation()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int tabIndex = i;
            tabButtons[i].onClick.AddListener(() => SwitchTab(tabIndex));
        }
    }

    private void SwitchTab(int tabIndex)
    {
        if (currentTab == tabIndex) return;

        tabPanels[currentTab].GetComponent<CanvasGroup>().DOFade(0, 0.3f)
            .OnComplete(() => tabPanels[currentTab].SetActive(false));

        currentTab = tabIndex;
        tabPanels[currentTab].SetActive(true);
        tabPanels[currentTab].GetComponent<CanvasGroup>().alpha = 0;
        tabPanels[currentTab].GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        UpdateTabButtons();
    }

    private void UpdateTabButtons()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            bool isActive = i == currentTab;
            tabButtons[i].GetComponent<Image>().color = isActive ? 
                new Color(0.83f, 0.63f, 0.09f) : Color.white;
        }
    }

    private void LoadDailyQuests()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject card = Instantiate(questCardPrefab, dailyQuestContainer);
            int questIndex = i;
            card.GetComponent<Button>().onClick.AddListener(() => OnQuestCardClick(questIndex));
        }
    }

    private void LoadWeeklyQuests()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject card = Instantiate(questCardPrefab, weeklyQuestContainer);
        }
    }

    private void LoadStoryQuests()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject card = Instantiate(questCardPrefab, storyQuestContainer);
        }
    }

    private void StartTimerUpdate()
    {
        dailyResetTime = System.DateTime.Today.AddDays(1);
        weeklyResetTime = GetNextWeeklyReset();
        InvokeRepeating(nameof(UpdateTimers), 0f, 1f);
    }

    private System.DateTime GetNextWeeklyReset()
    {
        System.DateTime now = System.DateTime.Now;
        int daysUntilMonday = ((int)System.DayOfWeek.Monday - (int)now.DayOfWeek + 7) % 7;
        if (daysUntilMonday == 0 && now.Hour < 0) daysUntilMonday = 7;
        return now.AddDays(daysUntilMonday).Date;
    }

    private void UpdateTimers()
    {
        System.TimeSpan dailyTimeLeft = dailyResetTime - System.DateTime.Now;
        dailyTimer.text = $"Reset tra: {dailyTimeLeft.Hours:00}:{dailyTimeLeft.Minutes:00}:{dailyTimeLeft.Seconds:00}";

        System.TimeSpan weeklyTimeLeft = weeklyResetTime - System.DateTime.Now;
        weekDisplay.text = $"Settimana {System.DateTime.Now.DayOfYear / 7} - Reset in {weeklyTimeLeft.Days} giorni";
    }

    private void OnQuestCardClick(int questIndex)
    {
        Debug.Log($"Quest {questIndex} clicked");
    }

    public void OnClaimReward(int questIndex)
    {
        ShowRewardPopup(50);
        UpdateHUDBadge();
    }

    private void ShowRewardPopup(int xpAmount)
    {
        rewardPopup.SetActive(true);
        rewardPopup.transform.localScale = Vector3.zero;
        rewardPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        rewardText.text = $"+{xpAmount} XP";

        if (confettiParticles != null)
        {
            confettiParticles.Play();
        }

        StartCoroutine(HideRewardPopupAfterDelay());
    }

    private IEnumerator HideRewardPopupAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        rewardPopup.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InQuad)
            .OnComplete(() => rewardPopup.SetActive(false));
    }

    public void UpdateQuestProgress(int questIndex, int current, int max)
    {
        Debug.Log($"Quest {questIndex} progress: {current}/{max}");
    }

    public void UpdateHUDBadge()
    {
        int availableRewards = GetAvailableRewardCount();
        if (availableRewards > 0)
        {
            hudBadge.SetActive(true);
            badgeCount.text = availableRewards.ToString();
        }
        else
        {
            hudBadge.SetActive(false);
        }
    }

    private int GetAvailableRewardCount()
    {
        return Random.Range(0, 5);
    }

    public void ShowNewQuestNotification(string questTitle)
    {
        Debug.Log($"New quest: {questTitle}");
    }

    public void ShowQuestCompletedNotification(string questTitle)
    {
        Debug.Log($"Quest completed: {questTitle}");
    }
}
