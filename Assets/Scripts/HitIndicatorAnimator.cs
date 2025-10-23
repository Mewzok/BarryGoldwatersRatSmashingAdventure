using UnityEngine;

public class HitIndicatorAnimator : MonoBehaviour
{
    public float duration = 0.6f;
    public float moveAmount = 50f;
    public float initialScale = 0.8f;
    public float targetScale = 1.2f;

    private CanvasGroup canvasGroup;
    private Vector3 startPos;
    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // move up slightly
        transform.position = startPos + Vector3.up * moveAmount * t;

        // fade out
        canvasGroup.alpha = 1f - t;

        // scale bounce effect
        float scaleFactor = Mathf.Lerp(initialScale, targetScale, Mathf.Sin(t * Mathf.PI));
        transform.localScale = Vector3.one * scaleFactor;

        // destroy after fade
        if(t >= 1f) {
            Destroy(gameObject);
        }
    }
}
