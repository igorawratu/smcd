using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
		foreach (KeyCode kc in CurrentPlayerKeys.Instance.playerKeys) {
			GameObject player = (GameObject) Instantiate(playerPrefab);
			player.name = kc.ToString();
			player.GetComponent<PlayerMovement>().setJumpKey(kc);

			//Random player's spawn position near the middle
			float offset = Random.Range(0, 2);
			player.transform.position = new Vector3(-4 - offset, 0.59f, 0);

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
