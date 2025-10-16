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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // check current animation state
        var currentState = animator.GetCurrentAnimatorStateInfo(0);

        // receive position
        Vector3 pos = transform.position;

        if(currentState.IsName("BarryIdle")) {
            if(Input.GetKeyDown(KeyCode.Q)) {
                pos = new Vector3(qPos, pos.y, pos.z);
                animator.SetTrigger("DoAction");
                gameManager.CheckHit(0);
            }

            if(Input.GetKeyDown(KeyCode.W)) {
                pos = new Vector3(wPos, pos.y, pos.z);
                animator.SetTrigger("DoAction");
                gameManager.CheckHit(1);
            }

            if(Input.GetKeyDown(KeyCode.E)) {
                pos = new Vector3(ePos, pos.y, pos.z);
                animator.SetTrigger("DoAction");
                gameManager.CheckHit(2);
            }

            if(Input.GetKeyDown(KeyCode.R)) {
                pos = new Vector3(rPos, pos.y, pos.z);
                animator.SetTrigger("DoAction");
                gameManager.CheckHit(3);
            }

            transform.position = pos;
        }
    }
}
