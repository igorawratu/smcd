using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class LevelSounds : MonoBehaviour 
{
    public List<AudioClip> hitSounds;
    public float hitVolume = 1.0f;

    public List<AudioClip> footstepSounds;
    public float footstepVolume = 1.0f;

    public List<AudioClip> deathSounds;
    public float deathVolume = 1.0f;

    public List<AudioClip> jumpSounds;
    public float jumpVolume = 1.0f;

    public List<AudioClip> pickupSounds;
    public float pickupVolume = 1.0f;

    public List<AudioClip> powerupSounds;
    public float powerupVolume = 1.0f;

    public List<AudioClip> spawnSounds;
    public float spawnVolume = 1.0f;
    
    public List<AudioClip> breakableObjectSounds;
    public float breakableObjectVolume = 1.0f;

    public AudioClip music;
    public float musicVolume = 1.0f;
    public static LevelSounds inst = null;
    
    public AudioClip winSound;
    public float winVolume = 1.0f;

	// Use this for initialization
	void Start () 
    {
        if (inst != null)
        {
            Destroy(gameObject);
        }
        else
        {
            inst = this;
            SoundManager.instance.playMusic(music, musicVolume);
        }
	}
	
    //// Update is called once per frame
    //void Update () 
    //{
	
    //}

    public void playDeath(Vector3 position)
    {
        int rnd = Random.Range(0, deathSounds.Count);
        SoundManager.instance.playTemporarySound(deathSounds[rnd], deathVolume, position);
    }
    public void playFootstep(Vector3 position)
    {
        int rnd = Random.Range(0, footstepSounds.Count);
        SoundManager.instance.playTemporarySound(footstepSounds[rnd], footstepVolume, position);
    }
    public void playHit(Vector3 position)
    {
        int rnd = Random.Range(0, hitSounds.Count);
        SoundManager.instance.playTemporarySound(hitSounds[rnd], hitVolume, position);
    }
    public void playJump(Vector3 position)
    {
        int rnd = Random.Range(0, jumpSounds.Count);
        SoundManager.instance.playTemporarySound(jumpSounds[rnd], jumpVolume, position);
    }

    public void playPowerup(Vector3 position)
    {
        int rnd = Random.Range(0, powerupSounds.Count);
        SoundManager.instance.playTemporarySound(powerupSounds[rnd], powerupVolume, position);
    }
    public void playPickup(Vector3 position)
    {
        int rnd = Random.Range(0, pickupSounds.Count);
        SoundManager.instance.playTemporarySound(pickupSounds[rnd], pickupVolume, position);
    }
    public void playSpawn(Vector3 position)
    {
        int rnd = Random.Range(0, spawnSounds.Count);
        SoundManager.instance.playTemporarySound(spawnSounds[rnd], spawnVolume, position);
    }
    public void playBreakableObject(Vector3 position)
    {
        int rnd = Random.Range(0, breakableObjectSounds.Count);
        SoundManager.instance.playTemporarySound(breakableObjectSounds[rnd], breakableObjectVolume, position);
    }
    public void playWinSound(Vector3 position)
    {
        SoundManager.instance.playTemporarySound(winSound, winVolume, position);
    }
}
