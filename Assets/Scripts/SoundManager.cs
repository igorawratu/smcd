using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour 
{
    public static SoundManager soundManager;

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

    public List<AudioClip> introSpawnSounds;
    public float introSpawnVolume = 1.0f;


    public AudioClip titleSound;
    public float titleVolume = 1.0f;

    public AudioClip titleAcceptSound;
    public float titleAcceptVolume = 1.0f;

    public AudioClip winSound;
    public float winVolume = 1.0f;

    public AudioClip menuMusic;
    public AudioClip gameMusic;


    public AudioClip introSound;

    public GameObject tempSound;

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
        soundManager = this;
    }
	// Use this for initialization
	void Start () 
    {
        if (Application.loadedLevelName == "MenuScene")
        {
            audio.clip = introSound;
            audio.volume = introSpawnVolume;
            audio.loop = false;
            audio.Play();
        }
        else if (Application.loadedLevelName == "MainRunSequence")
        {
            audio.clip = gameMusic;
            audio.Play();
        }
        else if (Application.loadedLevelName == "TitleScreen")
        {
            audio.clip = titleSound;
            audio.volume = titleVolume;
            audio.loop = false;
            audio.Play();

            audio.clip = gameMusic;
            audio.Play();
        }
        else
        {
            audio.clip = gameMusic;
            audio.Play();
        }
	}
    void playTemporarySound(AudioClip clip, float volume)
    {
        GameObject tempS = (GameObject)Instantiate(tempSound);
        TemporarySound script = tempS.GetComponent<TemporarySound>();
        script.play(clip, volume);
    }
	// Update is called once per frame
	void Update () 
    {
        
	}
}
