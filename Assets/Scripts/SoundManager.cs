using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour 
{
    //public static SoundManager soundManager;

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


    private static SoundManager _instance = null;
    public static SoundManager instance
    {
        get 
        {
            return _instance; 
        }
    }

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
	// Use this for initialization
	void Start () 
    {
        if (Application.loadedLevelName == "JoinScene")
        {
            audio.clip = introSound;
            audio.volume = introSpawnVolume;
            audio.loop = true;
            audio.Play();
        }
        else if (Application.loadedLevelName == "RunScene")
        {
            playLevelTrack();
        }
        else if (Application.loadedLevelName == "TitleScreen")
        {
            //GameObject tempSound = (GameObject)Instantiate(SoundManager.instance.tempSound);
            //TemporarySound ts = tempSound.GetComponent<TemporarySound>();
            //ts.play(SoundManager.instance.titleSound,
            //        SoundManager.instance.titleVolume);
            audio.clip = SoundManager.instance.titleSound;
            audio.volume = SoundManager.instance.titleVolume;
            audio.loop = false;
            audio.Play();

            //Invoke("playLevelTrack", SoundManager.instance.titleSound.length);
        }
        else if (Application.loadedLevelName == "EndScene")
        {
        }
        else
        {
            playLevelTrack();
        }
	}

    void playLevelTrack()
    {
        //int rnd = getNewTrack();
        //audio.clip = gameMusic[rnd];
        audio.loop = true;
        bool sameTrack = false;
        if (Application.loadedLevelName == "TitleScreen")
        {
            audio.clip = gameMusic[0];
        }
        else
        {
            int newClip = 0;
            switch (LevelTypeManager.currentLevel)
            {
                case LevelTypeManager.Level.standard:
                    newClip = 0;
                    break;
                case LevelTypeManager.Level.evening:
                    newClip = 1;
                    break;
                case LevelTypeManager.Level.sunset:
                    newClip = 1;
                    break;
                case LevelTypeManager.Level.underground:
                    newClip = 0;
                    break;
            }

            if (audio.clip != gameMusic[newClip])
            {
                audio.clip = gameMusic[newClip];
            }
            else
            {
                sameTrack = true;
            }
        }
        if (!sameTrack)
        {
            audio.Play();
        }
        //Invoke("playNextTrack", gameMusic[rnd].length);
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
    public void sceneChanged()
    {
        //Debug.Log("scene changed");
        Start();
    }
	// Update is called once per frame
	void Update () 
    {
        if (!audio.isPlaying)
        {
            playLevelTrack();
        }
	}
}
