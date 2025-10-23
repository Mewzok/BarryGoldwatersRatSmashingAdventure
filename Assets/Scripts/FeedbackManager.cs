using UnityEngine;
using System.Collections;
using TMPro;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    public GameObject burstPrefab;

    [Header("Hit Indicator")]
    public GameObject hitIndicatorPrefab;
    public Transform canvasTransform;

    // gradients
    public TMP_ColorGradient perfectGradient;
    public TMP_ColorGradient goodGradient;
    public TMP_ColorGradient okGradient;
    public TMP_ColorGradient missGradient;

    // fonts
    public TMP_FontAsset perfectFont;
    public TMP_FontAsset defaultFont;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // rat smash effects
    public void PlaySmashEffects(Vector3 position) {
        if(burstPrefab != null) {
            Instantiate(burstPrefab, position, Quaternion.identity);
        }

        StartCoroutine(CameraShake(0.1f, 0.05f));
    }

    IEnumerator CameraShake(float duration, float magnitude) {
        var cam = Camera.main;
        Vector3 originalPos = cam.transform.position;
        float elapsed = 0f;

        while(elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            cam.transform.position = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = originalPos;
    }

    // hit indicator text
    void Awake() {
        Instance = this;
    }

    public void ShowHitIndicator(string text, Vector3 worldPos) {
        var indicatorObj = Instantiate(hitIndicatorPrefab, canvasTransform);

        // set text
        var tmp = indicatorObj.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        
        // create animator and store reference
        var anim = indicatorObj.AddComponent<HitIndicatorAnimator>();

        // set color based on hit timing
        switch(text) {
            case "Miss":
                tmp.colorGradientPreset = missGradient;
                tmp.font = defaultFont;
                anim.initialScale = 1.0f;
                anim.targetScale = 0.8f;
                break;
            case "OK":
                tmp.colorGradientPreset = okGradient;
                tmp.font = defaultFont;
                anim.initialScale = 0.8f;
                anim.targetScale = 1.0f;
                break;
            case "Good":
                tmp.colorGradientPreset = goodGradient;
                tmp.font = defaultFont;
                anim.initialScale = 0.9f;
                anim.targetScale = 1.2f;
                break;
            case "Perfect": 
                tmp.colorGradientPreset = perfectGradient;
                tmp.font = perfectFont;
                anim.initialScale = 0.9f;
                anim.targetScale = 1.4f;
                break;
        }

        // convert world position to screen position
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        indicatorObj.transform.position = screenPos;

        // animate fade out and slight movement
        indicatorObj.GetComponent<CanvasGroup>().alpha = 1f;
        indicatorObj.transform.localScale = Vector3.one;
    }
}
