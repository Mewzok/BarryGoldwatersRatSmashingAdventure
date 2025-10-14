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

        rat.OnDespawn(); // notify rat to do logic
        Destroy(col.gameObject);
    }
}
