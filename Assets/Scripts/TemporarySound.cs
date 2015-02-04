using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TemporarySound : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
    {
	    
	}
    public void play(AudioClip clip, float volume)
    {
        audio.clip = clip;
        audio.volume = volume;
        audio.loop = false;
        audio.Play();
        //audio.Play(clip, volume);
        Invoke("destroy",clip.length);
    }
    public void stop()
    {
        //if()
        audio.Stop();
        //Destroy(gameObject);
    }
    void destroy()
    {
        Destroy(gameObject);
    }
    //// Update is called once per frame
    //void Update () 
    //{
	
    //}
}
