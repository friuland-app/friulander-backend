using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class POIUIController : MonoBehaviour
{
    [Header("Proximity Popup")]
    [SerializeField] private GameObject proximityPopup;
    [SerializeField] private TextMeshProUGUI poiName;
    [SerializeField] private TextMeshProUGUI poiDistance;
    [SerializeField] private Button exploreButton;
    [SerializeField] private Button ignoreButton;

    [Header("Detail Card")]
    [SerializeField] private GameObject detailCard;
    [SerializeField] private Transform photoGallery;
    [SerializeField] private TextMeshProUGUI historicalDescription;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button encyclopediaButton;

    [Header("Distance Indicator")]
    [SerializeField] private GameObject distanceIndicator;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private Image directionArrow;

    [Header("Encyclopedia")]
    [SerializeField] private GameObject encyclopediaPanel;
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private Button[] categoryButtons;
    [SerializeField] private Transform poiGrid;
    [SerializeField] private GameObject poiCardPrefab;

    [Header("Effects")]
    [SerializeField] private ParticleSystem discoveryParticles;
    [SerializeField] private ParticleSystem completionParticles;

    private POI currentPOI;
    private float currentDistance;

    private void Start()
    {
        SetupEventListeners();
        HideAllPanels();
    }

    private void SetupEventListeners()
    {
        exploreButton.onClick.AddListener(OnExploreClick);
        ignoreButton.onClick.AddListener(OnIgnoreClick);
        interactButton.onClick.AddListener(OnInteractClick);
        encyclopediaButton.onClick.AddListener(OnEncyclopediaClick);

        for (int i = 0; i < categoryButtons.Length; i++)
        {
            int categoryIndex = i;
            categoryButtons[i].onClick.AddListener(() => OnCategoryFilterClick(categoryIndex));
        }
    }

    public void ShowProximityPopup(POI poi, float distance)
    {
        currentPOI = poi;
        currentDistance = distance;

        poiName.text = poi.name;
        poiDistance.text = $"{distance:F0}m";

        proximityPopup.SetActive(true);
        proximityPopup.transform.localPosition = new Vector3(0, 300f, 0);
        proximityPopup.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutBack);

        Invoke(nameof(HideProximityPopup), 10f);
    }

    private void HideProximityPopup()
    {
        proximityPopup.transform.DOLocalMoveY(300f, 0.5f).SetEase(Ease.InQuad)
            .OnComplete(() => proximityPopup.SetActive(false));
    }

    private void OnExploreClick()
    {
        HideProximityPopup();
        ShowDetailCard(currentPOI);
    }

    private void OnIgnoreClick()
    {
        HideProximityPopup();
    }

    public void ShowDetailCard(POI poi)
    {
        currentPOI = poi;

        historicalDescription.text = poi.description;
        progressSlider.value = poi.progress;
        progressText.text = $"{poi.progress:F0}% completato";

        detailCard.SetActive(true);
        detailCard.transform.localPosition = new Vector3(0, -Screen.height * 0.7f, 0);
        detailCard.transform.DOLocalMoveY(0, 0.4f).SetEase(Ease.OutBack);
    }

    public void HideDetailCard()
    {
        detailCard.transform.DOLocalMoveY(-Screen.height * 0.7f, 0.4f).SetEase(Ease.InQuad)
            .OnComplete(() => detailCard.SetActive(false));
    }

    private void OnInteractClick()
    {
        interactButton.transform.DOScale(0.95f, 0.1f)
            .OnComplete(() => interactButton.transform.DOScale(1f, 0.1f));

        PlayDiscoveryAnimation();
        UpdateProgress();
    }

    private void PlayDiscoveryAnimation()
    {
        if (discoveryParticles != null)
        {
            discoveryParticles.Play();
        }

        Camera.main.transform.DOShakePosition(0.3f, 0.1f, 10, 90f, false);
    }

    private void UpdateProgress()
    {
        currentPOI.progress = Mathf.Min(currentPOI.progress + 25f, 100f);
        progressSlider.DOValue(currentPOI.progress / 100f, 0.5f).SetEase(Ease.OutQuad);
        progressText.text = $"{currentPOI.progress:F0}% completato";

        if (currentPOI.progress >= 100f)
        {
            OnPOICompleted();
        }
    }

    private void OnPOICompleted()
    {
        if (completionParticles != null)
        {
            completionParticles.Play();
        }

        HideDetailCard();
        Debug.Log("POI completato!");
    }

    public void UpdateDistanceIndicator(float distance, Vector3 direction)
    {
        distanceText.text = $"{distance:F0}m";
        distanceIndicator.SetActive(distance < 500f);

        if (distance < 100f)
        {
            directionArrow.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.5f, 5, 1)
                .SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            directionArrow.transform.DOKill();
        }

        // Rotate arrow toward POI
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        directionArrow.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void OnEncyclopediaClick()
    {
        HideDetailCard();
        ShowEncyclopedia();
    }

    private void ShowEncyclopedia()
    {
        encyclopediaPanel.SetActive(true);
        LoadPOICards();
    }

    public void HideEncyclopedia()
    {
        encyclopediaPanel.SetActive(false);
    }

    private void LoadPOICards()
    {
        foreach (Transform child in poiGrid)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject card = Instantiate(poiCardPrefab, poiGrid);
            int cardIndex = i;
            card.GetComponent<Button>().onClick.AddListener(() => OnPOICardClick(cardIndex));
        }
    }

    private void OnPOICardClick(int cardIndex)
    {
        Debug.Log($"POI card {cardIndex} clicked");
    }

    private void OnCategoryFilterClick(int categoryIndex)
    {
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            bool isSelected = i == categoryIndex;
            categoryButtons[i].GetComponent<Image>().color = isSelected ? 
                new Color(0.18f, 0.35f, 0.15f) : Color.white;
        }

        LoadPOICards();
    }

    public void OnSearchChanged(string searchText)
    {
        // Filter POI cards based on search
        LoadPOICards();
    }
}

public class POI
{
    public string name;
    public string description;
    public float progress;
    public bool isDiscovered;
    public bool isCompleted;
}
