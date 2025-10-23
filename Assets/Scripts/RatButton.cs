using UnityEngine;
using UnityEngine.EventSystems;

public class RatButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() {
        anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        anim.SetBool("isHovering", true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        anim.SetBool("isHovering", false);
    }
}
