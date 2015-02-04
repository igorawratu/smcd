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
    public List<AudioClip> gameMusic;
    int lastTrackPlayed = 0;
    bool firstTrack = true;


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
        if (Application.loadedLevelName == "JoinScene")
        {
            audio.clip = introSound;
            audio.volume = introSpawnVolume;
            audio.loop = false;
            audio.Play();
        }
        else if (Application.loadedLevelName == "RunScene")
        {
            playNextTrack();
        }
        else if (Application.loadedLevelName == "TitleScreen")
        {
            GameObject tempSound = (GameObject)Instantiate(SoundManager.soundManager.tempSound);
            TemporarySound ts = tempSound.GetComponent<TemporarySound>();
            ts.play(SoundManager.soundManager.titleSound,
                    SoundManager.soundManager.titleVolume);

            Invoke("playNextTrack", SoundManager.soundManager.titleSound.length);
        }
        else
        {
            playNextTrack();
        }
	}

    void playNextTrack()
    {
        int rnd = getNewTrack();
        audio.clip = gameMusic[rnd];
        audio.Play(); 
        Invoke("playNextTrack", gameMusic[rnd].length);
    }
    int getNewTrack()
    {
        if (firstTrack)
        {
            int rnd = Random.Range(0, gameMusic.Count);
            lastTrackPlayed = rnd;
            return lastTrackPlayed;
        }

        if (gameMusic.Count <= 1)
        {
            lastTrackPlayed = 1;
        }
        else
        {
            int rnd = Random.Range(0, gameMusic.Count);
            while (rnd == lastTrackPlayed)
            {
                rnd = Random.Range(0, gameMusic.Count);
            }
            lastTrackPlayed = rnd;
        }

        firstTrack = false;
        return lastTrackPlayed;
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
