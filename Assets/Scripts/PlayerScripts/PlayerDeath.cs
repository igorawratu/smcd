using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {

    public GameObject bloodEffect;
    public GameObject deathSoundObject;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector2 pos = transform.position;
        Vector2 cameraLeftPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        if (pos.x < cameraLeftPos.x)
        {
            GameObject winnerChecker = GameObject.Find("WinnerChecker");
            WinnerChecker script = winnerChecker.GetComponent<WinnerChecker>();
            script.removePlayer(gameObject.name);

            GameObject itemGenerator = GameObject.Find("ItemGenerator");
            GenerateItems igScript = itemGenerator.GetComponent<GenerateItems>();
            PlayerMovement pm = gameObject.GetComponent<PlayerMovement>();

            igScript.playerDied(pm.playerKey, gameObject.GetComponent<PlayerMovement>().playerColour);

            gameObject.SetActive(false);
            Instantiate(bloodEffect,
                new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, bloodEffect.transform.position.z),
                bloodEffect.transform.rotation);
            RandomShake.randomShake.PlayShakeX();

            Instantiate(deathSoundObject,transform.position,transform.rotation);

        }
	}
}
