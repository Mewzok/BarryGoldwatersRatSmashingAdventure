// handles game logic, mechanics

using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int totalPoints = 0;

    // thresholds for determining points when rat smashed
    private float perfect = 0.1f;
    private float good = 0.25f;
    private float okay = 0.5f;

    private float targetLineY = -4.10f;

    public List<EnemyBehavior> activeRats = new List<EnemyBehavior>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        Debug.Log($"CheckHit called for lane {lane}. Active rats: {activeRats.Count}");

        // get rats in lane
        var ratsInLane = activeRats.FindAll(r => r.lane == lane);
        if(ratsInLane.Count == 0) {
            return;
        }

        // find rat closest to target line, presumably the one meant to be smashed
        // initialize values
        EnemyBehavior closestRat = null;
        float closestDist = float.MaxValue;

        // check each rat in lane and check distance from rat to target line at absolute value
        // assign each closest distance to closestDist and make that the closest rat
        foreach(var rat in ratsInLane) {
            float dist = Mathf.Abs(rat.transform.position.y - targetLineY);
            if(dist < closestDist) {
                closestDist = dist;
                closestRat = rat;
            }
        }

        Debug.Log($"ClosestDist={closestDist:F3}, ClosestRat={closestRat?.name ?? "none"}");

        // if there's at least one rat in lane, it's in at least "okay" range and it's within reach it's smashable
        if(closestRat != null && closestDist <= okay && closestRat.transform.position.y < -3.90) {
            int points = 0;

            if(closestRat.isPerfectActive || closestDist <= perfect) {
                points += 3;
            } else if(closestDist <= good) {
                points += 2;
            } else {
                points += 1;
            }

            Debug.Log($"Rat hit in lane {lane}, Distance {closestDist:F3}, Points: {points}");


            // animate smashed rat
            Animator animator = closestRat.GetComponent<Animator>();
            animator.SetTrigger("SmashRat");

            // stop rat movement
            closestRat.isSmashed = true;

            // play particle effects
            FeedbackManager.Instance.PlaySmashEffects(closestRat.transform.position);

            UnregisterRat(closestRat);
            Destroy(closestRat.gameObject, 5f);
        } else {
            Debug.Log($"Miss on lane {lane}. ClosestDist={closestDist:F3}");
        }
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
}
