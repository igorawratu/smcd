using UnityEngine;
using System.Collections;

public class Flip : MonoBehaviour {
    void Awake() {
        initialYPos = transform.position.y;
    }

	// Use this for initialization
	void Start () {
        if(gameObject.transform.position.y > initialYPos) {
            gameObject.transform.Rotate(new Vector3(180, 0, 0));

            foreach(Transform child in transform){
                randomize rs = child.gameObject.GetComponent<randomize>();
                if(child.gameObject.name != "plank_base" && child.gameObject.name != "plank_end") {
                    rs.flip();
                }
            }

        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    private float initialYPos;
}
