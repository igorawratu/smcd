using UnityEngine;
using System.Collections;

public class GenerateObstacles : MonoBehaviour {
	public GameObject prefab;
	private GameObject o;
	// Use this for initialization
	void Start () {
		o = (GameObject)Instantiate(prefab, transform.position, prefab.transform.rotation);
		o.transform.position = new Vector2(0, 10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		
	}


}
