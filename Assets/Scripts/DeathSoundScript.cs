using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DeathSoundScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        int rnd = Random.Range(0, SoundManager.instance.deathSounds.Count - 1);
        audio.PlayOneShot(SoundManager.instance.deathSounds[rnd], SoundManager.instance.deathVolume);
        Invoke("destroyObject", SoundManager.instance.deathSounds[rnd].length);
	}
	
    void destroyObject()
    {
        Destroy(gameObject);
    }

}
