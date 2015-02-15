using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour 
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

    public Dictionary<string, AudioClip> powerupSounds;


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
    public void playMusic(AudioClip music,float musicVolume)
    {
        audio.loop = true;
        audio.clip = music;
        audio.volume = musicVolume;
        audio.Play();
    }
    void playLevelTrack()
    {
        //int rnd = getNewTrack();
        //audio.clip = gameMusic[rnd];
        audio.loop = true;
        //bool sameTrack = false;
        if (Application.loadedLevelName == "TitleScreen")
        {
            audio.clip = gameMusic[0];
        }
        else if (Application.loadedLevelName == "JoinScene")
        {

        }
        else
        {
            //int newClip = 0;
            //switch (LevelTypeManager.currentLevel)
            //{
            //    case LevelTypeManager.Level.standard:
            //        newClip = 0;
            //        break;
            //    case LevelTypeManager.Level.lowGravity:
            //        newClip = 1;
            //        break;
            //    case LevelTypeManager.Level.flappyBird:
            //        newClip = 2;
            //        break;
            //    case LevelTypeManager.Level.gravityFlip:
            //        newClip = 3;
            //        break;
            //}

            //if (audio.clip != gameMusic[newClip])
            //{
            //    audio.clip = gameMusic[newClip];
            //}
            //else
            //{
            //    sameTrack = true;
            //}
        }
        audio.Play();
        //if (!sameTrack)
        //{
        //    //audio.
        //    audio.Play();
        //}
        //Invoke("playNextTrack", gameMusic[rnd].length);
    }
    
    public void playTemporarySound(AudioClip clip, float volume, Vector3 position)
    {
        GameObject tempS = (GameObject)Instantiate(tempSound);
        tempS.transform.position = position;
        TemporarySound script = tempS.GetComponent<TemporarySound>();
        script.play(clip, volume);
    }
    public void sceneChanged()
    {
        Start();
    }
     //Update is called once per frame
    void Update () 
    {
        if (!audio.isPlaying)
        {
            playLevelTrack();
        }
    }
}
