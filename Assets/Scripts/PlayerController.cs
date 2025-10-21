// handles only visual aspects of player movement, for mechanics/gameplayer logic see LaneManager

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    public GameManager gameManager;

    private float qPos = -2.43f;
    private float wPos = -0.91f;
    private float ePos = 0.9f;
    private float rPos = 2.64f;

    private float smashCooldown = 0.05f;
    private float smashTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // start smash cooldown timer
        smashTimer -= Time.deltaTime;

        // allow cancel any time as long as cooldown is up
        if(smashTimer <= 0f) {
            if(Input.GetKeyDown(KeyCode.Q)) {
                TrySmash(qPos, 0);
            }
            if(Input.GetKeyDown(KeyCode.W)) {
                TrySmash(wPos, 1);
            }
            if(Input.GetKeyDown(KeyCode.E)) {
                TrySmash(ePos, 2);
            }
            if(Input.GetKeyDown(KeyCode.R)) {
                TrySmash(rPos, 3);
            }
        }
    }

    void TrySmash(float xPos, int lane) {
        // move player
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

        // run smash animation
        animator.ResetTrigger("DoAction");
        animator.Play("BarryIdle", 0, 0f);
        animator.SetTrigger("DoAction");
        gameManager.CheckHit(lane);

        // begin cooldown
        smashTimer = smashCooldown;
    }
}
