using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour {

	public int countDownTime = 10;

	public Text joinText;
	public Text countDownText;

	private Dictionary<KeyCode, float> keysPressed;

	public static KeyCode[] keyCodes;

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
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.33f, 0, 0));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0, 0.33f, 0));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.2f, 0.2f, 0));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.3f, 0, 0.1f));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.33f, 0, 0.33f));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.6f, 0.6f, 1));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 1, 0.6f));
		CurrentPlayerKeys.Instance.possibleColors.Add(Color.yellow);
		CurrentPlayerKeys.Instance.possibleColors.Add(Color.white);
		CurrentPlayerKeys.Instance.possibleColors.Add(Color.red);
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 0.2f, 0.6f));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0, 0.33f, 0.33f));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 0.2f, 0.6f));
		CurrentPlayerKeys.Instance.possibleColors.Add(Color.blue);		
		CurrentPlayerKeys.Instance.possibleColors.Add(Color.cyan);
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 0.2f, 0.6f));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0, 1, 0.5f));
		CurrentPlayerKeys.Instance.possibleColors.Add(Color.grey);
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 0.5f, 0));
		CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.5f, 1, 0));


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
						float xSpawn = Random.Range(45, 100);
						initialSpawn += xSpawn;

						GameObject player = (GameObject)Instantiate(playerPrefab);

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
						Transform spacer = player.GetComponentInChildren<Transform>();
						spacer.GetComponentInChildren<SpriteRenderer>().color = CurrentPlayerKeys.Instance.possibleColors[colourIndex];
						Transform spawnEffect = spacer.GetComponentInChildren<Transform>();
						spawnEffect.GetComponentsInChildren<SpriteRenderer>()[1].color = CurrentPlayerKeys.Instance.possibleColors[colourIndex];
						//Convert position
						Vector3 xPt = new Vector3(initialSpawn, 0, 0);
						Vector3 newXPt = Camera.main.ScreenToWorldPoint(xPt);
						player.transform.position = new Vector3(newXPt.x, 1, 0);

                        ////Add text
                        //GameObject aboveHead = (GameObject)Instantiate(headTextPrefab);
                        //aboveHead.transform.SetParent(canvas.transform);
                        //Text aboveHeadText = aboveHead.GetComponent<Text>();
                        
                        //aboveHeadText.text = keyCodes[i].ToString();
                        //if(aboveHeadText.text.Contains("Arrow"))
                        //{
                        //    aboveHeadText.text = aboveHeadText.text.Substring(0, aboveHeadText.text.Length - 5);
                        //}
                        //aboveHeadText.fontStyle = FontStyle.Bold;
                        //aboveHeadText.rectTransform.position = new Vector3(initialSpawn, 225, 0);


                        GameObject spacerObject = player.transform.FindChild("spacer").gameObject;
                        GameObject keyText = spacerObject.transform.FindChild("KeyText").gameObject;

                        TextMesh tm = keyText.GetComponent<TextMesh>();
                        tm.text = keyCodes[i].ToString();
                        if (tm.text.Contains("Arrow"))
                        {
                            tm.text = tm.text.Substring(0, tm.text.Length - 5);
                        }


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

		if (CurrentPlayerKeys.Instance.playerKeys.Count > 0) {
			Application.LoadLevel("TestScene");
		}
		else {
			Application.LoadLevel("MenuScene");
		}

	}

	void setFloorSprite(GameObject gObj)
	{
		int randomTile = Random.Range(0, floorSprites.Length);
		SpriteRenderer sr = gObj.GetComponent<SpriteRenderer>();
		sr.sprite = floorSprites[randomTile];
	}
}
