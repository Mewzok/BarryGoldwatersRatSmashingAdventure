using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 10.0f;
    public int lane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }

    public void OnDespawn() {
        FindFirstObjectByType<GameManager>().UnregisterRat(this);
    }
}
