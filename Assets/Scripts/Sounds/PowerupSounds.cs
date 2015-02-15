using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PowerupSounds : MonoBehaviour
{
    //activate sounds
    public List<AudioClip> glide = new List<AudioClip>(1);
    public float glideVolume = 1.0f;

    public List<AudioClip> smash = new List<AudioClip>(1);
    public float smashVolume = 1.0f;

    public List<AudioClip> boostJump = new List<AudioClip>(1);
    public float boostJumpVolume = 1.0f;

    public List<AudioClip> doubleJump = new List<AudioClip>(1);
    public float doubleJumpVolume = 1.0f;

    //pickup sounds
    public List<AudioClip> glidePickup = new List<AudioClip>(1);
    public float glidePickupVolume = 1.0f;

    public List<AudioClip> smashPickup = new List<AudioClip>(1);
    public float smashPickupVolume = 1.0f;

    public List<AudioClip> boostJumpPickup = new List<AudioClip>(1);
    public float boostJumpPickupVolume = 1.0f;

    public List<AudioClip> doubleJumpPickup = new List<AudioClip>(1);
    public float doubleJumpPickupVolume = 1.0f;
    
    public static PowerupSounds inst;
    // Use this for initialization
    void Start () 
    {
        inst = this;
    }

    //public void playGlide()
    //{
    //    int rnd = Random.Range(0, glide.Count);
    //    SoundManager.instance.playTemporarySound(glide[rnd], glideVolume);
    //}
    //public void playSmash()
    //{
    //    int rnd = Random.Range(0, smash.Count);
    //    SoundManager.instance.playTemporarySound(smash[rnd], smashVolume);
    //}
    //public void playDoubleJump()
    //{
    //    int rnd = Random.Range(0, boostJump.Count);
    //    SoundManager.instance.playTemporarySound(boostJump[rnd], boostJumpVolume);
    //}
    //public void playBoostJump()
    //{
    //    int rnd = Random.Range(0, doubleJump.Count);
    //    SoundManager.instance.playTemporarySound(doubleJump[rnd], doubleJumpVolume);
    //}

    //public void playGlidePickup()
    //{
    //    int rnd = Random.Range(0, glidePickup.Count);
    //    SoundManager.instance.playTemporarySound(glidePickup[rnd], glidePickupVolume);
    //}
    //public void playSmashPickup()
    //{
    //    int rnd = Random.Range(0, smashPickup.Count);
    //    SoundManager.instance.playTemporarySound(smashPickup[rnd], smashPickupVolume);
    //}
    //public void playDoubleJumpPickup()
    //{
    //    int rnd = Random.Range(0, boostJumpPickup.Count);
    //    SoundManager.instance.playTemporarySound(boostJumpPickup[rnd], boostJumpPickupVolume);
    //}
    //public void playBoostJumpPickup()
    //{
    //    int rnd = Random.Range(0, doubleJumpPickup.Count);
    //    SoundManager.instance.playTemporarySound(doubleJumpPickup[rnd], doubleJumpPickupVolume);
    //}

}
