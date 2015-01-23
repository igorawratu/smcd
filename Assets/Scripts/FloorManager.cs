using UnityEngine;
using System.Collections;

public class FloorManager : MonoBehaviour {

	//Array of floor pieces
	public GameObject[] floorArr = new GameObject[3];

	private Vector3 floorSize;

	// Use this for initialization
	void Start () {
		floorSize = floorArr[0].transform.renderer.bounds.max - floorArr[0].transform.renderer.bounds.min;
	}
	
	// Update is called once per frame
	void Update () {
		if (floorArr[0].transform.position.x < Camera.main.transform.position.x) {
			if (!floorArr[0].transform.renderer.IsVisibleFrom(Camera.main)) {
				//get position of last floor piece
				Transform lastFloor = floorArr[floorArr.Length-1].transform;

				floorArr[0].transform.position = new Vector3(lastFloor.position.x + floorSize.x, floorArr[0].transform.position.y, 0);

				//Shuffle all the pieces down
				GameObject temp = floorArr[0];
				for (int i=0; i < floorArr.Length - 1; i++) {
					floorArr[i] = floorArr[i + 1];
				}
				floorArr[floorArr.Length - 1] = temp;
			}
		}
	}
}
