﻿using UnityEngine;
using System.Collections;

public class AnimationBoard : MonoBehaviour
{
    private Animator animator;
    
    //String Hashes
    private int grounded = Animator.StringToHash("grounded");
    private int falling = Animator.StringToHash("falling");
    private int stumble = Animator.StringToHash("stumble");
    private int jump = Animator.StringToHash("jump");
    private int flappyMode = Animator.StringToHash("FlappyMode");
    private int introMode = Animator.StringToHash("IntroMode");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //Set Player Modes
    public bool FlappyMode
    {
        set 
        { 
            animator.SetBool(flappyMode, value);
        }
    }

    public bool IntroMode
    {
        set
        {
            animator.SetBool(introMode, value);
        }
    }
    

    //Trigger Animations
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
