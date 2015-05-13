using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour {

	public int countDownTime = 10;

	public Text joinText;
	public Text countDownText;

    private Dictionary<KeyCode, float> keysPressed;
    private Dictionary<KeyCode, TemporarySound> keysSound;

	public static KeyCode[] keyCodes;

    public float timeToHold = 2;
    public float flashSpeed = 1;
    public float flashSpeedBlank = 1;

	public GameObject floorPrefab;
	public GameObject groundPrefab;
	public GameObject playerPrefab;

    public GameObject[] floorArr = new GameObject[20];
	public GameObject[] groundArr = new GameObject[20];
	public Sprite[] floorSprites = new Sprite[6];
	public Vector3 floorOffset = new Vector3(-20, 0, 0);

	private Vector3 floorSize;
	private Vector3 groundSize;

	private float initialSpawn = 0;

	public GameObject headTextPrefab;


    public Color samplePlayerColour = Color.green;

    bool canAddKey(KeyCode key)
    {
        string keyTokens = key.ToString();
        if (keyTokens == "Escape")
        {
            return false;
        }
		
		if (keyTokens == "Pause")
		{
			return false;
		}

        if(keyTokens.Length >= 14)
        {
            if (keyTokens.Substring(0, 14) == "JoystickButton")
            {
                return false;
            }
        }

        return true;
    }

	// Use this for initialization
	void Start () 
    {
		keysPressed = new Dictionary<KeyCode, float>();
        keysSound = new Dictionary<KeyCode, TemporarySound>();

		keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
        List<KeyCode> keyList = new List<KeyCode>();

        List<int> numsToDelete = new List<int>();

        for(int i = 0;i< keyCodes.Length ;i++)
        {
            if (canAddKey(keyCodes[i]))
            {
                keyList.Add(keyCodes[i]);
                //Debug.Log(keyCodes[i]);
            }
        }

        keyCodes = keyList.ToArray();

		StartCoroutine(flashText());
		StartCoroutine(countDown());

        ////Create player colors
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.33f, 0, 0));
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0, 0.33f, 0));
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.2f, 0.2f, 0));
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.3f, 0, 0.1f));
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.33f, 0, 0.33f));
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0.6f, 0.6f, 1));
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 1, 0.6f));
        //CurrentPlayerKeys.Instance.possibleColors.Add(Color.yellow);
        //CurrentPlayerKeys.Instance.possibleColors.Add(Color.white);
        //CurrentPlayerKeys.Instance.possibleColors.Add(Color.red);
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 0.2f, 0.6f));
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0, 0.33f, 0.33f));
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 0.2f, 0.6f));
        //CurrentPlayerKeys.Instance.possibleColors.Add(Color.blue);		
        //CurrentPlayerKeys.Instance.possibleColors.Add(Color.cyan);
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 0.2f, 0.6f));
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(0, 1, 0.5f));
        //CurrentPlayerKeys.Instance.possibleColors.Add(Color.grey);
        //CurrentPlayerKeys.Instance.possibleColors.Add(new Color(1, 0.5f, 0));

        HSV hsv = HSV.RGBtoHSV(samplePlayerColour);
        hsv.h = 0;
        float currentHvalue = hsv.h;
        
        for (int i = 0; i < 20;i++ )
        {
            currentHvalue += 1.61803398875f * 30;
            if (currentHvalue>360)
            {
                currentHvalue -= 360;
                hsv.s += 0.25f;
            }
            
            hsv.h = currentHvalue;
            CurrentPlayerKeys.Instance.possibleColors.Add(HSV.HSVtoRGB(hsv));
        }

        CurrentPlayerKeys.Instance.colourNumbers.Clear();
        for (int i = 0; i < CurrentPlayerKeys.Instance.possibleColors.Count; i++)
        {
            CurrentPlayerKeys.Instance.colourNumbers.Add(i);
        }

        floorSize = floorPrefab.transform.GetComponent<Renderer>().bounds.max - floorPrefab.transform.GetComponent<Renderer>().bounds.min;
		groundSize = groundPrefab.transform.GetComponent<Renderer>().bounds.max - groundPrefab.transform.GetComponent<Renderer>().bounds.min;

        //for (int i = 0; i < floorArr.Length; i++)
        //{
        //    floorArr[i] = (GameObject)Instantiate(floorPrefab);
        //    setFloorSprite(floorArr[i]);
			
        //    Vector3 pos = floorOffset + new Vector3(floorSize.x * i, 0.0f, 0.0f);
        //    floorArr[i].transform.position = pos;
        //}

        //for (int i = 0; i < floorArr.Length; i++)
        //{
        //    groundArr[i] = (GameObject)Instantiate(groundPrefab);
        //    Vector3 pos = floorOffset + new Vector3(groundSize.x * i, -floorSize.y, 0.0f);
        //    groundArr[i].transform.position = pos;
        //}
        CurrentPlayerKeys.Instance.players.Clear();
        CurrentPlayerKeys.Instance.playerScores.Clear();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("TitleScreen");
		}

        if (CurrentPlayerKeys.Instance.colourNumbers.Count > 0)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKey(keyCodes[i]) &&
                    !CurrentPlayerKeys.Instance.playerExists(MenuScript.keyCodes[i]))
                {
                    float temp = 0;
                    if (keysPressed.TryGetValue(keyCodes[i], out temp))
                    {
                        keysPressed[keyCodes[i]] += Time.deltaTime;
                        if (keysPressed[keyCodes[i]] >= timeToHold)
                        {
                            //Spawn stuff here
                            GameObject canvas = GameObject.Find("Canvas");
                            float xSpawn = Random.Range(45, 100);
                            initialSpawn += xSpawn;

                            GameObject player = (GameObject)Instantiate(playerPrefab);

                            JoinPlayerJump jpj = player.GetComponent<JoinPlayerJump>();
                            jpj.setKey(keyCodes[i]);

                            //Assign player color
                            int index = Random.Range(0, CurrentPlayerKeys.Instance.colourNumbers.Count);
                            int colourIndex = CurrentPlayerKeys.Instance.colourNumbers[index];
                            CurrentPlayerKeys.Instance.colourNumbers.RemoveAt(index);
                            CurrentPlayerKeys.Instance.players.Add(new KeyValuePair<KeyCode, Color>(keyCodes[i], CurrentPlayerKeys.Instance.possibleColors[colourIndex]));

                            



                                
                            Transform spacer = player.GetComponentInChildren<Transform>();
                            spacer.GetComponentInChildren<SpriteRenderer>().color = CurrentPlayerKeys.Instance.possibleColors[colourIndex];
                            Transform spawnEffect = spacer.GetComponentInChildren<Transform>();
                            spawnEffect.GetComponentsInChildren<SpriteRenderer>()[1].color = CurrentPlayerKeys.Instance.possibleColors[colourIndex];
                            //Convert position
                            Vector3 xPt = new Vector3(initialSpawn, 0, 0);
                            Vector3 newXPt = Camera.main.ScreenToWorldPoint(xPt);
                            player.transform.position = new Vector3(newXPt.x, 1, 0);
                                
                            //GameObject spacerObject = player.transform.FindChild("spacer").gameObject;
                            GameObject keyText = player.transform.FindChild("KeyText").gameObject;

                            TextMesh tm = keyText.GetComponent<TextMesh>();
                            tm.text = keyCodes[i].ToString();

                            tm.color = CurrentPlayerKeys.Instance.possibleColors[colourIndex];

                            if (tm.text.Contains("Arrow"))
                            {
                                tm.text = tm.text.Substring(0, tm.text.Length - 5);
                            }


                            keysPressed[keyCodes[i]] = -1000;

                        }
                    }
                    else
                    {
                        keysPressed.Add(keyCodes[i], 0);
                        GameObject tempSound = (GameObject)Instantiate(SoundManager.instance.tempSound);
                        TemporarySound ts = tempSound.GetComponent<TemporarySound>();
                        int rnd = Random.Range(0, SoundManager.instance.introSpawnSounds.Count);
                        ts.play(SoundManager.instance.introSpawnSounds[rnd],
                                SoundManager.instance.introSpawnVolume);
                        keysSound.Add(keyCodes[i], ts);
                    }
                }
                if (Input.GetKeyUp(keyCodes[i]) &&
                    !CurrentPlayerKeys.Instance.playerExists(MenuScript.keyCodes[i]))
                {
                    float temp = -1000;
                    if (keysPressed.TryGetValue(keyCodes[i], out temp))
                    {
                        if (temp < timeToHold && temp > -900)
                        {
                            keysPressed.Remove(keyCodes[i]);

                            TemporarySound ts = null;
                            bool success = keysSound.TryGetValue(keyCodes[i], out ts);
                            if (success)
                            {
                                ts.stop();
                                keysSound.Remove(keyCodes[i]);
                            }
                        }
                    }
                }
            }
        }
	}

	private IEnumerator flashText() {
		while(true) {
			joinText.text = "HOLD A BUTTON TO JOIN!";
            yield return new WaitForSeconds(flashSpeed);
			joinText.text = "";
			yield return new WaitForSeconds(flashSpeedBlank);
		}
	}

	private IEnumerator countDown() {
		for (int i = countDownTime; i >= 0; i--) {
			countDownText.text = i.ToString();
			yield return new WaitForSeconds(1);
		}

		if (CurrentPlayerKeys.Instance.players.Count > 0) {
            LevelTypeManager.loadLevel();
            //Application.LoadLevel("RunScene");
		}
		else {
			Application.LoadLevel("JoinScene");
		}

	}

	void setFloorSprite(GameObject gObj)
	{
		int randomTile = Random.Range(0, floorSprites.Length);
		SpriteRenderer sr = gObj.GetComponent<SpriteRenderer>();
		sr.sprite = floorSprites[randomTile];
	}
}
