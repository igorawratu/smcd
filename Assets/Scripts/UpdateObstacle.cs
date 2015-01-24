using UnityEngine;
using System.Collections;

public class UpdateObstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		if (transform.position.y < 0) {
			transform.position = new Vector2(transform.position.x, 0);
			gameObject.rigidbody2D.velocity = new Vector2(gameObject.rigidbody2D.velocity.x, 0);
		}
		
	}
}
