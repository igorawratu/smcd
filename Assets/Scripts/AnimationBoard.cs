using UnityEngine;
using System.Collections;

public class AnimationBoard : MonoBehaviour
{
    private Animator animator;

    private int grounded = Animator.StringToHash("grounded");
    private int falling = Animator.StringToHash("falling");
    private int stumble = Animator.StringToHash("stumble");
    private int jump = Animator.StringToHash("jump");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Jump()
    {
        //Debug.Log("jump");
        animator.SetTrigger(jump);
        animator.SetBool(grounded, false);
    }
    public void Fall()
    {
        animator.SetTrigger(falling);
    }
    public void Land()
    {
        animator.SetBool(grounded, true);
    }
    public void Hit()
    {
        animator.SetTrigger(stumble);
    }
    

}
