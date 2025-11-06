using UnityEngine;
using TMPro;

public class ElectoralUI : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform rightBarPanel;
    public RectTransform redBar;
    public RectTransform grayLine;

    [Header("Scoring")]
    public int totalElectoralVotes = 538;
    public int winThreshold = 270;

    [Header("Animation")]
    public float fillLerpSpeed = 6f;

    [Header("Bounce Effect")]
    public float bounceAmount = 10f;
    public float bounceSpeed = 8f;
    private float bounceOffset = 0f;

    [Header("Left Panel Texts")]
    public TMP_Text pointsText;
    public TMP_Text remainingRatsText;
    public TMP_Text titleText;

    [Header("Mode Info")]
    public GameManager.GameMode currentMode;

    [Header("Health")]
    public TMP_Text healthText;

    [Header("Health Settings")]
    public int maxHealth;
    public int currentHealth;
    private float healthFill = 1f;

    // internal
    private float displayedFill = 0f;
    private float targetFill = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(currentMode == GameManager.GameMode.Election) {
            // initialize sizes
            UpdateTargetFillImmediate(0);
        } else {
            currentHealth = maxHealth;
            healthFill = 1f;
            displayedFill = 1f;
            targetFill = 1f;
            ApplyFill(healthFill);

        }

        if(healthText != null && currentMode == GameManager.GameMode.Endless) {
            healthText.text = $"{currentHealth}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        // smooth display
        displayedFill = Mathf.Lerp(displayedFill, targetFill, Mathf.Clamp01(Time.deltaTime * fillLerpSpeed));
        ApplyFill(displayedFill);
    }

    // call when score changes
    public void UpdateTargetFill(int currentPoints) {
        float newTarget = Mathf.Clamp01((float)currentPoints / (float)totalElectoralVotes);

        // trigger bounce if score increased
        if(newTarget > targetFill) {
            bounceOffset = bounceAmount;
        }

        targetFill = newTarget;
    }

    // immediate, no smoothing, used at Start
    public void UpdateTargetFillImmediate(int currentPoints) {
        targetFill = Mathf.Clamp01((float)currentPoints / (float)totalElectoralVotes);
        displayedFill = targetFill;
    }

    private void ApplyFill(float fill) {
        if(rightBarPanel == null || redBar == null) {
            return;
        }

        RectTransform parent = rightBarPanel;
        float parentHeight = parent.rect.height;

        // smooth bounce offset towards 0
        bounceOffset = Mathf.Lerp(bounceOffset, 0f, Time.deltaTime * bounceSpeed);

        // right bar UI for Election Mode
        if(currentMode == GameManager.GameMode.Election) {
            // set red bar height
            float newHeight = parentHeight * fill;
            newHeight = Mathf.Clamp(newHeight, 0f, parentHeight);
            redBar.sizeDelta = new Vector2(0f, newHeight);

            // place threshold line
            float thresholdRatio = (float)winThreshold / (float)totalElectoralVotes;
            float thresholdY = parentHeight * thresholdRatio;
            grayLine.gameObject.SetActive(true);
            grayLine.anchoredPosition = new Vector2(grayLine.anchoredPosition.x, thresholdY);

            // hide health text
            if(healthText != null) {
                healthText.gameObject.SetActive(false);
            }
        } else {
            // set red bar height
            float newHeight = parentHeight * healthFill;
            newHeight = Mathf.Clamp(newHeight, 0f, parentHeight);
            redBar.sizeDelta = new Vector2(0f, newHeight);

            // hide gray line
            grayLine.gameObject.SetActive(false);

            // show health text
            if(healthText != null) {
                healthText.gameObject.SetActive(true);
                healthText.text = $"{currentHealth}";
            }

            Color full = new Color(1f, 0f, 0f);
            Color empty = new Color(0.3f, 0f, 0f);
            redBar.GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(empty, full, healthFill);
        }
    }

    public void UpdateLeftPanel(int points, int totalPoints, int remainingRats) {
        if(currentMode == GameManager.GameMode.Election) {
            pointsText.text = $"Score:\n{points} / {totalPoints}";
            remainingRatsText.text = $"Rats Remaning:\n{remainingRats}";
        } else {
            pointsText.text = $"Score:\n{points}";
            remainingRatsText.text = $"Rats Remaining:\n∞";
        }
    }

    public void UpdateHealthBar(int current, int max) {
        if(currentMode != GameManager.GameMode.Endless) {
            return;
        }

        currentHealth = current;
        maxHealth = max;

        healthFill = Mathf.Clamp01((float)current / (float)max);

        // always update text immediately
        if(healthText != null) {
            healthText.text = $"{currentHealth}";
        }
    }

    public void SetupLeftPanelTitle() {
        // handle "title" text
        if(titleText == null) {
            return;
        } else {
            if(currentMode == GameManager.GameMode.Election) {
                titleText.text = $"{winThreshold} to Win";
            } else {
                titleText.text = "Endless";
            }
        }
    }
}
