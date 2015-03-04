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
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().volume = volume;
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().Play();
        //audio.Play(clip, volume);
        Invoke("destroy",clip.length);
    }
    public void stop()
    {
        //if()
        GetComponent<AudioSource>().Stop();
        //Destroy(gameObject);
    }
    void destroy()
    {
        Destroy(gameObject);
    }
    //// Update is called once per frame
    //void Update()
    //{
    //    Time.
    //}
}
