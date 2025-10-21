using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float baseSpeed;
    public int lane;
    public bool isSmashed = false;
    private float finalSpeed;

    // aura variables
    public AuraController auraController;
    public enum TimingState {
        None,
        Okay,
        Good,
        Perfect
    }
    public TimingState lastState = TimingState.None;
    private float pulseCooldown = 0.005f;
    private float lastPulseTime = -99f;
    public bool isPerfectActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finalSpeed = baseSpeed * FindFirstObjectByType<RatSpawner>().globalSpeedMultiplier;

        // assign AuraController automatically if not already set
        if(auraController == null) {
            auraController = FindFirstObjectByType<AuraController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // modify rat speed
        if(!isSmashed) {
            float globalMultiplier = FindFirstObjectByType<RatSpawner>().globalSpeedMultiplier;
            float finalSpeed = baseSpeed * globalMultiplier;
            transform.Translate(Vector3.down * Time.deltaTime * finalSpeed);
        }
    }

    public void OnDespawn() {
        FindFirstObjectByType<GameManager>().UnregisterRat(this);
    }

    public void UpdateAuraFeedback(float distanceToTarget, float perfect, float good, float okay, float targetLineY) {
        // determine state based on distance

        // prevent aura from firing if perfect is passed
        if(lastState == TimingState.Perfect) {
            return;
        }

        TimingState state = TimingState.None;

        if(distanceToTarget <= perfect + 0.05f) { 
            state = TimingState.Perfect;
        } else if(distanceToTarget <= good + 0.05f) {
            state = TimingState.Good;
        } else if(distanceToTarget <= okay + 0.05f) {
            state = TimingState.Okay;
        } else {
            state = TimingState.None;
        }

        if(state == TimingState.Perfect) {
            isPerfectActive = true;
        }

        if(state == lastState) {
            return;
        }

        lastState = state;

        // only pulse if moved into a new, real state
        if(state != TimingState.None) {
            if(Time.time - lastPulseTime < pulseCooldown) {
                return;
            }
            lastPulseTime = Time.time;

            // choose color and intensity per state
            Color pulseColor = Color.red;
            switch(state) {
                case TimingState.Perfect: pulseColor = Color.red;
                    break;
                case TimingState.Good: pulseColor = new Color(1f, 0.92f, 0.6f);
                    break;
                case TimingState.Okay: pulseColor = new Color(1f, 0.8f, 0.4f);
                    break;
            }

            // if auraController is assigned, pulse at rat's position
            if(auraController != null) {
                auraController.TriggerPulse(transform.position, pulseColor);
            }
        }
    }
}
