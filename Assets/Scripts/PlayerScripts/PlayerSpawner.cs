using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;
    public float offset = 0.0f;
    public float startPoint = 0.0f;

	private List<int> chosenColours;
    private Dictionary<KeyCode, float> enteringPlayers;
    private Dictionary<KeyCode, TemporarySound> keysSound;

    private Dictionary<KeyCode, GameObject> mInactivePlayers;


	public float timeToHold = 2;

    float lastFrameTime = 0.0f;


	// Use this for initialization
	void Start () {
		chosenColours = new List<int>();
        enteringPlayers = new Dictionary<KeyCode, float>();
        keysSound = new Dictionary<KeyCode, TemporarySound>();

        mInactivePlayers = new Dictionary<KeyCode, GameObject>();

        startPoint = Camera.main.transform.position.x;

		for (int i = 0; i < CurrentPlayerKeys.Instance.players.Count; i++) {
			KeyCode kc = CurrentPlayerKeys.Instance.players[i].Key;
			GameObject player = (GameObject) Instantiate(playerPrefab);
			player.name = kc.ToString();
			player.GetComponent<PlayerMovement>().setJumpKey(kc);

			//Random player's spawn position near the middle
            offset = Random.Range(-2.5f, 2.5f);
            player.transform.position = new Vector3(startPoint + offset, 0.59f, 0);

			player.GetComponent<PlayerMovement>().playerColour = CurrentPlayerKeys.Instance.players[i].Value;
		}

        if (CurrentPlayerKeys.Instance.players.Count==0)
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

    public void respawnPlayer(KeyCode _kc, Color _col) {
        GameObject player = mInactivePlayers[_kc];
        mInactivePlayers.Remove(_kc);
        player.SetActive(true);
            
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(VariableSpeed.current, 10);

        offset = Random.Range(-1f, 1f);
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
        player.transform.position = new Vector3(spawnPos.x + offset, 0.59f, 0);

        player.GetComponent<PlayerMovement>().playerColour = _col;
        player.GetComponent<PlayerPowerups>().createPowerupEffect();

        if(LevelTypeManager.currentLevel != LevelTypeManager.Level.flappyBird)
            player.GetComponent<PlayerMovement>().animationBoard.FlappyMode = false;

        GameObject wc = GameObject.Find("WinnerChecker");
        WinnerChecker wcscript = wc.GetComponent<WinnerChecker>();
        wcscript.addPlayer(player.name);
    }

    public void registerInactivePlayer(GameObject _player, KeyCode _kc) {
        mInactivePlayers[_kc] = _player;
    }
	
	// Update is called once per frame
	void Update () 
    {
        float realDeltaTime = Time.realtimeSinceStartup - lastFrameTime;
        lastFrameTime = Time.realtimeSinceStartup;
        //Debug.Log(realDeltaTime);
        //Debug.Log(Time.timeSinceLevelLoad);
        //Debug.Log(Time.time);
        //Debug.Log(Time.realtimeSinceStartup);

        if (CurrentPlayerKeys.Instance.colourNumbers.Count > 0)
        {
            for (int i = 0; i < MenuScript.keyCodes.Length; i++)
            {
                if (Input.GetKey(MenuScript.keyCodes[i]) &&
                    !CurrentPlayerKeys.Instance.playerExists(MenuScript.keyCodes[i]))
                {
                    float temp = 0;
                    if (enteringPlayers.TryGetValue(MenuScript.keyCodes[i], out temp))
                    {

                        //Debug.Log(enteringPlayers[MenuScript.keyCodes[i]]);
                        //Debug.Log(Time.deltaTime);
                        //enteringPlayers[MenuScript.keyCodes[i]] += Time.deltaTime;
                        enteringPlayers[MenuScript.keyCodes[i]] += realDeltaTime;

                        if (enteringPlayers[MenuScript.keyCodes[i]] >= timeToHold)
                        {
                            Debug.Log("Adding player " + MenuScript.keyCodes[i].ToString());
                            //Spawn stuff here								
                            GameObject player = (GameObject)Instantiate(playerPrefab);
                            player.name = MenuScript.keyCodes[i].ToString();
                            player.GetComponent<PlayerMovement>().setJumpKey(MenuScript.keyCodes[i]);
                            //Random player's spawn position near the middle
                            Vector3 screenMid = new Vector3(0.5f, 0, 0);
                            startPoint = Camera.main.ViewportToWorldPoint(screenMid).x;
                            offset = Random.Range(-1, 1);
                            player.transform.position = new Vector3(startPoint + offset, 0.59f, 0);
                            Debug.Log("Added player " + player.name);
                            Debug.Log(player.transform.position);

                            GameObject wc = GameObject.Find("WinnerChecker");
                            WinnerChecker wcscript = wc.GetComponent<WinnerChecker>();
                            wcscript.addPlayer(player.name);

                            //Assign player color
                            int index = Random.Range(0,CurrentPlayerKeys.Instance.colourNumbers.Count);
                            int colourIndex = CurrentPlayerKeys.Instance.colourNumbers[index];
                            CurrentPlayerKeys.Instance.colourNumbers.RemoveAt(index);

                            CurrentPlayerKeys.Instance.players.Add(new KeyValuePair<KeyCode, Color>(MenuScript.keyCodes[i], 
                                                                            CurrentPlayerKeys.Instance.possibleColors[colourIndex]));

                            player.GetComponent<PlayerMovement>().playerColour = CurrentPlayerKeys.Instance.possibleColors[colourIndex];

                            enteringPlayers[MenuScript.keyCodes[i]] = -1000;

                        }
                    }
                    else
                    {
                        enteringPlayers.Add(MenuScript.keyCodes[i], 0);

                        GameObject tempSound = (GameObject)Instantiate(SoundManager.instance.tempSound);
                        TemporarySound ts = tempSound.GetComponent<TemporarySound>();
                        int rnd = Random.Range(0, SoundManager.instance.introSpawnSounds.Count);
                        ts.play(SoundManager.instance.introSpawnSounds[rnd],
                                SoundManager.instance.introSpawnVolume);
                        keysSound.Add(MenuScript.keyCodes[i], ts);
                    }
                }

                if (Input.GetKeyUp(MenuScript.keyCodes[i]) &&
                    !CurrentPlayerKeys.Instance.playerExists(MenuScript.keyCodes[i]))
                {
                    
                    float temp = -1000;
                    if (enteringPlayers.TryGetValue(MenuScript.keyCodes[i], out temp))
                    {
                        if (temp < timeToHold && temp > -900)
                        {
                            enteringPlayers.Remove(MenuScript.keyCodes[i]);

                            TemporarySound ts = null;
                            bool success = keysSound.TryGetValue(MenuScript.keyCodes[i], out ts);
                            if (success)
                            {
                                ts.stop();
                                keysSound.Remove(MenuScript.keyCodes[i]);
                            }
                        }
                    }
                }
            }
        }
	}
}
