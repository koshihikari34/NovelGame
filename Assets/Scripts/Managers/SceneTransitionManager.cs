using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1.0f;
    public Color fadeColor = Color.black;
    
    private static SceneTransitionManager instance;
    public static SceneTransitionManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("SceneTransitionManager");
                instance = go.AddComponent<SceneTransitionManager>();
                DontDestroyOnLoad(go);
                
                // フェード用のCanvasとImageを作成
                instance.CreateFadeUI();
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CreateFadeUI();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void CreateFadeUI()
    {
        // フェード用のCanvasを作成
        GameObject canvasObject = new GameObject("FadeCanvas");
        canvasObject.transform.SetParent(transform);
        
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // 最前面に表示
        
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // フェード用のImageを作成
        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(canvasObject.transform, false);
        
        fadeImage = imageObject.AddComponent<Image>();
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        
        // 画面全体を覆うようにサイズを設定
        RectTransform rectTransform = imageObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
    
    /// <summary>
    /// フェード付きシーン遷移
    /// </summary>
    /// <param name="sceneName">遷移先のシーン名</param>
    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    
    /// <summary>
    /// フェード付きシーン遷移（コルーチン）
    /// </summary>
    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // フェードアウト
        yield return StartCoroutine(FadeOut());
        
        // シーン読み込み
        yield return SceneManager.LoadSceneAsync(sceneName);
        
        // フェードイン
        yield return StartCoroutine(FadeIn());
    }
    
    /// <summary>
    /// フェードアウト（画面を暗くする）
    /// </summary>
    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color startColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        Color endColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            fadeImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        
        fadeImage.color = endColor;
    }
    
    /// <summary>
    /// フェードイン（画面を明るくする）
    /// </summary>
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color startColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
        Color endColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            fadeImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        
        fadeImage.color = endColor;
    }
}