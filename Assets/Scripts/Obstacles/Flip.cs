using UnityEngine;
using System.Collections;

public class Flip : MonoBehaviour {
    void Awake() {
        initialYPos = transform.position.y;
    }

	// Use this for initialization
	void Start () {
	    if(gameObject.transform.position.y > initialYPos)
            gameObject.transform.Rotate(new Vector3(180, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private float initialYPos;
}
