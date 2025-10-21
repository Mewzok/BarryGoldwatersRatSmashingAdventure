using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    public GameObject ratPrefab;
    public Transform[] spawnPoints;
    public GameManager gameManager;

    // rat speed variables
    public float globalSpeedMultiplier = 1f;
    public float speedIncreaseRate = 0.05f;
    public float maxGlobalSpeedMultiplier = 5f;

    // rat spawn variables
    private float spawnTimer;
    public float spawnInterval = 3f;
    public float minSpawnInterval = 0.5f;
    public float spawnIncreaseRate = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // handle global speed
        globalSpeedMultiplier = Mathf.Lerp(1f, maxGlobalSpeedMultiplier, Mathf.Clamp01(Time.timeSinceLevelLoad / 120f));

        // handle spawn timing
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= spawnInterval) {
            SpawnRat();
            spawnTimer = 0f;
        }

        spawnInterval = Mathf.Max(spawnInterval - spawnIncreaseRate * Time.deltaTime, minSpawnInterval);
    }

    void SpawnRat() {
        int randNum = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randNum];

        GameObject ratObj = Instantiate(ratPrefab, spawnPoint.position, Quaternion.identity);
        EnemyBehavior rat = ratObj.GetComponent<EnemyBehavior>();
        rat.lane = randNum;

        rat.baseSpeed =  Random.Range(.5f, 2);

        gameManager.RegisterRat(rat);
    }
}
