using UnityEngine;
using System.Collections;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    public GameObject burstPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
