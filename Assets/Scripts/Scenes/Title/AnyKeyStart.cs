using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AnyKeyStart : MonoBehaviour {

    bool hasPlayedSound = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.anyKey && !hasPlayedSound) 
        {
            GetComponent<AudioSource>().PlayOneShot(SoundManager.instance.titleAcceptSound, SoundManager.instance.titleAcceptVolume);
            hasPlayedSound = true;
            Invoke("loadMenu", SoundManager.instance.titleAcceptSound.length);
		}
	}
    void loadMenu()
    {
        Application.LoadLevel("JoinScene");
    }
}
