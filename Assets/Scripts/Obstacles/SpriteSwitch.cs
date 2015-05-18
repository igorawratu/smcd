using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteSwitch : MonoBehaviour {
    public List<Sprite> spriteList;

    void Awake() {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        //System.Random rng = new System.Random();
        sr.sprite = spriteList[mRng.Next(0, spriteList.Count)];
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private static System.Random mRng = new System.Random();
}
