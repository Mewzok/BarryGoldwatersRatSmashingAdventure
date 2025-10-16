// manages lanes and other game logic

using UnityEngine;
using System.Collections.Generic;

public class LaneManager : MonoBehaviour
{
    public int points = 0;

    public KeyCode laneKey;
    private float targetLineY = -4.28f;

    // thresholds for determining points when rat smashed
    private float perfect = 0.1f;
    private float good = 0.25f;
    private float okay = 0.5f;

    public List<EnemyBehavior> activeRats = new List<EnemyBehavior>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckHit(int lane) {
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

        // if there's at least one rat in lane, it's in at least "okay" range and it's within reach it's smashable
        if(closestRat != null && closestDist <= okay && closestRat.transform.position.y < -4.18) {
            if(closestDist <= perfect) {
                points += 3;
            } else if(closestDist <= good) {
                points += 2;
            } else {
                points += 1;
            }

            UnregisterRat(closestRat);
            Destroy(closestRat.gameObject);
        }
    }

    public void RegisterRat(EnemyBehavior rat) {
        activeRats.Add(rat);
    }

    public void UnregisterRat(EnemyBehavior rat) {
        activeRats.Remove(rat);
    }
}
