using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour {

	public int countDownTime = 10;

	public Text joinText;
	public Text countDownText;
	public Text playersText;

	private Dictionary<KeyCode, float> keysPressed;

	private static KeyCode[] keyCodes;

	public float timeToHold = 2;

	//private List<KeyCode> playerKeys;

	// Use this for initialization
	void Start () {
		keysPressed = new Dictionary<KeyCode, float>();

		keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));

		StartCoroutine(flashText());
		StartCoroutine(countDown());

		//playerKeys = new List<KeyCode>();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < keyCodes.Length; i++) {
			if (Input.GetKey(keyCodes[i])) {
				float temp = 0;
				if (keysPressed.TryGetValue(keyCodes[i], out temp)) {
					keysPressed[keyCodes[i]] += Time.deltaTime;
					if (keysPressed[keyCodes[i]] >= timeToHold) {
						CurrentPlayerKeys.Instance.playerKeys.Add(keyCodes[i]);
						//Text stuff here
						playersText.text = playersText.text + " " + keyCodes[i].ToString() + " ";

						keysPressed[keyCodes[i]] = -100;
					}
				}
				else {
					keysPressed.Add(keyCodes[i], 0);
				}
			}
		}
	}

	private IEnumerator flashText() {
		while(true) {
			joinText.text = "HOLD A BUTTON TO JOIN!";
			yield return new WaitForSeconds(1.5f);
			joinText.text = "";
			yield return new WaitForSeconds(0.5f);
		}
	}

	private IEnumerator countDown() {
		for (int i = countDownTime; i >= 0; i--) {
			countDownText.text = i.ToString();
			yield return new WaitForSeconds(1);
		}

		Application.LoadLevel("TestScene");
	}
}
