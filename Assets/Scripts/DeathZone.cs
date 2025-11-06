using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col) {
        EnemyBehavior rat = col.GetComponent<EnemyBehavior>();
        if(rat == null) {
            return;
        }

        rat.OnDespawn(); // notify rat to do logic

        var gm = FindFirstObjectByType<GameManager>();
        if(gm.currentMode == GameManager.GameMode.Endless) {
            gm.currentHealth = Mathf.Max(0, gm.currentHealth - 1);
            gm.electoralUI.UpdateHealthBar(gm.currentHealth, gm.maxHealth);
            if(gm.currentHealth <= 0) {
                // gm.GameOver();
            }
        }

        Destroy(col.gameObject);
    }
}
