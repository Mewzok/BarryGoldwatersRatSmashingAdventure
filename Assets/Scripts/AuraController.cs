// controls aura around rat to determine when to smash it
using UnityEngine;

public class AuraController : MonoBehaviour
{
    public ParticleSystem auraSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        auraSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerPulse(Vector3 pos, Color? color = null) {
        if(auraSystem == null) {
            return;
        }

        ParticleSystem ps = Instantiate(auraSystem, pos, Quaternion.identity);

        if(color.HasValue) {
            var main = ps.main;
            main.startColor = color.Value;

            if(color == Color.red) {
                main.startSize = 1.5f;
                main.startLifetime = 0.35f;
            }
        }

        ps.Play();
    }
}
