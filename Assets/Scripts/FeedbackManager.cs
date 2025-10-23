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

        // set color based on hit timing
        switch(text) {
            case "OK":
                tmp.color = Color.white;
                break;
            case "Good":
                tmp.color = Color.green;
                break;
            case "Perfect": 
                tmp.color = Color.yellow;
                break;
        }

        // convert world position to screen position
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        indicatorObj.transform.position = screenPos;

        // animate fade out and slight movement
        indicatorObj.GetComponent<CanvasGroup>().alpha = 1f;
        indicatorObj.transform.localScale = Vector3.one;

        indicatorObj.AddComponent<HitIndicatorAnimator>();
    }
}
