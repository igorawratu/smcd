using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;
    public float offset = 0.0f;
    public float startPoint = 0.0f;

	private List<int> chosenColours;
	private Dictionary<KeyCode, float> enteringPlayers;

	public float timeToHold = 2;

	// Use this for initialization
	void Start () {
		chosenColours = new List<int>();
		enteringPlayers = new Dictionary<KeyCode, float>();

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
				for (int i = 0; i < MenuScript.keyCodes.Length; i++) {
					if (Input.GetKey(MenuScript.keyCodes[i])) {
						float temp = 0;
						if (enteringPlayers.TryGetValue(MenuScript.keyCodes[i], out temp)) {
							enteringPlayers[MenuScript.keyCodes[i]] += Time.deltaTime;
							if (enteringPlayers[MenuScript.keyCodes[i]] >= timeToHold) {
								CurrentPlayerKeys.Instance.playerKeys.Add(MenuScript.keyCodes[i]);
								
								//Spawn stuff here								
								GameObject player = (GameObject)Instantiate(playerPrefab);
								player.name = MenuScript.keyCodes[i].ToString();
								player.GetComponent<PlayerMovement>().setJumpKey(MenuScript.keyCodes[i]);
								//Random player's spawn position near the middle
								Vector3 screenMid = new Vector3(0.5f, 0, 0);
								startPoint = Camera.main.ViewportToWorldPoint(screenMid).x;
								offset = Random.Range(-1, 1);
								player.transform.position = new Vector3(startPoint + offset, 0.59f, 0);
								
								//Assign player color
								int colourIndex = Random.Range(0, CurrentPlayerKeys.Instance.possibleColors.Count);
								while (true) {
									if (CurrentPlayerKeys.Instance.playerColors.Contains(CurrentPlayerKeys.Instance.possibleColors[colourIndex])) {
										//go again
										colourIndex = Random.Range(0, CurrentPlayerKeys.Instance.possibleColors.Count);
									}
									else {
										CurrentPlayerKeys.Instance.playerColors.Add(CurrentPlayerKeys.Instance.possibleColors[colourIndex]);
										break;
									}
								}
								player.GetComponent<PlayerMovement>().playerColour = CurrentPlayerKeys.Instance.possibleColors[colourIndex];
								
								enteringPlayers[MenuScript.keyCodes[i]] = -100;
							}
						}
						else {
							enteringPlayers.Add(MenuScript.keyCodes[i], 0);
						}
					}
				}
			}
		}
	}
}
