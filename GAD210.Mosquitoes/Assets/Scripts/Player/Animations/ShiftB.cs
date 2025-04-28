using UnityEngine;

public class ShiftB : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("ShiftTrig");
        }
    }
}