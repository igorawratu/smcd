using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour 
{
    public static SoundManager soundManager;

    public List<AudioClip> hitSounds;
    public float hitSoundLevel = 1.0f;
    public List<AudioClip> footstepSounds;
    public float footstepSoundLevel = 1.0f;
    public List<AudioClip> deathSounds;
    public float deathSoundLevel = 1.0f;
    public List<AudioClip> jumpSounds;
    public float jumpSoundLevel = 1.0f;
    public List<AudioClip> pickupSounds;
    public float pickupSoundLevel = 1.0f;

    public AudioClip winSound;
    public float winSoundLevel = 1.0f;
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
        soundManager = this;
    }
	// Use this for initialization
	void Start () 
    {
        if (Application.loadedLevel == 1)
        {
            audio.clip = menuMusic; 
            audio.Play();
        }
        else if (Application.loadedLevel == 2)
        {
            audio.clip = gameMusic;
            audio.Play();
        }
        else
        {
            audio.clip = gameMusic;
            audio.Play();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}
}
