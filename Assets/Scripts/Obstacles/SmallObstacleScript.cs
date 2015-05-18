﻿using UnityEngine;
using System.Collections;

public class SmallObstacleScript : MonoBehaviour {
    public Sprite[] sprites = new Sprite[4];
	// Use this for initialization
	void Start () {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        //System.Random rng = new System.Random();
        sr.sprite = sprites[mRng.Next(0, sprites.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private static System.Random mRng = new System.Random();
}
