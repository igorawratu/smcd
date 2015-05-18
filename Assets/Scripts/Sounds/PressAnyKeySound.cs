using UnityEngine;
using System.Collections;

public class PressAnyKeySound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void playSound()
    {
        SoundManager.instance.playPressStartBuzz(gameObject.transform.position);
    }
    //// Update is called once per frame
    //void Update () {
	
    //}
}
