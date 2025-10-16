using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    public GameObject ratPrefab;
    public Transform[] spawnPoints;
    public GameManager gameManager;

    public float globalSpeedMultiplier = 1f;
    public float speedIncreaseRate = 0.05f;
    public float maxGlobalSpeedMultiplier = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(SpawnRat), 1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        globalSpeedMultiplier = Mathf.Lerp(1f, maxGlobalSpeedMultiplier, Mathf.Clamp01(Time.timeSinceLevelLoad / 120f));
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
