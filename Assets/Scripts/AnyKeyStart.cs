using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AnyKeyStart : MonoBehaviour {

    bool hasPlayedSound = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKey && !hasPlayedSound) 
        {
            audio.PlayOneShot(SoundManager.soundManager.titleAcceptSound, SoundManager.soundManager.titleAcceptVolume);
            hasPlayedSound = true;
            Invoke("loadMenu", SoundManager.soundManager.titleAcceptSound.length);
		}
	}
    void loadMenu()
    {
        Application.LoadLevel("MenuScene");
    }
}
