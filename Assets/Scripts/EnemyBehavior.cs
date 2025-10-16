using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float baseSpeed;
    public int lane;
    public bool isSmashed = false;
    private float finalSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finalSpeed = baseSpeed * FindFirstObjectByType<RatSpawner>().globalSpeedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSmashed) {
            float globalMultiplier = FindFirstObjectByType<RatSpawner>().globalSpeedMultiplier;
            float finalSpeed = baseSpeed * globalMultiplier;
            transform.Translate(Vector3.down * Time.deltaTime * finalSpeed);
        }
    }

    public void OnDespawn() {
        FindFirstObjectByType<GameManager>().UnregisterRat(this);
    }
}
