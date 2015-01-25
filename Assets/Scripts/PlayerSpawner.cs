using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;
    public float offset = 0.0f;
    public float startPoint = 0.0f;

	private List<int> chosenColours;

	// Use this for initialization
	void Start () {
		chosenColours = new List<int>();

		for (int i = 0; i < CurrentPlayerKeys.Instance.playerKeys.Count; i++) {
			KeyCode kc = CurrentPlayerKeys.Instance.playerKeys[i];
			GameObject player = (GameObject) Instantiate(playerPrefab);
			player.name = kc.ToString();
			player.GetComponent<PlayerMovement>().setJumpKey(kc);

			//Random player's spawn position near the middle
			offset = Random.Range(0, 2);
            player.transform.position = new Vector3(startPoint + offset, 0.59f, 0);

			player.GetComponent<PlayerMovement>().playerColour = CurrentPlayerKeys.Instance.playerColors[i];
		}

        if (CurrentPlayerKeys.Instance.playerKeys.Count==0)
        {
            KeyCode kc = KeyCode.UpArrow;
            GameObject player = (GameObject)Instantiate(playerPrefab);
            player.name = kc.ToString();
            player.GetComponent<PlayerMovement>().setJumpKey(kc);

            //Random player's spawn position near the middle
            offset = Random.Range(0, 2);
            player.transform.position = new Vector3(startPoint + offset, 0.59f, 0);

            //Assign player color
            player.GetComponent<PlayerMovement>().playerColour = Color.white;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			bool isNew = true;
			foreach (KeyCode kc in CurrentPlayerKeys.Instance.playerKeys) {
				if (Input.GetKey(kc)) {
					isNew = false;
					break;
				}
			}

			if (isNew) {

			}
		}
	}
}
