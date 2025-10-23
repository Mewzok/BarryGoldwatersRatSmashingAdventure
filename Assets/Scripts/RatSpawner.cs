using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    public GameObject ratPrefab;
    public GameObject auraPrefab;
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

        // attach rat aura
        var aura = Instantiate(auraPrefab, ratObj.transform);
        aura.transform.localPosition = Vector3.zero;

        gameManager.RegisterRat(rat);
    }

    public void SetupDifficulty() {
        Debug.Log($"Difficulty setup on difficulty {gameManager.currentDifficulty}");

       // determine values based on difficulty
        switch(gameManager.currentDifficulty) {
            case GameManager.Difficulty.Easy:
                globalSpeedMultiplier = 0.7f;
                speedIncreaseRate = 0.02f;
                spawnInterval = 4f;
                spawnIncreaseRate = 0.02f;
                break;
            case GameManager.Difficulty.Medium:
                globalSpeedMultiplier = 0.9f;
                speedIncreaseRate = 0.04f;
                spawnInterval = 3f;
                spawnIncreaseRate = 0.04f;
                break;
            case GameManager.Difficulty.Hard:
                globalSpeedMultiplier = 1.1f;
                speedIncreaseRate = 0.06f;
                spawnInterval = 2f;
                spawnIncreaseRate = 0.06f;
                break;
        }
    }
}
