using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI funFactText;
    [SerializeField] private Image logoImage;

    [Header("Settings")]
    [SerializeField] private float splashDuration = 2.5f;
    [SerializeField] private float minLoadingTime = 3f;
    [SerializeField] private float factChangeInterval = 3f;

    private AsyncOperation loadOperation;
    private float currentProgress;
    private bool isSplashComplete;
    private int currentFactIndex;

    private readonly string[] friuliFacts = new string[]
    {
        "Il Friuli è terra di confine e di incontro",
        "Il Tagliamento è il fiume sacro del Friuli",
        "Le Dolomiti sono patrimonio dell'UNESCO",
        "Il prosciutto di San Daniele è famoso nel mondo",
        "Il Friuli ha 3 lingue ufficiali: italiano, friulano, sloveno",
        "Udine è stata la capitale del Ducato del Friuli",
        "La regione ha 4 parchi naturali",
        "Il Collio è uno dei migliori vini italiani",
        "Cividale del Friuli è città d'arte",
        "Il Friuli Venezia Giulia ha 5 province"
    };

    private void Start()
    {
        StartCoroutine(LoadingSequence());
    }

    private IEnumerator LoadingSequence()
    {
        // Phase 1: Splash Screen
        yield return StartCoroutine(ShowSplashScreen());
        
        // Phase 2: Loading Screen
        yield return StartCoroutine(ShowLoadingScreen());
        
        // Phase 3: Load Main Scene
        yield return StartCoroutine(LoadMainScene());
    }

    private IEnumerator ShowSplashScreen()
    {
        splashScreen.SetActive(true);
        loadingScreen.SetActive(false);
        
        // Logo animation
        yield return StartCoroutine(AnimateLogo());
        
        // Wait for splash duration
        yield return new WaitForSeconds(splashDuration);
        
        // Fade out
        yield return StartCoroutine(FadeOutSplash());
        
        splashScreen.SetActive(false);
        isSplashComplete = true;
    }

    private IEnumerator AnimateLogo()
    {
        logoImage.transform.localScale = Vector3.zero;
        Color logoColor = logoImage.color;
        logoColor.a = 0;
        logoImage.color = logoColor;

        float duration = 1.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            
            // Scale animation
            logoImage.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);
            
            // Fade in
            logoColor.a = Mathf.Lerp(0, 1, progress);
            logoImage.color = logoColor;
            
            yield return null;
        }

        // Subtle pulse
        yield return StartCoroutine(PulseLogo());
    }

    private IEnumerator PulseLogo()
    {
        Vector3 originalScale = Vector3.one;
        float pulseDuration = 0.5f;
        float elapsed = 0;

        while (elapsed < pulseDuration)
        {
            elapsed += Time.deltaTime;
            float scale = 1 + Mathf.Sin(elapsed * Mathf.PI * 2) * 0.05f;
            logoImage.transform.localScale = originalScale * scale;
            yield return null;
        }

        logoImage.transform.localScale = originalScale;
    }

    private IEnumerator FadeOutSplash()
    {
        CanvasGroup canvasGroup = splashScreen.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = splashScreen.AddComponent<CanvasGroup>();
        }

        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1 - (elapsed / duration);
            yield return null;
        }
    }

    private IEnumerator ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
        currentProgress = 0;
        currentFactIndex = Random.Range(0, friuliFacts.Length);
        
        // Start fun fact rotation
        StartCoroutine(RotateFunFacts());
        
        // Start resource preloading
        yield return StartCoroutine(PreloadResources());
    }

    private IEnumerator RotateFunFacts()
    {
        while (true)
        {
            funFactText.text = friuliFacts[currentFactIndex];
            yield return StartCoroutine(FadeInText(funFactText));
            yield return new WaitForSeconds(factChangeInterval);
            yield return StartCoroutine(FadeOutText(funFactText));
            
            currentFactIndex = (currentFactIndex + 1) % friuliFacts.Length;
        }
    }

    private IEnumerator FadeInText(TextMeshProUGUI text)
    {
        Color textColor = text.color;
        textColor.a = 0;
        text.color = textColor;

        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            textColor.a = Mathf.Lerp(0, 1, elapsed / duration);
            text.color = textColor;
            yield return null;
        }
    }

    private IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        Color textColor = text.color;
        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            textColor.a = Mathf.Lerp(1, 0, elapsed / duration);
            text.color = textColor;
            yield return null;
        }
    }

    private IEnumerator PreloadResources()
    {
        // Phase 1: Essential (0-30%)
        yield return StartCoroutine(PreloadEssentialAssets());
        
        // Phase 2: Secondary (30-70%)
        yield return StartCoroutine(PreloadSecondaryAssets());
        
        // Phase 3: Tertiary (70-100%)
        yield return StartCoroutine(PreloadTertiaryAssets());
    }

    private IEnumerator PreloadEssentialAssets()
    {
        loadingText.text = "Caricamento risorse essenziali...";
        
        // Simulate loading
        for (float i = 0; i <= 30; i += 1)
        {
            currentProgress = i;
            UpdateProgressBar();
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator PreloadSecondaryAssets()
    {
        loadingText.text = "Caricamento creature e mappe...";
        
        for (float i = 30; i <= 70; i += 1)
        {
            currentProgress = i;
            UpdateProgressBar();
            yield return new WaitForSeconds(0.015f);
        }
    }

    private IEnumerator PreloadTertiaryAssets()
    {
        loadingText.text = "Caricamento animazioni...";
        
        for (float i = 70; i <= 100; i += 1)
        {
            currentProgress = i;
            UpdateProgressBar();
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void UpdateProgressBar()
    {
        progressBar.fillAmount = currentProgress / 100f;
    }

    private IEnumerator LoadMainScene()
    {
        loadingText.text = "Connessione al server...";
        
        loadOperation = SceneManager.LoadSceneAsync("MainScene");
        loadOperation.allowSceneActivation = false;

        while (!loadOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            progressBar.fillAmount = progress;
            
            if (loadOperation.progress >= 0.9f)
            {
                loadingText.text = "Pronto!";
                yield return new WaitForSeconds(0.5f);
                loadOperation.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
}
