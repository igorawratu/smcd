using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PowerupSounds : MonoBehaviour 
{
    public AudioClip glide;
    public float glideVolume = 1.0f;

    public AudioClip smash;
    public float smashVolume = 1.0f;

    public AudioClip boostJump;
    public float boostJumpVolume = 1.0f;

    public AudioClip doubleJump;
    public float doubleJumpVolume = 1.0f;

    public static PowerupSounds inst;
    // Use this for initialization
    void Start () 
    {
        inst = this;
    }

    public void playGlide()
    {
        SoundManager.instance.playTemporarySound(glide, glideVolume);
    }
    public void playSmash()
    {
        SoundManager.instance.playTemporarySound(smash, smashVolume);
    }
    public void playDoubleJump()
    {
        SoundManager.instance.playTemporarySound(boostJump, boostJumpVolume);
    }
    public void playBoostJump()
    {
        SoundManager.instance.playTemporarySound(doubleJump, doubleJumpVolume);
    }


}
