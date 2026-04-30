using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProfileUIController : MonoBehaviour
{
    [Header("Profile Elements")]
    [SerializeField] private Image playerAvatar;
    [SerializeField] private Button editAvatarButton;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerLevel;
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI regionalTitle;

    [Header("Statistics")]
    [SerializeField] private TextMeshProUGUI creaturesCaught;
    [SerializeField] private TextMeshProUGUI kmTraveled;
    [SerializeField] private TextMeshProUGUI battlesFought;
    [SerializeField] private TextMeshProUGUI battlesWon;

    [Header("Medals")]
    [SerializeField] private Transform medalsGrid;
    [SerializeField] private GameObject medalPrefab;

    [Header("Action Buttons")]
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button shareButton;

    [Header("Share Sheet")]
    [SerializeField] private GameObject shareSheet;
    [SerializeField] private Button[] shareOptions;

    private void Start()
    {
        SetupEventListeners();
        LoadProfileData();
        LoadMedals();
    }

    private void SetupEventListeners()
    {
        editAvatarButton.onClick.AddListener(OnEditAvatarClick);
        leaderboardButton.onClick.AddListener(OnLeaderboardClick);
        shareButton.onClick.AddListener(OnShareClick);

        for (int i = 0; i < shareOptions.Length; i++)
        {
            int optionIndex = i;
            shareOptions[i].onClick.AddListener(() => OnShareOptionClick(optionIndex));
        }
    }

    private void LoadProfileData()
    {
        playerName.text = "Giocatore";
        playerLevel.text = "Livello 25";
        xpBar.value = 0.83f;
        xpText.text = "2500/3000 XP";
        regionalTitle.text = "Campione del Friuli";

        creaturesCaught.text = "150";
        kmTraveled.text = "125.5";
        battlesFought.text = "89";
        battlesWon.text = "67";
    }

    private void LoadMedals()
    {
        // Load medals from player data
        for (int i = 0; i < 8; i++)
        {
            GameObject medal = Instantiate(medalPrefab, medalsGrid);
            bool isUnlocked = i < 5;
            medal.GetComponent<Image>().color = isUnlocked ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }

    public void UpdateXP(int currentXP, int maxXP)
    {
        float targetValue = (float)currentXP / maxXP;
        xpBar.DOValue(targetValue, 0.5f).SetEase(Ease.OutQuad);
        xpText.text = $"{currentXP}/{maxXP} XP";
    }

    public void OnLevelUp(int newLevel)
    {
        playerLevel.text = $"Livello {newLevel}";
        playerLevel.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.5f, 5, 1);
        
        // Play confetti effect
        Debug.Log("Level up animation!");
    }

    private void OnEditAvatarClick()
    {
        editAvatarButton.transform.DOScale(0.9f, 0.1f)
            .OnComplete(() => editAvatarButton.transform.DOScale(1f, 0.1f));
        
        // Open avatar editor
        Debug.Log("Edit avatar");
    }

    private void OnLeaderboardClick()
    {
        leaderboardButton.transform.DOScale(0.95f, 0.1f)
            .OnComplete(() => leaderboardButton.transform.DOScale(1f, 0.1f));
        
        // Open leaderboard screen
        Debug.Log("Open leaderboard");
    }

    private void OnShareClick()
    {
        shareButton.transform.DOScale(0.95f, 0.1f)
            .OnComplete(() => shareButton.transform.DOScale(1f, 0.1f));
        
        ShowShareSheet();
    }

    private void ShowShareSheet()
    {
        shareSheet.SetActive(true);
        shareSheet.transform.localPosition = new Vector3(0, -300f, 0);
        shareSheet.transform.DOLocalMoveY(0, 0.4f).SetEase(Ease.OutBack);
    }

    public void HideShareSheet()
    {
        shareSheet.transform.DOLocalMoveY(-300f, 0.4f).SetEase(Ease.InQuad)
            .OnComplete(() => shareSheet.SetActive(false));
    }

    private void OnShareOptionClick(int optionIndex)
    {
        Debug.Log($"Share option {optionIndex} clicked");
        HideShareSheet();
    }

    public void UpdateStat(string statName, int value)
    {
        switch (statName)
        {
            case "creatures":
                creaturesCaught.text = value.ToString();
                break;
            case "km":
                kmTraveled.text = value.ToString();
                break;
            case "battles":
                battlesFought.text = value.ToString();
                break;
            case "wins":
                battlesWon.text = value.ToString();
                break;
        }
    }
}
