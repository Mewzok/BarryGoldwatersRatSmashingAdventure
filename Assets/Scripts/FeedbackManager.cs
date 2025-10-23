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

    // particles
    public GameObject perfectSparklePrefab;

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

        // set font material for stylizing text
        var mat = tmp.fontMaterial;

        // set color based on hit timing
        switch(text) {
            case "Miss":
                tmp.colorGradientPreset = missGradient;
                tmp.font = defaultFont;
                mat.SetColor("_OutlineColor", new Color(0.3f, 0f, 0f));
                mat.SetFloat("_OutlineWidth", 0.1f);
                anim.initialScale = 0.5f;
                anim.targetScale = 0.4f;
                break;
            case "OK":
                tmp.colorGradientPreset = okGradient;
                tmp.font = defaultFont;
                anim.initialScale = 0.4f;
                anim.targetScale = 0.5f;
                break;
            case "Good":
                tmp.colorGradientPreset = goodGradient;
                tmp.font = defaultFont;
                anim.initialScale = 0.45f;
                anim.targetScale = 0.6f;
                break;
            case "Perfect": 
                tmp.colorGradientPreset = perfectGradient;
                tmp.font = perfectFont;
                mat.SetColor("_OutlineColor", new Color(1f, 0.9f, 0.2f));
                mat.SetFloat("_OutlineWidth", 0.15f);
                mat.SetColor("_GlowColor", new Color(1f, 0.8f, 0.1f));
                mat.SetFloat("_GlowPower", 0.25f);
                anim.initialScale = 0.45f;
                anim.targetScale = 0.7f;

                /*if(perfectSparklePrefab != null) {
                    var sparkle = Instantiate(perfectSparklePrefab, indicatorObj.transform);
                    sparkle.transform.localPosition = Vector3.zero; // center on text
                    sparkle.transform.localScale = Vector3.one * 5.5f; // slightly smaller than full size
                }*/
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
