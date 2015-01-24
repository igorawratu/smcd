using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour {

	public int countDownTime = 10;

	public Text joinText;
	public Text countDownText;

	private Dictionary<KeyCode, float> keysPressed;

	private static KeyCode[] keyCodes;

	public float timeToHold = 2;

	public GameObject floorPrefab;
	public GameObject groundPrefab;
	public GameObject playerPrefab;

	public GameObject[] floorArr = new GameObject[40];
	public GameObject[] groundArr = new GameObject[40];
	public Sprite[] floorSprites = new Sprite[6];
	public Vector3 floorOffset = new Vector3(-20, 0, 0);

	private Vector3 floorSize;
	private Vector3 groundSize;

	private float initialSpawn = 0;

	public GameObject headTextPrefab;

	// Use this for initialization
	void Start () {
		keysPressed = new Dictionary<KeyCode, float>();

		keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));

		StartCoroutine(flashText());
		StartCoroutine(countDown());

		//Create player colors
		CurrentPlayerKeys.Instance.playerColors.Add(Color.blue);
		CurrentPlayerKeys.Instance.playerColors.Add(Color.red);
		CurrentPlayerKeys.Instance.playerColors.Add(Color.green);
		CurrentPlayerKeys.Instance.playerColors.Add(Color.yellow);
		CurrentPlayerKeys.Instance.playerColors.Add(Color.cyan);
		CurrentPlayerKeys.Instance.playerColors.Add(Color.magenta);
		CurrentPlayerKeys.Instance.playerColors.Add(Color.grey);

		floorSize = floorPrefab.transform.renderer.bounds.max - floorPrefab.transform.renderer.bounds.min;
		groundSize = groundPrefab.transform.renderer.bounds.max - groundPrefab.transform.renderer.bounds.min;

		for (int i = 0; i < floorArr.Length; i++)
		{
			floorArr[i] = (GameObject)Instantiate(floorPrefab);
			setFloorSprite(floorArr[i]);
			
			Vector3 pos = floorOffset + new Vector3(floorSize.x * i, 0.0f, 0.0f);
			floorArr[i].transform.position = pos;
		}

		for (int i = 0; i < floorArr.Length; i++)
		{
			groundArr[i] = (GameObject)Instantiate(groundPrefab);
			Vector3 pos = floorOffset + new Vector3(groundSize.x * i, -floorSize.y, 0.0f);
			groundArr[i].transform.position = pos;
		}
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

						//Spawn stuff here
						GameObject canvas = GameObject.Find("Canvas");
						float xSpawn = Random.Range(25, 75);
						initialSpawn += xSpawn;

						//GameObject player = (GameObject)Instantiate(playerPrefab);
						//player.GetComponent<PlayerMovement>().setJumpKey(KeyCode.Asterisk);
						//player.GetComponent<PlayerMovement>().playerColour = CurrentPlayerKeys.Instance.playerColors[i];
						//player.transform.position = new Vector3(xSpawn, player.transform.position.y, 0);
						//Add text
						GameObject aboveHead = (GameObject)Instantiate(headTextPrefab);

						aboveHead.transform.SetParent(canvas.transform);
						Text aboveHeadText = aboveHead.GetComponent<Text>();
						aboveHeadText.text = keyCodes[i].ToString();
						aboveHeadText.rectTransform.position = new Vector3(initialSpawn, 150, 0);

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

	void setFloorSprite(GameObject gObj)
	{
		int randomTile = Random.Range(0, floorSprites.Length);
		SpriteRenderer sr = gObj.GetComponent<SpriteRenderer>();
		sr.sprite = floorSprites[randomTile];
	}
}
