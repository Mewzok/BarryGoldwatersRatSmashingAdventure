// handles game logic, mechanics

using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum GameMode {
        Election,
        Endless
    }

    public enum Difficulty {
        Easy,
        Medium,
        Hard
    }

    public GameMode currentMode;
    public Difficulty currentDifficulty;
    public RatSpawner ratSpawner;
    public Transform player;

    public int totalPoints = 0;
    private int pointsToWin = 270;
    private int perfectScore = 538;
    private bool playerHasWon = false;
    private bool perfectGame = false;

    // thresholds for determining points when rat smashed
    private float perfect = 0.1f;
    private float good = 0.3f;
    private float okay = 0.6f;

    // point UI variables
    public RectTransform blueBar;
    public RectTransform redBar;

    private float targetLineY = -4.10f;

    public List<EnemyBehavior> activeRats = new List<EnemyBehavior>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!string.IsNullOrEmpty(GameSettings.difficulty) && 
            System.Enum.TryParse(GameSettings.difficulty, true, out Difficulty parsedDiff)) {
                currentDifficulty = parsedDiff;
        } else {
                currentDifficulty = Difficulty.Medium;
        }

        if(!string.IsNullOrEmpty(GameSettings.mode) && 
            System.Enum.TryParse(GameSettings.mode, true, out GameMode parsedMode)) {
                currentMode = parsedMode;
        } else {
            currentMode = GameMode.Endless;
        }

        ratSpawner.SetupDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        // constantly update rat distance to target line
        for(int i = activeRats.Count - 1; i >= 0; i--) {
            var rat = activeRats[i];
            if(rat == null) {
                continue;
            }
            
            float dist = Mathf.Abs(rat.transform.position.y - targetLineY);
            rat.UpdateAuraFeedback(dist, perfect, good, okay, targetLineY);
        }
    }

    public void CheckHit(int lane) {
        // get rats in lane
        var ratsInLane = activeRats.FindAll(r => r.lane == lane);
        if(ratsInLane.Count == 0) {
            HandleMiss();
            return;
        }

        bool hitSomething = false;

        // check each rat in lane and check distance from rat to target line at absolute value
        foreach(var rat in ratsInLane) {
            float dist = Mathf.Abs(rat.transform.position.y - targetLineY);

            // if there's at least one rat in lane, it's in at least "okay" range and it's within reach it's smashable
            if(dist <= okay && rat.transform.position.y < -3.70) {
                hitSomething = true;

                int points = 0;
                string hitText = "";

                if(rat.isPerfectActive || dist <= perfect) {
                    points += 3;
                    hitText = "Perfect";
                } else if(dist <= good) {
                    points += 2;
                    hitText = "Good";
                } else {
                    points += 1;
                    hitText = "OK";
                }

                Debug.Log($"Rat hit in lane {lane}, Distance {dist:F3}, Points: {points}, TotalPoints: {totalPoints}");

                // animate smashed rat
                Animator animator = rat.GetComponent<Animator>();
                animator.SetTrigger("SmashRat");

                // stop rat movement
                rat.isSmashed = true;

                // play particle effects
                FeedbackManager.Instance.PlaySmashEffects(rat.transform.position);

                // display hit indicator timing text
                FeedbackManager.Instance.ShowHitIndicator(hitText, rat.transform.position);

                UnregisterRat(rat);
                Destroy(rat.gameObject, 5f);

                totalPoints += points;
            }
        }

        // handle miss if no rats were close enough
        if(!hitSomething) {
            HandleMiss();
        }

        // add points and check if player has passed win threshold
        totalPoints = Mathf.Min(totalPoints, perfectScore);
        if(totalPoints >= pointsToWin) {
            playerHasWon = true;
        } else {
            playerHasWon = false;
        }
        if(totalPoints == perfectScore) {
            perfectGame = true;
        }

        // update points UI
        UpdateScoreVisual();
    }

    public void RegisterRat(EnemyBehavior rat) {
        activeRats.Add(rat);
    }

    public void UnregisterRat(EnemyBehavior rat) {
        activeRats.Remove(rat);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-10, targetLineY, 0), new Vector3(10, targetLineY, 0));
    }

    void HandleMiss() {
        // missed entirely
        totalPoints = Mathf.Max(0, totalPoints - 1);

        if(player != null) {
            FeedbackManager.Instance.ShowHitIndicator("Miss", player.transform.position);
        }

        // update point UI
        UpdateScoreVisual();

        return;
    }

    void UpdateScoreVisual() {
        float fillRatio = totalPoints / perfectScore;
        fillRatio = Mathf.Clamp01(fillRatio);

        float totalHeight = ((RectTransform)blueBar.parent).rect.height;
        redBar.sizeDelta = new Vector2(redBar.sizeDelta.x, totalHeight * fillRatio);
    }
}
