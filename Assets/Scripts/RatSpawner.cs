using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    public GameObject ratPrefab;
    public Transform[] spawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(SpawnRat), 1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRat() {
        int randNum = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randNum];
        Instantiate(ratPrefab, spawnPoint.position, Quaternion.identity);
    }
}
