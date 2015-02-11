using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PowerupSounds : MonoBehaviour
{
    //activate sounds
    public AudioClip glide;
    public float glideVolume = 1.0f;

    public AudioClip smash;
    public float smashVolume = 1.0f;

    public AudioClip boostJump;
    public float boostJumpVolume = 1.0f;

    public AudioClip doubleJump;
    public float doubleJumpVolume = 1.0f;

    //pickup sounds
    public AudioClip glidePickup;
    public float glidePickupVolume = 1.0f;

    public AudioClip smashPickup;
    public float smashPickupVolume = 1.0f;

    public AudioClip boostJumpPickup;
    public float boostJumpPickupVolume = 1.0f;

    public AudioClip doubleJumpPickup;
    public float doubleJumpPickupVolume = 1.0f;


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

    public void playGlidePickup()
    {
        SoundManager.instance.playTemporarySound(glidePickup, glidePickupVolume);
    }
    public void playSmashPickup()
    {
        SoundManager.instance.playTemporarySound(smashPickup, smashPickupVolume);
    }
    public void playDoubleJumpPickup()
    {
        SoundManager.instance.playTemporarySound(boostJumpPickup, boostJumpPickupVolume);
    }
    public void playBoostJumpPickup()
    {
        SoundManager.instance.playTemporarySound(doubleJumpPickup, doubleJumpPickupVolume);
    }

}
