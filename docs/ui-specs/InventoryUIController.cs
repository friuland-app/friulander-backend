using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class InventoryUIController : MonoBehaviour
{
    [Header("Tab Navigation")]
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private GameObject[] tabPanels;
    private int currentTab = 0;

    [Header("Creature Tab")]
    [SerializeField] private Transform creatureGrid;
    [SerializeField] private GameObject creatureCardPrefab;
    [SerializeField] private TMP_Dropdown typeFilter;
    [SerializeField] private TMP_Dropdown rarityFilter;
    [SerializeField] private TMP_Dropdown levelFilter;

    [Header("Detail Panel")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private RectTransform creature3DModel;
    [SerializeField] private TextMeshProUGUI creatureName;
    [SerializeField] private TextMeshProUGUI creatureStats;
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private Button evolutionButton;

    [Header("Animations")]
    [SerializeField] private float tabSwitchDuration = 0.3f;
    [SerializeField] private float detailPanelDuration = 0.4f;

    private void Start()
    {
        SetupTabNavigation();
        LoadCreatures();
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

        // Animate out current tab
        tabPanels[currentTab].GetComponent<CanvasGroup>().DOFade(0, tabSwitchDuration)
            .OnComplete(() => tabPanels[currentTab].SetActive(false));

        // Animate in new tab
        currentTab = tabIndex;
        tabPanels[currentTab].SetActive(true);
        tabPanels[currentTab].GetComponent<CanvasGroup>().alpha = 0;
        tabPanels[currentTab].GetComponent<CanvasGroup>().DOFade(1, tabSwitchDuration);

        // Update tab button states
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

    private void LoadCreatures()
    {
        // Load creature cards from data
        for (int i = 0; i < 12; i++)
        {
            GameObject card = Instantiate(creatureCardPrefab, creatureGrid);
            int cardIndex = i;
            card.GetComponent<Button>().onClick.AddListener(() => OnCreatureCardClick(cardIndex));
        }
    }

    private void OnCreatureCardClick(int cardIndex)
    {
        ShowCreatureDetail(cardIndex);
    }

    private void ShowCreatureDetail(int creatureIndex)
    {
        detailPanel.SetActive(true);
        detailPanel.transform.localPosition = new Vector3(0, -300f, 0);
        detailPanel.transform.DOLocalMoveY(0, detailPanelDuration).SetEase(Ease.OutBack);

        // Update creature data
        creatureName.text = $"Creatura {creatureIndex + 1}";
        creatureStats.text = "ATK: 50 DEF: 30 SPD: 45";
        xpBar.value = 0.75f;
        xpText.text = "1500/2000 XP";

        // Check evolution availability
        bool canEvolve = Random.value > 0.5f;
        evolutionButton.interactable = canEvolve;
    }

    public void HideDetailPanel()
    {
        detailPanel.transform.DOLocalMoveY(-300f, detailPanelDuration).SetEase(Ease.InQuad)
            .OnComplete(() => detailPanel.SetActive(false));
    }

    public void OnFilterChange()
    {
        // Filter creatures based on selected filters
        string selectedType = typeFilter.options[typeFilter.value].text;
        string selectedRarity = rarityFilter.options[rarityFilter.value].text;
        string selectedLevel = levelFilter.options[levelFilter.value].text;

        // Apply filters and refresh grid
        RefreshCreatureGrid();
    }

    private void RefreshCreatureGrid()
    {
        // Clear and reload creature cards with filters applied
        foreach (Transform child in creatureGrid)
        {
            Destroy(child.gameObject);
        }
        LoadCreatures();
    }

    public void OnEvolutionClick()
    {
        evolutionButton.transform.DOScale(0.95f, 0.1f)
            .OnComplete(() => evolutionButton.transform.DOScale(1f, 0.1f));

        // Play evolution animation
        creature3DModel.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.InOutQuad);

        // Show evolution success
        Invoke(nameof(ShowEvolutionSuccess), 1f);
    }

    private void ShowEvolutionSuccess()
    {
        // Show success notification
        Debug.Log("Evoluzione completata!");
    }

    public void RotateCreatureModel(Vector2 rotationDelta)
    {
        creature3DModel.Rotate(Vector3.up, rotationDelta.x * 0.5f);
    }
}
