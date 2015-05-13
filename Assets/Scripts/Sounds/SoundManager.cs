using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour 
{
    public AudioClip titleSound;
    public float titleVolume = 1.0f;

    public AudioClip titleAcceptSound;
    public float titleAcceptVolume = 1.0f;
    
    public AudioClip menuMusic;
    public List<AudioClip> gameMusic;
    public float musicVolume = 1.0f;
    int lastTrackPlayed = 0;
    bool firstTrack = true;
    
    public AudioClip introSound;
    public float introVolume = 1.0f;

    public AudioClip winSound;
    public float winVolume = 1.0f;

    public List<AudioClip> introSpawnSounds;
    public float introSpawnVolume = 1.0f;

    public List<AudioClip> menuJumpSounds;
    public float menuJumpVolume = 1.0f;


    public AudioClip gameOverSound;
    public float gameOverVolume = 1.0f;

    public AudioClip pressStartBuzz;
    public float pressStartBuzzVolume = 1.0f;

    public bool canPlayMusic = true;

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
            GetComponent<AudioSource>().clip = introSound;
            GetComponent<AudioSource>().volume = introVolume;
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
        }
        else if (Application.loadedLevelName == "TitleScreen")
        {
            GetComponent<AudioSource>().clip = SoundManager.instance.titleSound;
            GetComponent<AudioSource>().volume = SoundManager.instance.titleVolume;
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();
        }
        else if (Application.loadedLevelName == "EndScene")
        {
            playTemporarySound(gameOverSound, gameOverVolume, gameObject.transform.position);
        }
        else
        {
            playLevelTrack();
        }
	}
    public void playMusic(AudioClip music,float musicVolume)
    {
        if (canPlayMusic)
        {
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().clip = music;
            GetComponent<AudioSource>().volume = musicVolume;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().volume = 0;
        }
    }
    public float playMenuJump(Vector3 position)
    {
        int rnd = Random.Range(0, menuJumpSounds.Count);
        playTemporarySound(menuJumpSounds[rnd], menuJumpVolume, position);
        return menuJumpSounds[rnd].length;
    }
    public float playPressStartBuzz(Vector3 position)
    {
        playTemporarySound(pressStartBuzz, pressStartBuzzVolume, position);
        return pressStartBuzz.length;
    }
    void playLevelTrack()
    {
        GetComponent<AudioSource>().loop = true;
        if (Application.loadedLevelName == "TitleScreen")
        {
            GetComponent<AudioSource>().clip = gameMusic[0];
            GetComponent<AudioSource>().volume = musicVolume;
        }
        else if (Application.loadedLevelName == "JoinScene")
        {

        }
        GetComponent<AudioSource>().Play();
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
        if (!GetComponent<AudioSource>().isPlaying)
        {
            playLevelTrack();
        }
    }
}
