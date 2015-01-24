using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;
    public float offset = 0.0f;
    public float startPoint = 0.0f;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < CurrentPlayerKeys.Instance.playerKeys.Count; i++) {
			KeyCode kc = CurrentPlayerKeys.Instance.playerKeys[i];
			GameObject player = (GameObject) Instantiate(playerPrefab);
			player.name = kc.ToString();
			player.GetComponent<PlayerMovement>().setJumpKey(kc);

			//Random player's spawn position near the middle
<<<<<<< HEAD
			offset = Random.Range(0, 2);
            player.transform.position = new Vector3(startPoint + offset, 0.59f, 0);
=======
			float offset = Random.Range(0, 2);
			player.transform.position = new Vector3(-4 - offset, 1.65f, 0);
>>>>>>> Added players spawning at the bottom

			//Assign player color
			player.GetComponent<PlayerMovement>().playerColour = CurrentPlayerKeys.Instance.playerColors[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
