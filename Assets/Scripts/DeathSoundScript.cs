using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DeathSoundScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        int rnd = Random.Range(0, SoundManager.soundManager.deathSounds.Count - 1);
        audio.PlayOneShot(SoundManager.soundManager.deathSounds[rnd], SoundManager.soundManager.deathVolume);
        Invoke("destroyObject", SoundManager.soundManager.deathSounds[rnd].length);
	}
	
    void destroyObject()
    {
        Destroy(gameObject);
    }

}
