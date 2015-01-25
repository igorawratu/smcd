using UnityEngine;
using System.Collections;

public class BigObstacleScript : MonoBehaviour {
    public Sprite[] sprites = new Sprite[5];
	// Use this for initialization
	void Start () {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        System.Random rng = new System.Random();
        sr.sprite = sprites[rng.Next(0, sprites.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
